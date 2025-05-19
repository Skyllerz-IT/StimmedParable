using UnityEngine;
using DefaultNamespace;

public class Door : MonoBehaviour, IInteractable
{
    public string InteractMessage => isOpen ? "Chiudi porta" : "Apri porta";

    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openSpeed = 2f;

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion targetRotation;

    private void Start()
    {
        closedRotation = transform.parent.rotation;
        targetRotation = closedRotation;
    }

    private void Update()
    {
        transform.parent.rotation = Quaternion.Lerp(transform.parent.rotation, targetRotation, Time.deltaTime * openSpeed);
    }

    public void Interact(InteractionController interactionController)
    {
        if (isOpen)
            CloseDoor();
        else
            OpenDoor();
    }

    private void OpenDoor()
    {
        isOpen = true;
        targetRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
    }

    private void CloseDoor()
    {
        isOpen = false;
        targetRotation = closedRotation;
    }
}