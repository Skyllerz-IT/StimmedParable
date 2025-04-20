using UnityEngine;

namespace DefaultNamespace
{
    public class PhysicsPickup: MonoBehaviour, IPickupable  
    {
        [SerializeField] Rigidbody pickupRigidbody; 
        
        [SerializeField] Collider pickupCollider;
        
        [SerializeField] Vector3 pickupPositionOffset;
        
        public virtual string InteractMessage => "Press E to Pickup";
        public void Interact(InteractionController interactionController)
        {
            var pickupController = interactionController.GetComponent<PickupController>();
            
            Grab(pickupController);
        }

        public virtual void Grab(PickupController pickupController)
        {
            if (pickupController ==null || pickupController.HasPickup)
            {
                return;
            }
            
            pickupController.GrabPickup(this); 
            
            SetPhysicsValues(true);

        } 

        public virtual void Drop(PickupController pickupController)
        {
            transform.parent = null;
            
            SetPhysicsValues(false);
            
        }

        public void SetPositionInParent(Transform newParent)
        {
           transform.parent = newParent;
           transform.localPosition = pickupPositionOffset;
           transform.localRotation = Quaternion.identity;
        }

        public virtual void Use()
        {
           Debug.Log("Pickup Used");
        }
        
        void SetPhysicsValues(bool wasPickedUp)
        {
            pickupRigidbody.isKinematic = wasPickedUp;
            pickupCollider.enabled = !wasPickedUp;
        }
    }
    
    
}