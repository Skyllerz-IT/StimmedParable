using UnityEngine;
using UnityEngine.EventSystems;

public class RunButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public FirstPersonMovement movement;

    public void OnPointerDown(PointerEventData eventData)
    {
        movement.IsMobileRunning = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        movement.IsMobileRunning = false;
    }
}