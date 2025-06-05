using UnityEngine;
using UnityEngine.UI;

public class CameraLook : MonoBehaviour
{

    private float XMove;
    private float YMove;
    private float XRotation;
    [SerializeField] private Transform PlayerBody;
    public Vector2 LockAxis;
    public float Sensitivity = 10f;
    public float ArrowKeySensitivity = 50f;
    public Slider sensitivitySlider;
    public Slider brightnessSlider;
    public Slider audioSlider;
    
    void Start()
    {
        if (SettingsManager.Instance != null)
            Sensitivity = SettingsManager.Instance.Sensitivity;
        else if (PlayerPrefs.HasKey("Sensitivity"))
            Sensitivity = PlayerPrefs.GetFloat("Sensitivity");
        else
            Sensitivity = 5f; // default
    }

    void Update()
    {
        // Always get the latest value from SettingsManager if available
        if (SettingsManager.Instance != null)
            Sensitivity = SettingsManager.Instance.Sensitivity;

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
