using UnityEngine;
using TMPro;
using System.Collections;

public class ObjectiveTextAnimator : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float popScale = 1.2f;
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private AnimationCurve scaleCurve;
    
    private TextMeshProUGUI textComponent;
    private Vector3 originalScale;
    private Coroutine currentAnimation;
    
    private void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        originalScale = textComponent.transform.localScale;
        
        // Create a default animation curve if none is assigned
        if (scaleCurve.keys.Length == 0)
        {
            scaleCurve = new AnimationCurve(
                new Keyframe(0f, 1f),
                new Keyframe(0.5f, popScale),
                new Keyframe(1f, 1f)
            );
            scaleCurve.preWrapMode = WrapMode.PingPong;
            scaleCurve.postWrapMode = WrapMode.PingPong;
        }
    }
    
    public void StartPopping()
    {
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
        }
        currentAnimation = StartCoroutine(PopAnimation());
    }
    
    public void StopPopping()
    {
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
            textComponent.transform.localScale = originalScale;
        }
    }
    
    private IEnumerator PopAnimation()
    {
        float elapsed = 0f;
        
        while (true)
        {
            elapsed += Time.deltaTime;
            float normalizedTime = (elapsed % animationDuration) / animationDuration;
            float currentScale = scaleCurve.Evaluate(normalizedTime);
            
            textComponent.transform.localScale = originalScale * currentScale;
            
            yield return null;
        }
    }
} 