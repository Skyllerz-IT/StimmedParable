using UnityEngine;
using TMPro;
using DefaultNamespace;
using System.Collections;

public class PhoneInteraction : MonoBehaviour, IInteractable
{
    [Header("UI")]
    public TextMeshProUGUI interactPrompt;
    public TextMeshProUGUI objectiveText;
    [SerializeField] private float fadeInDuration = 1f;

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

    private void Start()
    {
        if (interactPrompt != null)
            interactPrompt.gameObject.SetActive(false);

        if (objectiveText != null)
        {
            // Hide the text at start
            Color textColor = objectiveText.color;
            textColor.a = 0f;
            objectiveText.color = textColor;
        }

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
        
        // Start fading in the objective text
        if (objectiveText != null)
            StartCoroutine(FadeInText());
    }

    private IEnumerator FadeInText()
    {
        float elapsedTime = 0f;
        Color textColor = objectiveText.color;

        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);
            textColor.a = alpha;
            objectiveText.color = textColor;
            yield return null;
        }

        // Ensure we end up at full opacity
        textColor.a = 1f;
        objectiveText.color = textColor;
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