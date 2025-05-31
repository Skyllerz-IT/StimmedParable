using UnityEngine;
using DefaultNamespace;

public class Gear : MonoBehaviour, IPickupable
{
    public string InteractMessage => "Pick up Gear";
    private bool isPickedUp = false;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

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
        pickupController.GrabPickup(this); // <-- Add this line
        if (pickupController.PickupHolder != null)
        {
            transform.SetParent(pickupController.PickupHolder);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
        else
        {
            transform.SetParent(pickupController.transform);
            transform.localPosition = Vector3.zero;
        }
        var col = GetComponent<Collider>();
        if (col) col.enabled = true;
        if (rb) rb.isKinematic = true;
        Debug.Log("Gear picked up!");
    }

    public void Drop(PickupController pickupController)
    {
        isPickedUp = false;
        transform.SetParent(null);
        var col = GetComponent<Collider>();
        if (col) col.enabled = true;
        if (rb) rb.isKinematic = false;
        Debug.Log("Gear dropped!");
    }

    public void SetPositionInParent(Transform newParent)
    {
        transform.SetParent(newParent);
        transform.localPosition = Vector3.zero;
    }

    public void Use()
    {
        Debug.Log("Gear used on the grandfather clock!");
        Destroy(gameObject);
    }
}