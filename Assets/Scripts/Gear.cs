using UnityEngine;
using DefaultNamespace;

public class Gear : MonoBehaviour, IPickupable
{
    public string InteractMessage => "Pick up Gear";

    private bool isPickedUp = false;

    public void Interact(InteractionController interactionController)
    {
        var pickupController = interactionController.GetComponent<PickupController>();
        if (pickupController != null && !isPickedUp)
        {
            Grab(pickupController);
        }
    }

    public void Grab(PickupController pickupController)
    {
        isPickedUp = true;
        transform.SetParent(pickupController.transform);
        transform.localPosition = Vector3.zero;
        var col = GetComponent<Collider>();
        if (col) col.enabled = false;
        // Optionally: Add to inventory or UI
        Debug.Log("Gear picked up!");
    }

    public void Drop(PickupController pickupController)
    {
        isPickedUp = false;
        transform.SetParent(null);
        var col = GetComponent<Collider>();
        if (col) col.enabled = true;
        Debug.Log("Gear dropped!");
    }

    public void SetPositionInParent(Transform newParent)
    {
        transform.SetParent(newParent);
        transform.localPosition = Vector3.zero;
    }

    public void Use()
    {
        // Logic to use the gear on the grandfather clock
        Debug.Log("Gear used on the grandfather clock!");
        // Example: Notify the clock, destroy or disable the gear, etc.
        Destroy(gameObject);
    }
}