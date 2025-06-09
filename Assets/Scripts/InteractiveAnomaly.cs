using System.Collections.Generic;
using UnityEngine;

public class InteractiveAnomaly : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] protected float interactionDistance = 2f;
    [SerializeField] protected KeyCode interactKey = KeyCode.E;
    [SerializeField] private SpriteRenderer interactionPrompt;  // Changed to SpriteRenderer
    
    [SerializeField] private GameObject target;
    
    [SerializeField] protected bool hasBeenInteracted = false;  // Made serializable
    private static int totalAnomalies = 0;  // Total number of anomaly objects in the scene
    private Camera mainCamera;
    public AnomalyMessageUI anomalyMessageUI; // Assign in Inspector
    
    [SerializeField] private ParticleSystem interactParticles;

    [Header("Anomaly Settings")]
    public int anomalyID;

    private static Dictionary<int, bool> anomalyFoundDict = new Dictionary<int, bool>();
    private static bool staticsInitialized = false;

    private void Start()
    {
        // Only increment totalAnomalies once per scene load
        if (!staticsInitialized)
        {
            ResetStatics();
            staticsInitialized = true;
        }
        totalAnomalies++;

        mainCamera = Camera.main;
        
        // Hide interaction prompt at start
        if (interactionPrompt != null)
        {
            interactionPrompt.enabled = false;
        }

        if (interactParticles != null)
            interactParticles.gameObject.SetActive(false); // Deactivate at start
    }
    
    // Interaction is now triggered externally (e.g., by a raycast/crosshair script)
    public virtual void Interact()
    {
        if (!hasBeenInteracted)
        {
            hasBeenInteracted = true;
            anomalyFoundDict[anomalyID] = true;
            Debug.Log($"Anomaly {anomalyID} found! Progress: {GetAnomalyCount()}/{totalAnomalies} anomalies checked. {totalAnomalies - GetAnomalyCount()} remaining.");
            
            if (anomalyMessageUI != null)
                anomalyMessageUI.ShowMessage("You found an anomaly");
            
            if (interactionPrompt != null)
                interactionPrompt.enabled = false;
            
            if (interactParticles != null)
            {
                interactParticles.gameObject.SetActive(true);
                interactParticles.Play();
            }
            
            OnAnomalyInteracted();
        }
        else
        {
            if (anomalyMessageUI != null)
                anomalyMessageUI.ShowAlreadyFoundMessage();
        }
    }
    
    // Override this method in derived classes to add custom behavior
    protected virtual void OnAnomalyInteracted()
    {
        if (target != null)
        {
            target.SetActive(false);
        }
        
        if (interactParticles != null)
            interactParticles.Play();
    }
    
    // Public method to get the current anomaly count
    public static int GetAnomalyCount()
    {
        int count = 0;
        foreach (var kvp in anomalyFoundDict)
        {
            if (kvp.Value) count++;
        }
        return count;
    }
    
    // Public method to get the total number of anomalies
    public static int GetTotalAnomalies()
    {
        return totalAnomalies;
    }
    
    // Optional: Visualize interaction range in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }

    // New method to set anomaly counts from save data
    public static void SetAnomalyCounts(int found, int total)
    {
        totalAnomalies = total;
    }
    
    // New method to set individual anomaly state
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