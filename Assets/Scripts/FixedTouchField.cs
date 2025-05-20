using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FixedTouchField : MonoBehaviour
{
    public Vector2 TouchDist;
    private Vector2 pointerOld;
    private int pointerId = -1;
    private bool pressed = false;

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (!pressed && touch.phase == TouchPhase.Began && IsTouchInside(touch.position))
            {
                pointerId = touch.fingerId;
                pointerOld = touch.position;
                pressed = true;
            }

            if (pressed && touch.fingerId == pointerId)
            {
                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    TouchDist = touch.position - pointerOld;
                    pointerOld = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    pressed = false;
                    TouchDist = Vector2.zero;
                }
            }
        }

        if (!pressed)
            TouchDist = Vector2.zero;
    }

    private bool IsTouchInside(Vector2 screenPos)
    {
        RectTransform rt = GetComponent<RectTransform>();
        if (rt == null) return true; // se non è UI, accetta tutto

        Vector2 localPoint;
        return RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rt, screenPos, null, out localPoint) && rt.rect.Contains(localPoint);
    }
}