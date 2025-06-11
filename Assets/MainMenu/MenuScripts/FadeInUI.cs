using UnityEngine;
using UnityEngine.UI;

public class FadeInImage : MonoBehaviour
{
    public float fadeDuration = 1.5f;
    public AnimationCurve fadeCurve = AnimationCurve.Linear(0, 0, 1, 1); // Default: linear
    private Image image;
    private float timer = 0f;
    private Color originalColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        image = GetComponent<Image>();
        originalColor = image.color;
        image.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f); // Start transparent
    }

    void OnEnable()
    {
        timer = 0f;
        if (image == null)
            image = GetComponent<Image>();
        image.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (image.color.a < originalColor.a)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / fadeDuration);
            float curveValue = fadeCurve.Evaluate(t);
            float alpha = curveValue * originalColor.a;
            image.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
        }
    }
}
