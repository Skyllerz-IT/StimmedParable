using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EnemyEyeController : MonoBehaviour
{
    [Header("Spawn Points (Assign in Inspector)")]
    public Transform[] spawnPoints;

    [Header("Movement")]
    public float moveSpeed = 3f;

    [Header("Teleport Timing (seconds)")]
    public float minWaitTime = 5f;
    public float maxWaitTime = 10f;

    [Header("Chase Settings")]
    public float chaseDurationAfterLook = 2f;
    public float respawnCooldown = 2f;

    [Header("Look/Death")]
    public float stareTimeToDie = 3f;

    [Header("Post-Processing")]
    public Volume enemyVolume;
    private Vignette vignette;
    private float effectLerp = 0f;
    private bool effectFullyWhite = false;

    [Header("Chase Music")]
    public AudioSource musicSource;
    public AudioClip chaseMusic;
    public AudioClip normalMusic;

    private Camera playerCamera;
    private Transform playerTransform;
    private float stareTimer = 0f;
    private bool playerLookingAtEnemy = false;
    private Coroutine teleportRoutine;
    private int currentSpawnIndex = -1;

    [Header("Eyeball")]
    public Transform eyeball; // Assign your eyeball child here
    
    [Header("GameOver")]
    public GameObject gameOverUI;
    public GameObject HUD;
    
    // Chase/cooldown state
    private bool isChasing = false;
    private float chaseTimer = 0f;
    private bool isCooldown = false;

    // Flicker settings
    private float flickerSpeed = 20f;
    private float flickerAmount = 0.3f;

    void OnDrawGizmos()
    {
        if (spawnPoints == null) return;
        Gizmos.color = Color.red;
        foreach (var point in spawnPoints)
        {
            if (point != null)
                Gizmos.DrawSphere(point.position, 0.3f);
        }
    }

    void Awake()
    {
        playerCamera = Camera.main;
        if (playerCamera != null)
            playerTransform = playerCamera.transform;
        else
            Debug.LogWarning("EnemyEyeController: No main camera found.");
    }

    void Start()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("EnemyEyeController: No spawn points assigned.");
            enabled = false;
            return;
        }
        TeleportSmartly();
        teleportRoutine = StartCoroutine(TeleportLoop());

        if (enemyVolume != null && enemyVolume.profile.TryGet(out vignette))
            vignette.active = true;
        else
            Debug.LogWarning("EnemyEyeController: No Vignette override found in the assigned enemy Volume profile.");

        // Start normal music
        if (musicSource != null && normalMusic != null)
        {
            musicSource.clip = normalMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    void Update()
    {
        if (playerCamera == null) return;

        // Make eyeball look at camera
        if (eyeball != null)
            eyeball.LookAt(playerCamera.transform.position);

        playerLookingAtEnemy = IsPlayerLookingAtMe();

        if (!isChasing && !isCooldown)
        {
            if (playerLookingAtEnemy)
            {
                MoveTowardsPlayer();
                stareTimer += Time.deltaTime;
                if (effectLerp >= 0.99f && !effectFullyWhite && stareTimer >= stareTimeToDie)
                {
                    effectFullyWhite = true;
                    GameOver();
                    Debug.Log("Player stared at the enemy and died.");
                }
            }
            else
            {
                if (stareTimer > 0f)
                {
                    isChasing = true;
                    chaseTimer = chaseDurationAfterLook;
                    PlayChaseMusic();
                }
                stareTimer = 0f;
            }
        }
        else if (isChasing)
        {
            if (playerLookingAtEnemy)
            {
                chaseTimer = chaseDurationAfterLook;
                MoveTowardsPlayer();
            }
            else
            {
                chaseTimer -= Time.deltaTime;
                MoveTowardsPlayer();
                if (chaseTimer <= 0f)
                {
                    isChasing = false;
                    isCooldown = true;
                    if (musicSource != null)
                        musicSource.Stop(); // Stop music immediately
                    PlayNormalMusic();
                    StartCoroutine(CooldownAndRespawn());
                }
            }
        }

        // VFX: active if being looked at or chasing
        float targetLerp = 0f;
        if (playerLookingAtEnemy)
            targetLerp = Mathf.Clamp01(stareTimer / stareTimeToDie);
        else if (isChasing)
            targetLerp = 0.2f; // Less intense during chase
        effectLerp = Mathf.MoveTowards(effectLerp, targetLerp, Time.deltaTime / 0.5f);

        float flicker = (playerLookingAtEnemy || isChasing) ? Mathf.Sin(Time.time * flickerSpeed) * flickerAmount * effectLerp : 0f;
        if (vignette != null)
        {
            float baseIntensity = Mathf.Lerp(0f, 1f, effectLerp);
            vignette.intensity.value = Mathf.Clamp01(baseIntensity + flicker);
        }

        if (!playerLookingAtEnemy)
            effectFullyWhite = false;
    }

    IEnumerator CooldownAndRespawn()
    {
        // Move enemy far away during cooldown
        transform.position = Vector3.one * 9999f;
        yield return new WaitForSeconds(respawnCooldown);
        TeleportSmartly();
        isCooldown = false;
    }

    IEnumerator TeleportLoop()
    {
        while (true)
        {
            int found = InteractiveAnomaly.GetAnomalyCount();
            int total = Mathf.Max(InteractiveAnomaly.GetTotalAnomalies(), 1);
            float progress = Mathf.Clamp01((float)found / total);

            float min = Mathf.Lerp(minWaitTime, minWaitTime * 0.5f, progress);
            float max = Mathf.Lerp(maxWaitTime, maxWaitTime * 0.5f, progress);
            float waitTime = Random.Range(min, max);

            yield return new WaitForSeconds(waitTime);

            if (!playerLookingAtEnemy && !isChasing && !isCooldown)
                TeleportSmartly();
        }
    }

    void MoveTowardsPlayer()
    {
        if (playerTransform == null) return;
        Vector3 toPlayer = (playerTransform.position - transform.position).normalized;
        float step = moveSpeed * Time.deltaTime;
        transform.position += toPlayer * step;
    }

    // --- SMART TELEPORT LOGIC BELOW ---

    void TeleportSmartly()
    {
        if (spawnPoints.Length == 0 || playerTransform == null) return;

        // Gather spawn points not in line of sight
        var validPoints = new System.Collections.Generic.List<int>();
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (!IsVisibleToPlayer(spawnPoints[i].position))
                validPoints.Add(i);
        }

        // If all are visible, fallback to all points
        if (validPoints.Count == 0)
        {
            for (int i = 0; i < spawnPoints.Length; i++)
                validPoints.Add(i);
        }

        // Randomly choose between farthest or random
        int chosenIndex;
        if (Random.value < 0.5f)
        {
            // Farthest valid point
            float maxDist = float.MinValue;
            int farthest = validPoints[0];
            foreach (int idx in validPoints)
            {
                float dist = Vector3.Distance(playerTransform.position, spawnPoints[idx].position);
                if (dist > maxDist)
                {
                    maxDist = dist;
                    farthest = idx;
                }
            }
            chosenIndex = farthest;
        }
        else
        {
            // Random valid point
            chosenIndex = validPoints[Random.Range(0, validPoints.Count)];
        }

        currentSpawnIndex = chosenIndex;
        Transform target = spawnPoints[currentSpawnIndex];
        transform.position = target.position;
        transform.rotation = target.rotation;
    }

    bool IsVisibleToPlayer(Vector3 point)
    {
        if (playerCamera == null) return false;
        Vector3 dir = (point - playerCamera.transform.position).normalized;
        float dist = Vector3.Distance(playerCamera.transform.position, point);
        Ray ray = new Ray(playerCamera.transform.position, dir);
        if (Physics.Raycast(ray, out RaycastHit hit, dist + 0.1f))
        {
            // If the ray hits something before the point, it's not visible
            if ((hit.point - point).sqrMagnitude > 0.01f)
                return false;
        }
        // Check if within camera view
        Vector3 viewportPos = playerCamera.WorldToViewportPoint(point);
        return viewportPos.z > 0 && viewportPos.x > 0 && viewportPos.x < 1 && viewportPos.y > 0 && viewportPos.y < 1;
    }

    bool IsPlayerLookingAtMe()
    {
        if (playerCamera == null) return false;
        Vector3 dirToEnemy = (transform.position - playerCamera.transform.position).normalized;
        float dot = Vector3.Dot(playerCamera.transform.forward, dirToEnemy);
        float dist = Vector3.Distance(playerCamera.transform.position, transform.position);
        Ray ray = new Ray(playerCamera.transform.position, dirToEnemy);
        if (Physics.Raycast(ray, out RaycastHit hit, dist + 0.1f))
        {
            if (hit.transform != this.transform)
                return false;
        }
        return dot > 0.98f;
    }

    void PlayChaseMusic()
    {
        if (musicSource != null && chaseMusic != null)
        {
            musicSource.Stop();
            musicSource.clip = chaseMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    void PlayNormalMusic()
    {
        if (musicSource != null && normalMusic != null)
        {
            musicSource.Stop();
            musicSource.clip = normalMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameOver();
            Debug.Log("Player touched the enemy and died.");
        }
    }

    void GameOver()
    {
        HUD.SetActive(false);
        gameOverUI.SetActive(true);
    }
}