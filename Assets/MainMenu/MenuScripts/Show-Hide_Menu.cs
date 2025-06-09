using UnityEngine;

public class ShowHideMenu : MonoBehaviour
{
    public GameObject settingsPanel; // Assign in Inspector

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Call this from the X button
    public void HideSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    // Call this from the Settings button
    public void ShowSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }
}
