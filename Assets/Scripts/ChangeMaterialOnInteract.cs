using UnityEngine;

public class ChangeMaterialOnInteract : MonoBehaviour
{
    public GameObject targetObject;      // Assign the target GameObject in the Inspector
    public Material newMaterial;         // Assign the new Material in the Inspector

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Call this method to trigger the material change (e.g., from a button or another script)
    public void Interact()
    {
        if (targetObject != null && newMaterial != null)
        {
            Renderer renderer = targetObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = newMaterial;
            }
        }
    }
}
