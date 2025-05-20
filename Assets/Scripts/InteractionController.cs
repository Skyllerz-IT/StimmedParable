using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class InteractionController : MonoBehaviour
    {
        [SerializeField] Camera playerCamera;
        [SerializeField] float interactionDistance;
        [SerializeField] ClickTouchField touchField;
        [SerializeField] LayerMask interactableLayer; // Optional layer mask to filter interactable objects
        
        IInteractable currentTargetedInteractable;

        private void Start()
        {
            // Subscribe to the touch field's tap event
            if (touchField != null)
            {
                touchField.OnTap += InteractWithTouch;
            }
        }

        private void OnDestroy()
        {
            // Unsubscribe from the event when this component is destroyed
            if (touchField != null)
            {
                touchField.OnTap -= InteractWithTouch;
            }
        }

        public void Update()
        {
            UpdateCurrentInteractable();
            CheckForInteractionInput();
        }

        void UpdateCurrentInteractable()
        {
            var ray = playerCamera.ViewportPointToRay(new Vector2(0.5f, 0.5f));
            
            if (Physics.Raycast(ray, out var hit, interactionDistance, interactableLayer.value == 0 ? -1 : interactableLayer))
            {
                // First check for IInteractable
                currentTargetedInteractable = hit.collider?.GetComponent<IInteractable>();

                // If not found, check for Interactable and wrap it
                if (currentTargetedInteractable == null)
                {
                    var legacyInteractable = hit.collider?.GetComponent<Interactable>();
                    if (legacyInteractable != null)
                    {
                        // Create a wrapper on-the-fly
                        currentTargetedInteractable = new LegacyInteractableWrapper(legacyInteractable);
                    }
                }
            }
            else
            {
                currentTargetedInteractable = null;
            }
        }

        void CheckForInteractionInput()
        {
            // Keep keyboard support
            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame && currentTargetedInteractable != null)
            {
                InteractWithCurrent();
            }
        }

        // Method called from touch events
        void InteractWithTouch()
        {
            if (currentTargetedInteractable != null)
            {
                InteractWithCurrent();
            }
        }

        // Common interaction method used by both input types
        void InteractWithCurrent()
        {
            currentTargetedInteractable.Interact(this);
        }
        
        // Wrapper class to adapt legacy Interactable to IInteractable interface
        private class LegacyInteractableWrapper : IInteractable
        {
            private readonly Interactable _legacyInteractable;

            public LegacyInteractableWrapper(Interactable legacyInteractable)
            {
                _legacyInteractable = legacyInteractable;
            }

            public string InteractMessage => "Interact";

            public void Interact(InteractionController interactionController)
            {
                _legacyInteractable.OnInteraction();
            }
        }
    }
}