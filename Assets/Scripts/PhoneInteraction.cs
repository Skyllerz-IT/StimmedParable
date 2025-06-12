using UnityEngine;
using TMPro;
using DefaultNamespace;
using System.Collections;

public class PhoneInteraction : MonoBehaviour, IInteractable
{
    [Header("UI")]
    public TextMeshProUGUI interactPrompt;
    public TextMeshProUGUI objectiveText;
    public TextMeshProUGUI noah1stDialogue;
    public TextMeshProUGUI noah2ndDialogue;
    public TextMeshProUGUI noah3rdDialogue;
    public TextMeshProUGUI noah4thDialogue;
    public TextMeshProUGUI noah5thDialogue;

    [Header("Dialogue Settings")]
    [SerializeField] private float dialogueDelay = 1f;  // Delay before first dialogue
    [SerializeField] private float dialogueDisplayTime = 3f;  // How long each dialogue stays visible
    [SerializeField] private float dialogueFadeTime = 0.5f;  // Fade in/out time
    [SerializeField] private float afterCallDelay = 1f;  // Delay before post-call dialogues

    [Header("UI Transitions")]
    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private float fadeOutDuration = 0.5f;

    [Header("Audio")]
    public AudioClip ringClip;
    public AudioClip pickupClip;
    public AudioClip voiceMessageClip;
    public AudioClip endCallBeepClip;
    [SerializeField] private int numberOfBeeps = 5;
    [SerializeField] private float beepInterval = 0.5f;
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
            Color textColor = objectiveText.color;
            textColor.a = 0f;
            objectiveText.color = textColor;
        }

        // Hide first three dialogues
        HideDialogueText(noah1stDialogue);
        HideDialogueText(noah2ndDialogue);
        HideDialogueText(noah3rdDialogue);

        // Completely deactivate 4th and 5th dialogues
        if (noah4thDialogue != null)
            noah4thDialogue.gameObject.SetActive(false);
        if (noah5thDialogue != null)
            noah5thDialogue.gameObject.SetActive(false);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.volume = ringVolume;

        float delay = ringDelay > 0f ? ringDelay : 1f;
        Invoke(nameof(StartRinging), delay);
    }

    private void HideDialogueText(TextMeshProUGUI text)
    {
        if (text != null)
        {
            text.alpha = 0f;
        }
    }

    private IEnumerator ShowDialogueSequence()
    {
        yield return new WaitForSeconds(dialogueDelay);

        // First dialogue
        if (noah1stDialogue != null)
        {
            yield return StartCoroutine(FadeDialogue(noah1stDialogue, true));
            yield return new WaitForSeconds(dialogueDisplayTime);
            yield return StartCoroutine(FadeDialogue(noah1stDialogue, false));
        }

        // Second dialogue
        if (noah2ndDialogue != null)
        {
            yield return StartCoroutine(FadeDialogue(noah2ndDialogue, true));
            yield return new WaitForSeconds(dialogueDisplayTime);
            yield return StartCoroutine(FadeDialogue(noah2ndDialogue, false));
        }

        // Third dialogue
        if (noah3rdDialogue != null)
        {
            yield return StartCoroutine(FadeDialogue(noah3rdDialogue, true));
            yield return new WaitForSeconds(dialogueDisplayTime);
            yield return StartCoroutine(FadeDialogue(noah3rdDialogue, false));
        }
    }

    private IEnumerator ShowAfterCallDialogues()
    {
        yield return new WaitForSeconds(afterCallDelay);

        // Fourth dialogue
        if (noah4thDialogue != null)
        {
            noah4thDialogue.gameObject.SetActive(true);
            noah4thDialogue.alpha = 0f;
            yield return StartCoroutine(FadeDialogue(noah4thDialogue, true));
            yield return new WaitForSeconds(dialogueDisplayTime);
            yield return StartCoroutine(FadeDialogue(noah4thDialogue, false));
            noah4thDialogue.gameObject.SetActive(false);
        }

        // Fifth dialogue
        if (noah5thDialogue != null)
        {
            noah5thDialogue.gameObject.SetActive(true);
            noah5thDialogue.alpha = 0f;
            yield return StartCoroutine(FadeDialogue(noah5thDialogue, true));
            yield return new WaitForSeconds(dialogueDisplayTime);
            yield return StartCoroutine(FadeDialogue(noah5thDialogue, false));
            noah5thDialogue.gameObject.SetActive(false);
        }
    }

    private IEnumerator FadeDialogue(TextMeshProUGUI dialogueText, bool fadeIn)
    {
        float elapsedTime = 0f;
        float startAlpha = fadeIn ? 0f : 1f;
        float endAlpha = fadeIn ? 1f : 0f;

        while (elapsedTime < dialogueFadeTime)
        {
            elapsedTime += Time.deltaTime;
            dialogueText.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / dialogueFadeTime);
            yield return null;
        }

        dialogueText.alpha = endAlpha;
    }

    private void StartRinging()
    {
        audioSource.clip = ringClip;
        audioSource.loop = true;
        if (!audioSource.isPlaying)
            audioSource.Play();
        
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

        textColor.a = 1f;
        objectiveText.color = textColor;
    }

    private IEnumerator FadeOutText()
    {
        float elapsedTime = 0f;
        Color textColor = objectiveText.color;
        float startAlpha = textColor.a;

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeOutDuration);
            textColor.a = alpha;
            objectiveText.color = textColor;
            yield return null;
        }

        textColor.a = 0f;
        objectiveText.color = textColor;
    }

    public void Interact(InteractionController interactionController)
    {
        if (hasInteracted) return;

        Debug.Log("Phone answered!");
        if (interactPrompt != null)
            interactPrompt.gameObject.SetActive(false);
        hasInteracted = true;

        if (objectiveText != null)
            StartCoroutine(FadeOutText());

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

        StartCoroutine(ShowDialogueSequence());

        yield return new WaitWhile(() => audioSource.isPlaying);

        if (endCallBeepClip != null)
        {
            for (int i = 0; i < numberOfBeeps; i++)
            {
                audioSource.clip = endCallBeepClip;
                audioSource.Play();
                yield return new WaitForSeconds(beepInterval);
            }
        }

        // Start the after-call dialogues once the beeps are done
        StartCoroutine(ShowAfterCallDialogues());
    }
} 