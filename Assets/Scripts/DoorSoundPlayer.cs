using UnityEngine;

public class DoorSoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayOpenSound()
    {
        if (openSound != null)
            audioSource.PlayOneShot(openSound);
    }

    public void PlayCloseSound()
    {
        if (closeSound != null)
            audioSource.PlayOneShot(closeSound);
    }
}
