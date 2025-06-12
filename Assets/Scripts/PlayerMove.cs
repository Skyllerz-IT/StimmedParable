using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public FixedJoystick joystick;
    public float moveSpeed = 5f;
    public float gravity = 9.81f;
    public float groundCheckDistance = 0.1f;
    public Transform groundCheck;
    public LayerMask groundMask;

    public float acceleration = 10f; // How quickly the player accelerates
    public float inertia = 5f; // How quickly the player slows down when input is released

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private Vector3 currentMoveVelocity = Vector3.zero; // For smooth movement

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Check if grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Movement input (joystick or keyboard)
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        if (Mathf.Approximately(horizontal, 0f) && Mathf.Approximately(vertical, 0f))
        {
            // No joystick input, fallback to keyboard (WASD only)
            horizontal = Input.GetKey(KeyCode.D) ? 1f : Input.GetKey(KeyCode.A) ? -1f : 0f;
            vertical = Input.GetKey(KeyCode.W) ? 1f : Input.GetKey(KeyCode.S) ? -1f : 0f;
        }

        Vector3 inputDirection = (transform.right * horizontal + transform.forward * vertical).normalized;
        Vector3 targetVelocity = inputDirection * moveSpeed;

        // Apply acceleration and inertia
        float lerpSpeed = (inputDirection.magnitude > 0.01f) ? acceleration : inertia;
        currentMoveVelocity = Vector3.Lerp(currentMoveVelocity, targetVelocity, lerpSpeed * Time.deltaTime);

        controller.Move(currentMoveVelocity * Time.deltaTime);

        // Gravity
        velocity.y -= gravity * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, -10f);
        controller.Move(velocity * Time.deltaTime);
    }
}