using UnityEngine;
using TMPro;
using DefaultNamespace;
using System.Collections;

public class PhoneInteraction : MonoBehaviour, IInteractable
{
    [Header("UI")]
    public TextMeshProUGUI interactPrompt;
    [Header("Audio")]
    public AudioClip ringClip;
    public AudioClip pickupClip;
    public AudioClip voiceMessageClip;
    public float ringVolume = 1.0f;

    [Header("Ring Settings")]
    [Tooltip("Delay before the phone starts ringing (seconds)")]
    public float ringDelay = 1f;

    private bool hasInteracted = false;
    private AudioSource audioSource;

    public string InteractMessage => hasInteracted ? "" : "Answer Phone";

    void Start()
    {
        if (interactPrompt != null)
            interactPrompt.gameObject.SetActive(false);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.volume = ringVolume;

        // Start ringing after delay
        float delay = ringDelay > 0f ? ringDelay : 1f;
        Invoke(nameof(StartRinging), delay);
    }

    private void StartRinging()
    {
        audioSource.clip = ringClip;
        audioSource.loop = true;
        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    public void Interact(InteractionController interactionController)
    {
        if (hasInteracted) return;

        Debug.Log("Phone answered!");
        if (interactPrompt != null)
            interactPrompt.gameObject.SetActive(false);
        hasInteracted = true;

        audioSource.loop = false;
        audioSource.Stop();
        audioSource.clip = pickupClip;
        audioSource.Play();

        if (voiceMessageClip != null)
            StartCoroutine(PlayVoiceMessageAfterPickup());
    }

    private IEnumerator PlayVoiceMessageAfterPickup()
    {
        yield return new WaitWhile(() => audioSource.isPlaying);
        audioSource.clip = voiceMessageClip;
        audioSource.loop = false;
        audioSource.Play();
    }
}