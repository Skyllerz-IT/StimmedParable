using UnityEngine;

public class InteractiveAnomaly : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] protected float interactionDistance = 2f;
    [SerializeField] protected KeyCode interactKey = KeyCode.E;
    [SerializeField] private SpriteRenderer interactionPrompt;  // Changed to SpriteRenderer
    
    protected bool hasBeenInteracted = false;
    private static int anomaliesFound = 0;  // Static counter shared across all anomalies
    private static int totalAnomalies = 0;  // Total number of anomaly objects in the scene
    private Camera mainCamera;
    public AnomalyMessageUI anomalyMessageUI; // Assign in Inspector
    
    private void Start()
    {
        totalAnomalies++;  // Increment total when an anomaly object is created
        mainCamera = Camera.main;
        
        // Hide interaction prompt at start
        if (interactionPrompt != null)
        {
            interactionPrompt.enabled = false;
        }
    }
    
    // Interaction is now triggered externally (e.g., by a raycast/crosshair script)
    public virtual void Interact()
    {
        if (!hasBeenInteracted)
        {
            hasBeenInteracted = true;
            anomaliesFound++;
            Debug.Log($"Anomaly found! Progress: {anomaliesFound}/{totalAnomalies} anomalies checked. {totalAnomalies - anomaliesFound} remaining.");
            
            if (anomalyMessageUI != null)
                anomalyMessageUI.ShowMessage("You found an anomaly");
            
            if (interactionPrompt != null)
                interactionPrompt.enabled = false;
            
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
        // Base implementation does nothing
    }
    
    // Public method to get the current anomaly count
    public static int GetAnomalyCount()
    {
        return anomaliesFound;
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
} 