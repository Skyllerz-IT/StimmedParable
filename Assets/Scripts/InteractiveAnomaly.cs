using System.Collections.Generic;
using UnityEngine;

public class InteractiveAnomaly : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] protected float interactionDistance = 2f;
    [SerializeField] protected KeyCode interactKey = KeyCode.E;
    [SerializeField] private SpriteRenderer interactionPrompt;

    [SerializeField] private GameObject target;
    [SerializeField] private Component component;

    [SerializeField] private RandomBlink blinkingLight;

    [SerializeField] protected bool hasBeenInteracted = false;
    private static int totalAnomalies = 0;
    private Camera mainCamera;
    public AnomalyMessageUI anomalyMessageUI;
    public AnomalyUIManager anomalyUIManager; // Add this line

    [SerializeField] private ParticleSystem interactParticles;

    [Header("Anomaly Settings")]
    public int anomalyID;
    [Tooltip("GameObject to show when the anomaly is solved (optional)")]
    public GameObject solvedObject;

    private static Dictionary<int, bool> anomalyFoundDict = new Dictionary<int, bool>();
    private static bool staticsInitialized = false;

    private void Start()
    {
        /*if (!staticsInitialized)
        {
            ResetStatics();
            staticsInitialized = true;
        }
        totalAnomalies++; this is moved to Awake to ensure it counts all instances correctly
*/
        mainCamera = Camera.main;

        if (interactionPrompt != null)
        {
            interactionPrompt.enabled = false;
        }

        if (interactParticles != null)
            interactParticles.gameObject.SetActive(false);
    }

    private void Awake()
    {
        if (!staticsInitialized)
        {
            ResetStatics();
            staticsInitialized = true;
        }

        totalAnomalies++;
    }

    public virtual void Interact()
    {
        if (!hasBeenInteracted)
        {
            hasBeenInteracted = true;
            anomalyFoundDict[anomalyID] = true;
            Debug.Log($"Anomaly {anomalyID} found! Progress: {GetAnomalyCount()}/{totalAnomalies} anomalies checked. {totalAnomalies - GetAnomalyCount()} remaining.");

            if (anomalyMessageUI != null)
                anomalyMessageUI.ShowMessage("You found an anomaly");

            if (anomalyUIManager != null)
                anomalyUIManager.UpdateAnomalyProgress(); // Add this line

            if (interactionPrompt != null)
                interactionPrompt.enabled = false;

            if (interactParticles != null)
            {
                interactParticles.gameObject.SetActive(true);
                interactParticles.Play();
            }

            if (component != null)
            {
                Destroy(component);
            }

            OnAnomalyInteracted();
        }
        else
        {
            if (anomalyMessageUI != null)
                anomalyMessageUI.ShowAlreadyFoundMessage();
        }
    }

    protected virtual void OnAnomalyInteracted()
    {
        if (target != null)
            target.SetActive(false);
        if (solvedObject != null)
            solvedObject.SetActive(true);

        if (interactParticles != null)
            interactParticles.Play();
    }

    public static int GetAnomalyCount()
    {
        int count = 0;
        foreach (var kvp in anomalyFoundDict)
        {
            if (kvp.Value) count++;
        }
        return count;
    }

    public static int GetTotalAnomalies()
    {
        return totalAnomalies;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }

    public static void SetAnomalyCounts(int found, int total)
    {
        totalAnomalies = total;
    }

    public void SetInteractedState(bool state)
    {
        hasBeenInteracted = state;
        if (state)
        {
            if (interactionPrompt != null)
                interactionPrompt.enabled = false;

            if (interactParticles != null)
            {
                interactParticles.gameObject.SetActive(true);
                interactParticles.Play();
            }

            if (target != null)
                target.SetActive(false);
        }
        if (anomalyUIManager != null)
            anomalyUIManager.UpdateAnomalyProgress(); // Add this line
    }

    public static List<int> GetFoundAnomalyIDs()
    {
        List<int> found = new List<int>();
        foreach (var kvp in anomalyFoundDict)
        {
            if (kvp.Value)
                found.Add(kvp.Key);
        }
        return found;
    }

    public static void SetFoundAnomalyIDs(List<int> ids)
    {
        anomalyFoundDict.Clear();
        foreach (var id in ids)
            anomalyFoundDict[id] = true;
    }

    public void RestoreStateFromDict()
    {
        bool found = anomalyFoundDict.ContainsKey(anomalyID) && anomalyFoundDict[anomalyID];
        hasBeenInteracted = found;
        SetInteractedState(found);
    }

    public static void ResetStatics()
    {
        anomalyFoundDict.Clear();
        totalAnomalies = 0;
        staticsInitialized = false;
        foreach (var anomaly in GameObject.FindObjectsOfType<InteractiveAnomaly>())
        {
            anomaly.hasBeenInteracted = false;
            anomaly.SetInteractedState(false);
        }
    }
}