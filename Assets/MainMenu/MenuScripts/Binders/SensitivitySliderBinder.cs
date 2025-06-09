using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SensitivitySliderBinder : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Slider slider = GetComponent<Slider>();
        slider.onValueChanged.RemoveAllListeners();
        slider.onValueChanged.AddListener(OnSensitivityChanged);

        // Set slider value to current setting at start
        var settings = SettingsManager.Instance;
        if (settings != null)
            slider.value = settings.Sensitivity;
    }

    void OnSensitivityChanged(float value)
    {
        PlayerPrefs.SetFloat("Sensitivity", value);
        PlayerPrefs.Save();

        var settings = SettingsManager.Instance;
        if (settings != null)
            settings.SetSensitivity(value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
