using UnityEngine;

public class Crouch : MonoBehaviour
{
    public KeyCode key = KeyCode.LeftControl;

    [Header("Slow Movement")]
    public FirstPersonMovement movement;
    public float movementSpeed = 2;

    [Header("Low Head")]
    public Transform headToLower;
    [HideInInspector]
    public float? defaultHeadYLocalPosition;
    public float crouchYHeadPosition = 1;

    public CapsuleCollider colliderToLower;
    [HideInInspector]
    public float? defaultColliderHeight;

    public bool IsCrouched { get; private set; }
    public event System.Action CrouchStart, CrouchEnd;

    void Reset()
    {
        movement = GetComponentInParent<FirstPersonMovement>();
        headToLower = movement.GetComponentInChildren<Camera>().transform;
        colliderToLower = movement.GetComponentInChildren<CapsuleCollider>();
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(key))
        {
            ToggleCrouch();
        }
    }

    // Chiamabile da UI Button o da tastiera
    public void ToggleCrouch()
    {
        if (!IsCrouched)
        {
            if (headToLower)
            {
                if (!defaultHeadYLocalPosition.HasValue)
                    defaultHeadYLocalPosition = headToLower.localPosition.y;

                headToLower.localPosition = new Vector3(
                    headToLower.localPosition.x,
                    crouchYHeadPosition,
                    headToLower.localPosition.z);
            }

            if (colliderToLower)
            {
                if (!defaultColliderHeight.HasValue)
                    defaultColliderHeight = colliderToLower.height;

                float loweringAmount = defaultHeadYLocalPosition.HasValue
                    ? defaultHeadYLocalPosition.Value - crouchYHeadPosition
                    : defaultColliderHeight.Value * .5f;

                colliderToLower.height = Mathf.Max(defaultColliderHeight.Value - loweringAmount, 0);
                colliderToLower.center = Vector3.up * colliderToLower.height * .5f;
            }

            IsCrouched = true;
            SetSpeedOverrideActive(true);
            CrouchStart?.Invoke();
        }
        else
        {
            if (headToLower)
            {
                headToLower.localPosition = new Vector3(
                    headToLower.localPosition.x,
                    defaultHeadYLocalPosition.Value,
                    headToLower.localPosition.z);
            }

            if (colliderToLower)
            {
                colliderToLower.height = defaultColliderHeight.Value;
                colliderToLower.center = Vector3.up * colliderToLower.height * .5f;
            }

            IsCrouched = false;
            SetSpeedOverrideActive(false);
            CrouchEnd?.Invoke();
        }
    }

    #region Speed override
    void SetSpeedOverrideActive(bool state)
    {
        if (!movement)
            return;

        if (state)
        {
            if (!movement.speedOverrides.Contains(SpeedOverride))
                movement.speedOverrides.Add(SpeedOverride);
        }
        else
        {
            if (movement.speedOverrides.Contains(SpeedOverride))
                movement.speedOverrides.Remove(SpeedOverride);
        }
    }

    float SpeedOverride() => movementSpeed;
    #endregion
}