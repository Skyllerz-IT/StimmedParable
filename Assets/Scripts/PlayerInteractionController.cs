using System;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    public float maxDistance = 5f;
    public LayerMask InteractiveLayer;

    public ClickTouchField interactZone;

    private Interactable currentInteractable;

    private void Start()
    {
        interactZone.OnTap += Interact;
    }

    private void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, maxDistance, InteractiveLayer))
        {
            currentInteractable = hit.collider.GetComponent<Interactable>();
        }
        else currentInteractable = null;
    }

    public void Interact()
    {
        if (currentInteractable) currentInteractable.OnInteraction();
    }
}