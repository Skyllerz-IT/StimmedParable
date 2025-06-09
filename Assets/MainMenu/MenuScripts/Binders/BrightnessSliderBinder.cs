using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class BrightnessSliderBinder : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Slider slider = GetComponent<Slider>();
        slider.onValueChanged.RemoveAllListeners();
        slider.onValueChanged.AddListener(OnBrightnessChanged);

        // Set slider value to current setting at start
        var settings = SettingsManager.Instance;
        if (settings != null)
            slider.value = settings.Brightness;
    }

    void OnBrightnessChanged(float value)
    {
        PlayerPrefs.SetFloat("Brightness", value);
        PlayerPrefs.Save();

        var settings = SettingsManager.Instance;
        if (settings != null)
            settings.SetBrightness(value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
