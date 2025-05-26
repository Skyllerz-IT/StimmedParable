using UnityEngine;

public class CameraLook : MonoBehaviour
{

    private float XMove;
    private float YMove;
    private float XRotation;
    [SerializeField] private Transform PlayerBody;
    public Vector2 LockAxis;
    public float Sensitivity = 10f;
    public float ArrowKeySensitivity = 50f;
    
    void Start()
    {
        
    }

    void Update()
    {
        XMove = LockAxis.x * Sensitivity * Time.deltaTime;
        YMove = LockAxis.y * Sensitivity * Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftArrow))
            XMove -= ArrowKeySensitivity * Time.deltaTime;
        if (Input.GetKey(KeyCode.RightArrow))
            XMove += ArrowKeySensitivity * Time.deltaTime;
        if (Input.GetKey(KeyCode.UpArrow))
            YMove += ArrowKeySensitivity * Time.deltaTime;
        if (Input.GetKey(KeyCode.DownArrow))
            YMove -= ArrowKeySensitivity * Time.deltaTime;

        XRotation -= YMove;
        XRotation = Mathf.Clamp(XRotation, -90f, 90f);
        
        transform.localRotation = Quaternion.Euler(XRotation, 0, 0);
        PlayerBody.Rotate(Vector3.up * XMove);
    }
}
