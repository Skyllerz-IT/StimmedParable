using UnityEngine;
using DefaultNamespace;

public class GrandfatherClock : MonoBehaviour, IInteractable
{
    public string InteractMessage => "Use Gear on Clock";

    public void Interact(InteractionController interactionController)
    {
        var pickupController = interactionController.GetComponent<PickupController>();
        if (pickupController != null && pickupController.HasPickup)
        {
            var gear = pickupController.HeldGear;
            if (gear != null)
            {
                Debug.Log("Gear used on the grandfather clock!");
                Object.Destroy(gear.gameObject);
                // Optionally clear the pickup reference if needed
            }
            else
            {
                Debug.Log("You need a gear to use the clock.");
            }
        }
        else
        {
            Debug.Log("You need a gear to use the clock.");
        }
    }
}