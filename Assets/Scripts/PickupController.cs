using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class PickupController: MonoBehaviour
    {
        [SerializeField] Transform pickupHolder;
        public Transform PickupHolder => pickupHolder;
        public Gear HeldGear => currentPickup as Gear;

        IPickupable currentPickup;
        public bool HasPickup => currentPickup != null;
        
        public void GrabPickup(IPickupable newPickup)
        {
            currentPickup = newPickup;
            currentPickup.SetPositionInParent(pickupHolder);
        }

        void Update()
        {
            CheckDropInput();

            CheckUsePickupInput();
        }

        void CheckDropInput()
        {
            if (Keyboard.current.qKey.wasPressedThisFrame && HasPickup)
            {
                currentPickup.Drop(this);
                currentPickup = null; 
            }
        }

        void CheckUsePickupInput()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame && HasPickup)
            {
                currentPickup.Use();
            } 
        }
    }
}