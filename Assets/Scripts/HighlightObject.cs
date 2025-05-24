using UnityEngine;

public class HighlightObject : MonoBehaviour
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
    }

    public void Unhighlight()
    {
    }
}