using UnityEngine;

public class CreditsScroll : MonoBehaviour
{
    public enum ScrollDirection
    {
        BottomToTop,
        TopToBottom
    }

    public float scrollSpeed = 50f; // Units per second
    public ScrollDirection scrollDirection = ScrollDirection.BottomToTop;
    public bool loop = false;
    public float stopAtY = 0f; // Inspector setting for stop position

    private RectTransform rectTransform;
    private RectTransform parentRect;
    private Vector2 startPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRect = rectTransform.parent.GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();
        if (parentRect == null)
            parentRect = rectTransform.parent.GetComponent<RectTransform>();
        // Always use the current position as the start position
        startPosition = rectTransform.anchoredPosition;
        rectTransform.anchoredPosition = startPosition;
        enabled = true;
    }

    void OnDisable()
    {
        // Reset position so it's ready for next time
        if (rectTransform != null)
            rectTransform.anchoredPosition = startPosition;
        enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = (scrollDirection == ScrollDirection.BottomToTop) ? Vector2.up : Vector2.down;
        rectTransform.anchoredPosition += dir * scrollSpeed * Time.deltaTime;

        float y = rectTransform.anchoredPosition.y;

        if (scrollDirection == ScrollDirection.BottomToTop)
        {
            if (y >= stopAtY)
            {
                if (loop)
                    rectTransform.anchoredPosition = startPosition;
                else
                    enabled = false;
            }
        }
        else // TopToBottom
        {
            if (y <= stopAtY)
            {
                if (loop)
                    rectTransform.anchoredPosition = startPosition;
                else
                    enabled = false;
            }
        }
    }
}
