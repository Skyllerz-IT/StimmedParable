using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TMPFadeOnScene : MonoBehaviour
{
    public string targetSceneName = "AbandonedHouse";
    public float fadeInDuration = 1f;
    public float visibleDuration = 2f;
    public float fadeOutDuration = 1f;

    private TextMeshProUGUI tmp;
    private CanvasGroup canvasGroup;
    private bool started = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == targetSceneName && !started)
        {
            started = true;
            StartCoroutine(FadeRoutine());
        }
    }

    System.Collections.IEnumerator FadeRoutine()
    {
        // Fade in
        float t = 0f;
        while (t < fadeInDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(t / fadeInDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;

        // Wait
        yield return new WaitForSeconds(visibleDuration);

        // Fade out
        t = 0f;
        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(1f - (t / fadeOutDuration));
            yield return null;
        }
        canvasGroup.alpha = 0f;
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
