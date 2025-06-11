using UnityEngine.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    public float Sensitivity = 5f;
    public float Brightness = 0f;
    public float AudioVolume = 1f;

    [Header("Post Processing")]
    public Volume globalVolume; // Assign this in the inspector to ensure we use the correct volume

    private ColorAdjustments colorAdjustments;

    void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        // Reload settings
        Sensitivity = PlayerPrefs.GetFloat("Sensitivity", 10f);
        AudioVolume = PlayerPrefs.GetFloat("AudioVolume", 1f);
        Brightness = PlayerPrefs.GetFloat("Brightness", 1f);

        SceneManager.sceneLoaded += OnSceneLoaded;
        
        // Setup volume immediately
        SetupVolume();
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void SetupVolume()
    {
        // If not assigned in Inspector, try to find the Volume with the correct profile name
        if (globalVolume == null)
        {
            var allVolumes = Object.FindObjectsByType<Volume>(FindObjectsSortMode.None);
            foreach (var vol in allVolumes)
            {
                if (vol.profile != null && vol.profile.name == "Global Volume Profile")
                {
                    globalVolume = vol;
                    Debug.Log($"[SettingsManager] Assigned globalVolume: {globalVolume.gameObject.name} with profile '{vol.profile.name}'");
                    break;
                }
            }
            if (globalVolume == null)
            {
                Debug.LogWarning("[SettingsManager] No Volume with profile 'Global Volume Profile' found in scene!");
            }
        }
        if (globalVolume != null && globalVolume.profile != null)
        {
            // Try to get ColorAdjustments
            if (globalVolume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
            {
                SetBrightness(Brightness);
            }
            else
            {
                colorAdjustments = null;
                Debug.LogWarning("Could not find ColorAdjustments!");
            }
        }
        else
        {
            Debug.LogWarning("Global Volume not assigned in SettingsManager!");
        }
    }

    public void OnSensitivityChanged(float value)
    {
        value = Mathf.Max(1f, value);
        Sensitivity = value;
        PlayerPrefs.SetFloat("Sensitivity", value);
        PlayerPrefs.Save();
    }

    public void OnBrightnessChanged(float value)
    {
        Brightness = value;
        PlayerPrefs.SetFloat("Brightness", value);
        PlayerPrefs.Save();
        SetBrightness(value);
    }

    public void OnAudioChanged(float value)
    {
        AudioVolume = value;
        PlayerPrefs.SetFloat("AudioVolume", value);
        PlayerPrefs.Save();
    }

    public void SetBrightness(float value)
    {
        Brightness = value;
        if (colorAdjustments != null)
        {
            colorAdjustments.postExposure.overrideState = true;
            // Map the slider value (0-1) to postExposure range (-2 to 2)
            colorAdjustments.postExposure.value = Mathf.Lerp(-2f, 2f, value);
            Debug.Log($"Setting brightness to {value}, postExposure to {colorAdjustments.postExposure.value}");
        }
        else
        {
            Debug.LogWarning("Cannot set brightness - ColorAdjustments not found!");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetupVolume();
    }

    public void SetAudioVolume(float value)
    {
        AudioVolume = value;
        // Apply to your audio system here
    }

    public void SetSensitivity(float value)
    {
        Sensitivity = value;
        // Apply to your input system here
    }
}
