using UnityEngine;
using DefaultNamespace;

public class SlidingWindow : MonoBehaviour, IInteractable
{
    public string InteractMessage => isOpen ? "Chiudi porta" : "Apri porta";

    [SerializeField] private float slideDistance = 0.8f; // distanza dello slide in negativo su X
    [SerializeField] private float slideSpeed = 2f;

    private bool isOpen = false;
    private Vector3 closedPosition;
    private Vector3 targetPosition;

    private void Start()
    {
        closedPosition = transform.localPosition;
        targetPosition = closedPosition;
    }

    private void Update()
    {
        // Movimento fluido verso la posizione target
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * slideSpeed);
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
        targetPosition = closedPosition + Vector3.left * slideDistance;
    }

    private void CloseDoor()
    {
        isOpen = false;
        targetPosition = closedPosition;
    }
}