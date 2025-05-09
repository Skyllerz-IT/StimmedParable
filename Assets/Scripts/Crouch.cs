using UnityEngine;


public class Crouch : MonoBehaviour
{
    private Vector3 crouchScale = new Vector3(1f, 0.5f, 1f);
    private Vector3 playerScale = new Vector3(1f, 1f, 1f);
    private bool isCrouching = false;

    public void ToggleCrouch()
    {
        if (!isCrouching)
        {
            // Abbassa il personaggio
            transform.localScale = crouchScale;
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
            isCrouching = true;
            print(transform.localScale);
        }
        else
        {
            // Rialza il personaggio
            transform.localScale = playerScale;
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            isCrouching = false;
        }
    }
}