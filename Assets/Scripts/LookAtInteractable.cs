using UnityEngine;
using UnityEngine.UI;

public class LookAtInteractable : MonoBehaviour
{
    public float maxDistance = 5f;
    public LayerMask interactableLayer;

    public Image crosshairImage;
    public Sprite defaultCrosshair;
    public Sprite interactableCrosshair;

    public float highlightScale = 15f;

    private Vector3 originalScale;
    private bool isLookingAtInteractable = false;

    void Start()
    {
        originalScale = crosshairImage.rectTransform.localScale;
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, interactableLayer))
        {
            if (!isLookingAtInteractable)
            {
                crosshairImage.sprite = interactableCrosshair;
                crosshairImage.rectTransform.localScale = originalScale * highlightScale;
                isLookingAtInteractable = true;
            }

            return;
        }

        if (isLookingAtInteractable)
        {
            crosshairImage.sprite = defaultCrosshair;
            crosshairImage.rectTransform.localScale = originalScale;
            isLookingAtInteractable = false;
        }
    }
}