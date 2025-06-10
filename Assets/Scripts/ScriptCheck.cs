using UnityEngine;

public class ScriptCheck : MonoBehaviour
{

    [SerializeField] private Component target;
    [SerializeField] private GameObject activeOBJ;
    
    private Light targetLight;
    private Coroutine blinkCoroutine;
    private bool isBlinking = true;
    
    private void Start()
    {
        targetLight = activeOBJ.GetComponent<Light>();

        if (targetLight == null)
        {
            Debug.LogWarning("No component light on gameObject.");
        }
    }

    void Update()
    {
        if (target != null)
        {
            activeOBJ.SetActive(true);
        }
    }
    
    public void StopBlinkingAndTurnOn()
    {
        if (isBlinking)
        {
            isBlinking = false;
            StopCoroutine(blinkCoroutine);
            targetLight.enabled = true;
        }
    }
}
