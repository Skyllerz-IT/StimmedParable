using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Slider audioSlider;
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private Slider sensitivitySlider;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Subscribe to scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Unsubscribe from scene loaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene loaded: {scene.name}");
        ReconnectUIElements();
    }

    private void ReconnectUIElements()
    {
        // Find and reconnect UI elements in the current scene
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            // Find Load Game button
            loadGameButton = GameObject.Find("LoadGameButton")?.GetComponent<Button>();
            if (loadGameButton != null)
            {
                loadGameButton.onClick.RemoveAllListeners();
                loadGameButton.onClick.AddListener(LoadGame);
            }

            // Find Quit button
            quitButton = GameObject.Find("QuitButton")?.GetComponent<Button>();
            if (quitButton != null)
            {
                quitButton.onClick.RemoveAllListeners();
                quitButton.onClick.AddListener(QuitApplication);
            }

            // Find and reconnect settings sliders
            audioSlider = GameObject.Find("AudioSlider")?.GetComponent<Slider>();
            brightnessSlider = GameObject.Find("BrightnessSlider")?.GetComponent<Slider>();
            sensitivitySlider = GameObject.Find("SensitivitySlider")?.GetComponent<Slider>();

            // Reconnect slider events if they exist
            if (audioSlider != null)
            {
                audioSlider.onValueChanged.RemoveAllListeners();
                audioSlider.onValueChanged.AddListener(OnAudioVolumeChanged);
            }
            if (brightnessSlider != null)
            {
                brightnessSlider.onValueChanged.RemoveAllListeners();
                brightnessSlider.onValueChanged.AddListener(OnBrightnessChanged);
            }
            if (sensitivitySlider != null)
            {
                sensitivitySlider.onValueChanged.RemoveAllListeners();
                sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
            }
        }
    }

    // Settings handlers
    private void OnAudioVolumeChanged(float value)
    {
        // Implement your audio volume change logic here
        PlayerPrefs.SetFloat("AudioVolume", value);
        PlayerPrefs.Save();
    }

    private void OnBrightnessChanged(float value)
    {
        // Implement your brightness change logic here
        PlayerPrefs.SetFloat("Brightness", value);
        PlayerPrefs.Save();
    }

    private void OnSensitivityChanged(float value)
    {
        // Implement your sensitivity change logic here
        PlayerPrefs.SetFloat("Sensitivity", value);
        PlayerPrefs.Save();
    }

    // Static method to get the GameManager instance
    public static GameManager GetInstance()
    {
        if (Instance == null)
        {
            Debug.LogWarning("GameManager instance not found! Make sure it exists in the main menu scene.");
        }
        return Instance;
    }

    public void SaveGame()
    {
        SaveSystem.SaveGame();
    }

    public void LoadGame()
    {
        Debug.Log("Attempting to load game...");
        
        if (!SaveSystem.SaveExists())
        {
            Debug.LogWarning("No save file exists to load!");
            return;
        }

        SaveSystem.SaveData saveData = SaveSystem.LoadGame();
        if (saveData != null)
        {
            Debug.Log($"Loading scene: {saveData.currentScene}");

            // Reset statics and set found IDs BEFORE loading the scene
            InteractiveAnomaly.ResetStatics();
            InteractiveAnomaly.SetFoundAnomalyIDs(saveData.foundAnomalyIDs);

            // Load the saved scene
            SceneManager.sceneLoaded += OnSceneLoadedRestoreAnomalies;
            SceneManager.LoadScene(saveData.currentScene);
        }
        else
        {
            Debug.LogError("Failed to load save data!");
        }
    }

    // This method will be called after the scene is loaded
    private void OnSceneLoadedRestoreAnomalies(Scene scene, LoadSceneMode mode)
    {
        // Set the anomaly counts (optional, as statics are now correct)
        // InteractiveAnomaly.SetAnomalyCounts(saveData.anomaliesFound, saveData.totalAnomalies);

        // Restore anomaly states
        foreach (var anomaly in FindObjectsOfType<InteractiveAnomaly>())
        {
            anomaly.RestoreStateFromDict();
        }
        // Unsubscribe to avoid duplicate calls
        SceneManager.sceneLoaded -= OnSceneLoadedRestoreAnomalies;
    }

    public bool HasSaveGame()
    {
        return SaveSystem.SaveExists();
    }

    public void QuitGame()
    {
        // Save the game before returning to main menu
        SaveGame();
        
        // Load the main menu scene
        SceneManager.LoadScene("MainMenu");
    }

    // New method for actually quitting the application
    public void QuitApplication()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
} 