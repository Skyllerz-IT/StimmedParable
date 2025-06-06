using UnityEngine;
using DefaultNamespace;

public class Openable : Interactable
{
    public enum Axis { X, Y, Z }

    public override string InteractMessage => isOpen ? closeMessage : openMessage;

    [Header("Open/Close Settings")]
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openSpeed = 2f;
    [SerializeField] private string openMessage = "Open";
    [SerializeField] private string closeMessage = "Close";
    [Header("Open Axis")]
    [SerializeField] private Axis openAxis = Axis.Y;
    [Header("Pivot Offset Axis")]
    [SerializeField] private Axis pivotAxis = Axis.Y;
    [SerializeField] private Vector3 pivotOffset = Vector3.zero;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion targetRotation;
    private Vector3 closedPosition;
    private Vector3 targetPosition;

    private void Start()
    {
        closedRotation = transform.localRotation;
        targetRotation = closedRotation;
        closedPosition = transform.localPosition;
        targetPosition = closedPosition;
    }

    private void Update()
    {
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * openSpeed);
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * openSpeed);
    }

    public override void Interact()
    {
        Debug.Log("Interact called on " + gameObject.name);
        if (isOpen)
            Close();
        else
            Open();
    }

    private void Open()
    {
        Debug.Log("Open called on " + gameObject.name);
        isOpen = true;
        targetRotation = closedRotation * Quaternion.Euler(GetOpenAngleVector(openAngle));
        targetPosition = closedPosition + GetPivotOffsetVector(pivotOffset);
        if (audioSource && openSound)
            audioSource.PlayOneShot(openSound);
    }

    private void Close()
    {
        Debug.Log("Close called on " + gameObject.name);
        isOpen = false;
        targetRotation = closedRotation;
        targetPosition = closedPosition;
        if (audioSource && closeSound)
            audioSource.PlayOneShot(closeSound);
    }

    private Vector3 GetOpenAngleVector(float angle)
    {
        switch (openAxis)
        {
            case Axis.X: return new Vector3(angle, 0, 0);
            case Axis.Y: return new Vector3(0, angle, 0);
            case Axis.Z: return new Vector3(0, 0, angle);
            default: return Vector3.zero;
        }
    }

    private Vector3 GetPivotOffsetVector(Vector3 offset)
    {
        switch (pivotAxis)
        {
            case Axis.X: return new Vector3(offset.x, 0, 0);
            case Axis.Y: return new Vector3(0, offset.y, 0);
            case Axis.Z: return new Vector3(0, 0, offset.z);
            default: return Vector3.zero;
        }
    }
}