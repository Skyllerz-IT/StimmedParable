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

    private Volume globalVolume;
    private LiftGammaGain liftGammaGain;
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
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private System.Collections.IEnumerator FindVolumeWhenReady()
    {
        // Wait until the end of the frame to ensure all objects are initialized
        yield return new WaitForEndOfFrame();

        // Try to find the Volume in the current scene
        FindAndSetupVolume();

        // If not found, keep trying for a few frames (optional)
        int tries = 0;
        while (globalVolume == null && tries < 10)
        {
            yield return new WaitForSeconds(0.1f);
            FindAndSetupVolume();
            tries++;
        }
    }

    private void FindAndSetupVolume()
    {
        globalVolume = FindFirstObjectByType<Volume>();
        if (globalVolume != null && globalVolume.profile != null)
        {
            // Try to get ColorAdjustments
            if (globalVolume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
            {
                Debug.Log("Found Volume and ColorAdjustments!");
                SetBrightness(Brightness);
            }
            else
            {
                colorAdjustments = null;
                Debug.LogWarning("Could not find ColorAdjustments!");
            }
        }
    }

    public void OnSensitivityChanged(float value)
    {
        Debug.Log("OnSensitivityChanged CALLED with value: " + value);
        value = Mathf.Max(1f, value);
        Sensitivity = value;
        PlayerPrefs.SetFloat("Sensitivity", value);
        PlayerPrefs.Save();
        Debug.Log("Sensitivity updated to: " + Sensitivity);
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
        // Apply to your brightness system here
        if (colorAdjustments != null)
        {
            colorAdjustments.postExposure.overrideState = true;
            colorAdjustments.postExposure.value = Mathf.Lerp(-2f, 2f, (value - 1f) / (2f - 1f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        // No slider references in SettingsManager, so no need to update Debug.Log
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FindVolumeWhenReady());
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
