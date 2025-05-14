using UnityEngine;

public class LookAtInteractable : MonoBehaviour
{
    public float maxDistance = 5f;
    public LayerMask interactableLayer;
    
    private GameObject currentHighlighted;
    private IHighlightable lastHighlightable;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, interactableLayer))
        {
            GameObject hitObject = hit.collider.gameObject;
            var highlightable = hitObject.GetComponent<IHighlightable>();

            if (highlightable != null)
            {
                if (highlightable != lastHighlightable)
                {
                    Unhighlight();
                    highlightable.Highlight();
                    lastHighlightable = highlightable;
                }
            }
        }
        else
        {
            Unhighlight();
        }
    }

    void Unhighlight()
    {
        if (lastHighlightable != null)
        {
            lastHighlightable.Unhighlight();
            lastHighlightable = null;
        }
    }
}