using UnityEngine;
using UnityEngine.UI;

public class LookAtInteractable : MonoBehaviour
{
    public float maxDistance = 5f;
    public LayerMask interactableLayer;

    public Image crosshairImage;
    public Sprite defaultCrosshair;
    public Sprite interactableCrosshair;
    public Sprite anomalyCrosshair;

    public float highlightScale = 15f;

    private Vector3 originalScale;
    private bool isLookingAtInteractable = false;

    public ClickTouchField touchField;

    void Start()
    {
        originalScale = crosshairImage.rectTransform.localScale;
        if (touchField != null)
            touchField.OnTap += OnTouchTap;
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, interactableLayer))
        {
            int anomalyLayer = LayerMask.NameToLayer("Anomaly");
            if (hit.collider.gameObject.layer == anomalyLayer && anomalyCrosshair != null)
            {
                crosshairImage.sprite = anomalyCrosshair;
            }
            else
            {
                crosshairImage.sprite = interactableCrosshair;
            }
                crosshairImage.rectTransform.localScale = originalScale * highlightScale;
                isLookingAtInteractable = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                var anomaly = hit.collider.GetComponent<InteractiveAnomaly>();
                if (anomaly != null)
                {
                    anomaly.Interact();
                }
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

    private void OnTouchTap()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, interactableLayer))
        {
            var anomaly = hit.collider.GetComponent<InteractiveAnomaly>();
            if (anomaly != null)
            {
                anomaly.Interact();
            }
        }
    }
}