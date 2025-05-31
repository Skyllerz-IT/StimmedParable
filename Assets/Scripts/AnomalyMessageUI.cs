using UnityEngine;
using TMPro;
using System.Collections;

public class AnomalyMessageUI : MonoBehaviour
{
    public TextMeshProUGUI messageText; // For "You found an anomaly"
    public TextMeshProUGUI alreadyFoundText; // For "You already found this anomaly"
    public float fadeInDuration = 1f;
    public float fadeOutDuration = 1f;
    public float messageDuration = 2f;

    private Coroutine fadeCoroutine;
    private Coroutine alreadyFoundCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (messageText != null)
        {
            var color = messageText.color;
            messageText.color = new Color(color.r, color.g, color.b, 0f);
        }
        if (alreadyFoundText != null)
        {
            var color = alreadyFoundText.color;
            alreadyFoundText.color = new Color(color.r, color.g, color.b, 0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowMessage(string message)
    {
        HideAllMessagesInstantly();
        message = message.Replace("\n", " ");
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeMessageRoutine(messageText, message));
    }

    public void ShowAlreadyFoundMessage()
    {
        HideAllMessagesInstantly();
        if (alreadyFoundCoroutine != null)
            StopCoroutine(alreadyFoundCoroutine);

        alreadyFoundCoroutine = StartCoroutine(FadeMessageRoutine(alreadyFoundText, alreadyFoundText.text));
    }

    private IEnumerator FadeMessageRoutine(TextMeshProUGUI textObj, string message)
    {
        textObj.text = message;

        // Fade in
        yield return StartCoroutine(FadeTextAlpha(textObj, 0f, 1f, fadeInDuration));

        // Wait for message duration
        yield return new WaitForSeconds(messageDuration);

        // Fade out
        yield return StartCoroutine(FadeTextAlpha(textObj, 1f, 0f, fadeOutDuration));
    }

    private IEnumerator FadeTextAlpha(TextMeshProUGUI textObj, float from, float to, float duration)
    {
        float elapsed = 0f;
        Color color = textObj.color;
        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(from, to, elapsed / duration);
            textObj.color = new Color(color.r, color.g, color.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }
        textObj.color = new Color(color.r, color.g, color.b, to);
    }

    public void HideAllMessagesInstantly()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
        if (alreadyFoundCoroutine != null)
            StopCoroutine(alreadyFoundCoroutine);

        if (messageText != null)
        {
            var color = messageText.color;
            messageText.color = new Color(color.r, color.g, color.b, 0f);
        }
        if (alreadyFoundText != null)
        {
            var color = alreadyFoundText.color;
            alreadyFoundText.color = new Color(color.r, color.g, color.b, 0f);
        }
    }
}
