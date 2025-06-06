using System.Collections;
using UnityEngine;

public class RandomBlink : MonoBehaviour
{
    [SerializeField] private GameObject target; // Oggetto da attivare/disattivare
    [SerializeField] private float minInterval = 0.1f;
    [SerializeField] private float maxInterval = 1.0f;

    private void Start()
    {
        StartCoroutine(BlinkRoutine());
    }

    private IEnumerator BlinkRoutine()
    {
        while (true)
        {
            target.SetActive(!target.activeSelf);
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);
        }
    }
}