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
        TeleportToNearestSpawnPoint();
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
            targetLerp = 0.4f; // Less intense during chase
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
        TeleportToNearestSpawnPoint();
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
                TeleportToNearestSpawnPoint();
        }
    }

    void MoveTowardsPlayer()
    {
        if (playerTransform == null) return;
        Vector3 toPlayer = (playerTransform.position - transform.position).normalized;
        float step = moveSpeed * Time.deltaTime;
        transform.position += toPlayer * step;
    }

    void TeleportToNearestSpawnPoint()
    {
        if (spawnPoints.Length == 0 || playerTransform == null) return;

        float minDist = float.MaxValue;
        int nearestIndex = 0;
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            float dist = Vector3.Distance(playerTransform.position, spawnPoints[i].position);
            if (dist < minDist)
            {
                minDist = dist;
                nearestIndex = i;
            }
        }
        currentSpawnIndex = nearestIndex;
        Transform target = spawnPoints[currentSpawnIndex];
        transform.position = target.position;
        transform.rotation = target.rotation;
    }

    void TeleportToFarthestSpawnPoint()
    {
        if (spawnPoints.Length == 0 || playerTransform == null) return;

        float maxDist = float.MinValue;
        int farthestIndex = 0;
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            float dist = Vector3.Distance(playerTransform.position, spawnPoints[i].position);
            if (dist > maxDist)
            {
                maxDist = dist;
                farthestIndex = i;
            }
        }
        currentSpawnIndex = farthestIndex;
        Transform target = spawnPoints[currentSpawnIndex];
        transform.position = target.position;
        transform.rotation = target.rotation;
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
            Debug.Log("Player touched the enemy and died.");
        }
    }
}