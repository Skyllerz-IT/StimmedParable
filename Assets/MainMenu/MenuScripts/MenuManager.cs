using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public Image fadePanel; // Assign the FadePanel (black image) in Inspector
    public float fadeDuration = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Start with fadePanel fully opaque, then fade out to show menu
        if (fadePanel != null)
        {
            fadePanel.gameObject.SetActive(true);
            fadePanel.color = new Color(0, 0, 0, 1);
            StartCoroutine(FadeOut());
        }
    }

    public void OnNewGamePressed()
    {
        StartCoroutine(FadeAndLoadScene("AbandonedHouse"));
    }

    IEnumerator FadeAndLoadScene(string sceneName)
    {
        if (fadePanel != null)
        {
            fadePanel.gameObject.SetActive(true);
            // Fade in to black
            float t = 0;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                fadePanel.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, t / fadeDuration));
                yield return null;
            }
            fadePanel.color = new Color(0, 0, 0, 1);
        }
        // Load the game scene
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator FadeOut()
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadePanel.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, t / fadeDuration));
            yield return null;
        }
        fadePanel.color = new Color(0, 0, 0, 0);
        fadePanel.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
