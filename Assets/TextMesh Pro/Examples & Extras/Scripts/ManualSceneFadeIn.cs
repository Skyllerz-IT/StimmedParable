using UnityEngine;

public class ManualSceneFadeIn : MonoBehaviour
{
    public float fadeInDuration = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PersistentFadeCanvas.Instance != null)
        {
            var fadePanel = PersistentFadeCanvas.Instance.fadePanel;
            if (fadePanel != null)
            {
                fadePanel.gameObject.SetActive(true);
                fadePanel.color = new Color(0, 0, 0, 1); // Ensure it's fully black
            }
            PersistentFadeCanvas.Instance.StartCoroutine(
                PersistentFadeCanvas.Instance.FadeIn(fadeInDuration)
            );
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
