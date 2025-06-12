using UnityEngine;

public class GameWinManager : MonoBehaviour
{
    public GameObject gameWinPanel; // Assign your GameWin Panel in the Inspector

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if all anomalies are completed
        int total = InteractiveAnomaly.GetTotalAnomalies();
        int found = InteractiveAnomaly.GetAnomalyCount();

        if (total > 0 && found == total)
        {
            if (gameWinPanel != null && !gameWinPanel.activeSelf)
            {
                gameWinPanel.SetActive(true);
            }
        }
    }
}
