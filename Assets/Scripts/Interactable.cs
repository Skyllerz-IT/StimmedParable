using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public abstract string InteractMessage { get; }
    public abstract void Interact();
}
