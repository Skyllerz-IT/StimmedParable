using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;
    public float acceleration = 20f; // per inerzia

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public KeyCode runningKey = KeyCode.LeftShift;

    [HideInInspector] public bool IsMobileRunning = false;

    Rigidbody rigidbody;

    [Header("Joystick")]
    public FloatingJoystick moveJoystick;

    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

    Vector3 currentVelocity;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        IsRunning = canRun && (Input.GetKey(runningKey) || IsMobileRunning);

        float targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        float horizontal = 0f;
        float vertical = 0f;

#if UNITY_EDITOR
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
#else
        if (moveJoystick != null)
        {
            horizontal = moveJoystick.Horizontal;
            vertical = moveJoystick.Vertical;
        }
#endif

        Vector3 targetVelocity = transform.rotation * new Vector3(horizontal, 0, vertical) * targetMovingSpeed;

        // Inerzia: interpoliamo verso il targetVelocity
        currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);

        // Applichiamo la velocity mantenendo quella verticale attuale (gravità)
        rigidbody.linearVelocity = new Vector3(currentVelocity.x, rigidbody.linearVelocity.y, currentVelocity.z);
    }

    public void ToggleMobileRun()
    {
        IsMobileRunning = !IsMobileRunning;
    }
}