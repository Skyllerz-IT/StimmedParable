using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class AudioSliderBinder : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Slider slider = GetComponent<Slider>();
        slider.onValueChanged.RemoveAllListeners();
        slider.onValueChanged.AddListener(OnAudioChanged);

        // Set slider value to current setting at start
        var settings = SettingsManager.Instance;
        if (settings != null)
            slider.value = settings.AudioVolume;
    }

    void OnAudioChanged(float value)
    {
        PlayerPrefs.SetFloat("AudioVolume", value);
        PlayerPrefs.Save();

        var settings = SettingsManager.Instance;
        if (settings != null)
            settings.SetAudioVolume(value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
