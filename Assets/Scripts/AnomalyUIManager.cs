using TMPro;
using UnityEngine;

public class AnomalyUIManager : MonoBehaviour
{
    public TextMeshProUGUI anomalyProgressText;

    void Start()
    {
        UpdateAnomalyProgress();
    }

    public void UpdateAnomalyProgress()
    {
        int found = InteractiveAnomaly.GetAnomalyCount();
        int total = InteractiveAnomaly.GetTotalAnomalies();
        anomalyProgressText.text = $"Anomalies Found {found}/{total} ";
    }
}