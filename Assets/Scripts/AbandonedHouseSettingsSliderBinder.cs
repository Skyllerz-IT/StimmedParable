using UnityEngine;
using UnityEngine.UI;

public class AbandonedHouseSettingsSliderBinder : MonoBehaviour
{
    public enum SettingType { Sensitivity, Brightness, Audio }
    public SettingType settingType;
    public Slider slider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Awake()
    {
        if (slider == null)
            slider = GetComponent<Slider>();
    }

    void OnEnable()
    {
        if (SettingsManager.Instance != null && slider != null)
        {
            slider.onValueChanged.RemoveAllListeners();

            switch (settingType)
            {
                case SettingType.Sensitivity:
                    slider.value = SettingsManager.Instance.Sensitivity;
                    slider.onValueChanged.AddListener(SettingsManager.Instance.OnSensitivityChanged);
                    break;
                case SettingType.Brightness:
                    slider.value = SettingsManager.Instance.Brightness;
                    slider.onValueChanged.AddListener(SettingsManager.Instance.OnBrightnessChanged);
                    break;
                case SettingType.Audio:
                    slider.value = SettingsManager.Instance.AudioVolume;
                    slider.onValueChanged.AddListener(SettingsManager.Instance.OnAudioChanged);
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
