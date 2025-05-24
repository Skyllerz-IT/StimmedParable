using UnityEngine;

public class RandomSoundTiming : MonoBehaviour
{
    public AudioClip soundClip;        // Il suono da riprodurre
    public float minDelay = 30f;        // Minimo tempo di attesa
    public float maxDelay = 90f;        // Massimo tempo di attesa

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        
        audioSource.volume = 0.2f;

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