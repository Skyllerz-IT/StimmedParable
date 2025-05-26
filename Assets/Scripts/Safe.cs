using UnityEngine;
using DefaultNamespace;

public class Safe : MonoBehaviour, IInteractable
{
    public string InteractMessage => isCanvasActive ? "Chiudi canvas" : "Apri canvas";
    
    [SerializeField] private GameObject canvasToActivate;

    private bool isCanvasActive = false;

    public void Interact(InteractionController interactionController)
    {
        isCanvasActive = !isCanvasActive;
        canvasToActivate.SetActive(isCanvasActive);
    }
}