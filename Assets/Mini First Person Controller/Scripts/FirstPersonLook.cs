using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField]
    Transform character;

    public float sensitivity = 2;
    public float smoothing = 1.5f;

    Vector2 velocity;
    Vector2 frameVelocity;

    Vector2 touchStartPos;
    Vector2 touchDelta;

    void Reset()
    {
        character = GetComponentInParent<FirstPersonMovement>().transform;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
#if UNITY_EDITOR
        // Controlli da mouse per editor
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -90, 90);
#else
        // Controlli da touch su mobile
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Ignora se tocca nella zona sinistra (dove c'è il joystick)
            if (touch.position.x > Screen.width / 2)
            {
                if (touch.phase == TouchPhase.Moved)
                {
                    touchDelta = touch.deltaPosition;

                    // Calcola la velocità come il delta del touch
                    Vector2 rawFrameVelocity = Vector2.Scale(touchDelta, Vector2.one * sensitivity * 0.05f);
                    frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
                    velocity += frameVelocity;
                    velocity.y = Mathf.Clamp(velocity.y, -90, 90);
                }
            }
        }
#endif

        // Rotazione camera e player
        transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
        character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
    }
}