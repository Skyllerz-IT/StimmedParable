using UnityEngine;

public class RandomSoundTiming : MonoBehaviour
{
    public AudioClip soundClip;        // Il suono da riprodurre
    public float minDelay = 2f;        // Minimo tempo di attesa
    public float maxDelay = 5f;        // Massimo tempo di attesa

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        StartCoroutine(PlaySoundAtRandomTime());
    }

    System.Collections.IEnumerator PlaySoundAtRandomTime()
    {
        while (true)
        {
            float waitTime = Random.Range(minDelay, maxDelay);  // Tempo casuale
            yield return new WaitForSeconds(waitTime);          // Aspetta

            audioSource.PlayOneShot(soundClip);                 // Riproduce il suono
        }
    }
}