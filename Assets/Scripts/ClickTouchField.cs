using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ClickTouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event Action OnTap;

    private Vector2 touchStart;
    private float maxTapMovement = 20f;
    private float maxTapTime = 0.3f;
    private float touchStartTime;
    private bool isPointerDown;

    public void OnPointerDown(PointerEventData eventData)
    {
        touchStart = eventData.position;
        touchStartTime = Time.time;
        isPointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isPointerDown) return;
        isPointerDown = false;

        float duration = Time.time - touchStartTime;
        float movement = Vector2.Distance(touchStart, eventData.position);

        if (duration <= maxTapTime && movement <= maxTapMovement)
        {
            OnTap?.Invoke();
        }
    }
}
