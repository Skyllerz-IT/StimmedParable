using UnityEngine;

namespace DefaultNamespace
{
    public interface IInteractable
    {
        string InteractMessage { get; }
        void Interact(InteractionController interactionController);
    }
}
