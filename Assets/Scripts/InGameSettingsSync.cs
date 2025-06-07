using UnityEngine;
using UnityEngine.UI;

public class InGameSettingsSync : MonoBehaviour
{
    public Slider sensitivitySlider;
    public Slider brightnessSlider;
    public Slider audioSlider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        if (SettingsManager.Instance != null)
        {
            sensitivitySlider.value = SettingsManager.Instance.Sensitivity;
            brightnessSlider.value = SettingsManager.Instance.Brightness;
            audioSlider.value = SettingsManager.Instance.AudioVolume;
        }
    }
}
