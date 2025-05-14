using UnityEngine;

public class HighlightObject : MonoBehaviour, IHighlightable
{
    private Material originalMaterial;
    public Material highlightMaterial;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        originalMaterial = rend.material;
    }

    public void Highlight()
    {
        rend.material = highlightMaterial;
    }

    public void Unhighlight()
    {
        rend.material = originalMaterial;
    }
}