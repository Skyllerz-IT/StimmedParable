using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EnemyController : MonoBehaviour
{
    [Header("Spawn Points (Assign in Inspector)")]
    public Transform[] spawnPoints;

    [Header("Teleport Timing (seconds)")]
    public float minWaitTime = 5f;
    public float maxWaitTime = 10f;

    [Header("Post-Processing")]
    public Volume postProcessVolume; // Assign in Inspector
    private ColorAdjustments colorAdjustments;
    private float effectLerp = 0f;
    private bool effectFullyWhite = false;

    private int currentSpawnIndex = -1;
    private Coroutine teleportRoutine;

    private GameTimer gameTimer;
    private Camera playerCamera;
    private PlayerMove playerMove;
    private float stareTimer = 0f;
    public float stareTimeToDie = 3f;
    public float lookMaxDistance = 100f;

    private bool playerLookingAtEnemy = false;

    // Flicker settings
    private float flickerSpeed = 20f; // Flicker frequency
    private float flickerAmount = 0.3f; // Flicker intensity

    void Awake()
    {
        // Try to find the main camera and player script
        playerCamera = Camera.main;
        if (playerCamera != null)
        {
            playerMove = playerCamera.GetComponentInParent<PlayerMove>();
            if (playerMove == null)
            {
                playerMove = Object.FindFirstObjectByType<PlayerMove>();
            }
        }
        else
        {
            Debug.LogWarning("EnemyController: No main camera found.");
        }
    }

    void Start()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("EnemyController: No spawn points assigned.");
            enabled = false;
            return;
        }
        TeleportToRandomSpawnPoint();
        teleportRoutine = StartCoroutine(TeleportLoop());

        // Get ColorAdjustments from the volume profile
        if (postProcessVolume != null && postProcessVolume.profile.TryGet(out colorAdjustments))
        {
            colorAdjustments.active = true;
        }
        else
        {
            Debug.LogWarning("EnemyController: No ColorAdjustments override found in the assigned Volume profile.");
        }

        gameTimer = FindFirstObjectByType<GameTimer>();
    }

    void Update()
    {
        if (playerCamera == null || playerMove == null) return;
        // Check if enemy is on screen
        Vector3 viewportPos = playerCamera.WorldToViewportPoint(transform.position);
        bool onScreen = viewportPos.z > 0 && viewportPos.x > 0 && viewportPos.x < 1 && viewportPos.y > 0 && viewportPos.y < 1;
        bool visible = false;
        if (onScreen)
        {
            Vector3 dir = (transform.position - playerCamera.transform.position).normalized;
            float dist = Vector3.Distance(playerCamera.transform.position, transform.position);
            Ray ray = new Ray(playerCamera.transform.position, dir);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, dist + 0.1f))
            {
                if (hit.transform == this.transform)
                {
                    visible = true;
                }
            }
        }
        playerLookingAtEnemy = visible;
        if (visible)
        {
            stareTimer += Time.deltaTime;
            if (playerLookingAtEnemy && effectLerp >= 0.99f && !effectFullyWhite)
            {
                if (stareTimer >= stareTimeToDie)
                {
                    effectFullyWhite = true;
                    //call death logic here
                    Debug.Log("Player has stared at the enemy long enough to die.");
                }
            }
        }
        else
        {
            stareTimer = 0f;
        }

        // --- Post-processing effect logic with flicker ---
        float targetLerp = playerLookingAtEnemy ? Mathf.Clamp01(stareTimer / stareTimeToDie) : 0f;
        effectLerp = Mathf.MoveTowards(effectLerp, targetLerp, Time.deltaTime / 0.5f); // 0.5s smooth return

        float flicker = 0f;
        if (playerLookingAtEnemy)
        {
            flicker = Mathf.Sin(Time.time * flickerSpeed) * flickerAmount * effectLerp;
        }

        if (colorAdjustments != null)
        {
            colorAdjustments.postExposure.value = Mathf.Lerp(0f, 3f, effectLerp) + flicker;
            colorAdjustments.saturation.value = Mathf.Lerp(0f, -100f, effectLerp) + flicker * 10f;
        }
        if (!playerLookingAtEnemy)
        {
            effectFullyWhite = false;
        }
    }

    IEnumerator TeleportLoop()
    {
        while (true)
        {
            float progress = gameTimer != null ? gameTimer.GetProgress() : 0f;
            // As time progresses, min/max wait times decrease by up to 50%
            float min = Mathf.Lerp(minWaitTime, minWaitTime * 0.5f, progress);
            float max = Mathf.Lerp(maxWaitTime, maxWaitTime * 0.5f, progress);

            float waitTime = Random.Range(min, max);
            yield return new WaitForSeconds(waitTime);

            if (!playerLookingAtEnemy)
            {
                TeleportToRandomSpawnPoint();
            }
        }
    }

    void TeleportToRandomSpawnPoint()
    {
        if (spawnPoints.Length == 0) return;
        int newIndex = currentSpawnIndex;
        // Ensure a different spawn point is chosen
        while (spawnPoints.Length > 1 && newIndex == currentSpawnIndex)
        {
            newIndex = Random.Range(0, spawnPoints.Length);
        }
        currentSpawnIndex = newIndex;
        Transform target = spawnPoints[currentSpawnIndex];
        transform.position = target.position;
        transform.rotation = target.rotation;
    }
}