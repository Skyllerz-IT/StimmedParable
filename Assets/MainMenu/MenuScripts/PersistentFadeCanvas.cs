using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PersistentFadeCanvas : MonoBehaviour
{
    public static PersistentFadeCanvas Instance { get; private set; }
    public Image fadePanel;
    public float defaultFadeDuration = 1f;

    private float _fadeInDuration = 1f;

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

        // Ensure panel is fully opaque at start
        if (fadePanel != null)
        {
            fadePanel.color = new Color(0, 0, 0, 1);
            fadePanel.gameObject.SetActive(true);
        }
    }

    void Start()
    {
        // Fade in at the start
        StartCoroutine(FadeIn(defaultFadeDuration));
    }

    public void FadeToScene(string sceneName, float fadeOutDuration = -1f, float fadeInDuration = -1f)
    {
        if (fadeOutDuration < 0) fadeOutDuration = defaultFadeDuration;
        if (fadeInDuration < 0) fadeInDuration = defaultFadeDuration;
        StartCoroutine(FadeOutInAndLoad(sceneName, fadeOutDuration, fadeInDuration));
    }

    private IEnumerator FadeOutInAndLoad(string sceneName, float fadeOutDuration, float fadeInDuration)
    {
        yield return StartCoroutine(FadeOut(fadeOutDuration));
        _fadeInDuration = fadeInDuration;
        Debug.Log("Subscribing to sceneLoaded");
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneName);
        Debug.Log("Scene load called");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded called for: " + scene.name);
        SceneManager.sceneLoaded -= OnSceneLoaded;
        StartCoroutine(FadeIn(_fadeInDuration));
    }

    public IEnumerator FadeIn(float duration)
    {
        Debug.Log("FadeIn started");
        fadePanel.gameObject.SetActive(true);
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            fadePanel.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, t / duration));
            yield return null;
        }
        fadePanel.color = new Color(0, 0, 0, 0);
        fadePanel.gameObject.SetActive(false);
        Debug.Log("FadeIn finished");
    }

    public IEnumerator FadeOut(float duration)
    {
        Debug.Log("FadeOut started");
        fadePanel.gameObject.SetActive(true);
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            fadePanel.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, t / duration));
            yield return null;
        }
        fadePanel.color = new Color(0, 0, 0, 1);
        Debug.Log("FadeOut finished");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
