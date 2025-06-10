using System.Collections;
using UnityEngine;

public class RandomBlink : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float minInterval = 0.1f;
    [SerializeField] private float maxInterval = 1.0f;
    
    private Light targetLight;

    private void Start()
    {
        targetLight = target.GetComponent<Light>();

        if (targetLight == null)
        {
            Debug.LogWarning("No component light on gameObject.");
        }

        StartCoroutine(BlinkRoutine());
    }

    private IEnumerator BlinkRoutine()
    {
        while (true)
        {
            if (targetLight != null)
            {
                targetLight.enabled = !targetLight.enabled;
            }

            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);
        }
    }
}