using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public FixedJoystick joystick;
    public float moveSpeed = 5f;
    public float gravity = -4f;
    public float groundCheckDistance = 0.1f;
    public Transform groundCheck;
    public LayerMask groundMask;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Controllo se è a terra
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Mantiene il player incollato al terreno
        }

        // Movimento orizzontale
        Vector3 move = transform.right * joystick.Horizontal + transform.forward * joystick.Vertical;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Applica la gravità manualmente
        velocity.y += gravity * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, -10f);
        controller.Move(velocity * Time.deltaTime);
    }
}
