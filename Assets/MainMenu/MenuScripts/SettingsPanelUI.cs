using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelUI : MonoBehaviour
{
    public Slider sensitivitySlider;
    public Slider brightnessSlider;
    public Slider audioSlider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnEnable()
    {
        // Set sliders to current values
        if (SettingsManager.Instance != null)
        {
            sensitivitySlider.value = SettingsManager.Instance.Sensitivity;
            brightnessSlider.value = SettingsManager.Instance.Brightness;
            audioSlider.value = SettingsManager.Instance.AudioVolume;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
