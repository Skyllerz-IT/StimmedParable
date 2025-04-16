using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float runSpeed = 9f;
    private Vector2 moveInput;
    private bool isRunning;

    [Header("Look")]
    public Transform cameraTransform;
    public float lookSensitivity = 0.1f;
    public float maxLookX = 90f;
    public float minLookX = -90f;
    private Vector2 lookInput;
    private float currentLookX;

    private Rigidbody rb;
    private PlayerInput playerInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

        // Assicura che il cursore sia nascosto su editor
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        playerInput.actions["Move"].performed += OnMove;
        playerInput.actions["Move"].canceled += ctx => moveInput = Vector2.zero;

        playerInput.actions["Look"].performed += OnLook;
        playerInput.actions["Look"].canceled += ctx => lookInput = Vector2.zero;

        playerInput.actions["Run"].performed += ctx => isRunning = true;
        playerInput.actions["Run"].canceled += ctx => isRunning = false;
    }

    private void OnDisable()
    {
        playerInput.actions["Move"].performed -= OnMove;
        playerInput.actions["Look"].performed -= OnLook;
        playerInput.actions["Run"].performed -= ctx => isRunning = true;
        playerInput.actions["Run"].canceled -= ctx => isRunning = false;
    }

    private void FixedUpdate()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        float speed = isRunning ? runSpeed : moveSpeed;

        rb.linearVelocity = new Vector3(move.x * speed, rb.linearVelocity.y, move.z * speed);
    }

    private void LateUpdate()
    {
        // Rotazione camera
        currentLookX -= lookInput.y * lookSensitivity;
        currentLookX = Mathf.Clamp(currentLookX, minLookX, maxLookX);

        cameraTransform.localRotation = Quaternion.Euler(currentLookX, 0f, 0f);
        transform.Rotate(Vector3.up * lookInput.x * lookSensitivity);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }
}