using UnityEngine;

namespace DefaultNamespace
{
    public interface IPickupable: IInteractable
    {
        public void Grab(PickupController pickupControl);
        public void Drop(PickupController pickupController);
        public void SetPositionInParent(Transform newParent);
        public void Use();
        
        

    }
}