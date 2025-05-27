using UnityEngine;

public class Crouch : MonoBehaviour
{
    private Vector3 crouchScale = new Vector3(0.8f, 0.35f, 0.8f);
    private Vector3 playerScale = new Vector3(0.8f, 0.8f, 0.8f);
    private bool isCrouching = false;

    private PlayerMove playerMovement;
    private float normalSpeed;
    public float crouchSpeedMultiplier = 0.5f; // Dimezza la velocità

    void Start()
    {
        playerMovement = GetComponent<PlayerMove>();
        normalSpeed = playerMovement.moveSpeed;
    }

    public void ToggleCrouch()
    {
        if (!isCrouching)
        {
            transform.localScale = crouchScale;
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
            playerMovement.moveSpeed = normalSpeed * crouchSpeedMultiplier;
            isCrouching = true;
        }
        else
        {
            transform.localScale = playerScale;
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            playerMovement.moveSpeed = normalSpeed;
            isCrouching = false;
        }
    }
}