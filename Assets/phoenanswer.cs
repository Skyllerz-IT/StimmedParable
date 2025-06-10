using UnityEngine;
using TMPro;

public class PhoneInteraction : MonoBehaviour
{
    public TextMeshProUGUI interactPrompt;
    public float interactionDistance = 2.0f;

    private Transform player;
    private bool hasInteracted = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        interactPrompt.gameObject.SetActive(false);
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (!hasInteracted && distance < interactionDistance)
        {
            interactPrompt.gameObject.SetActive(true);

#if UNITY_EDITOR || UNITY_STANDALONE
            if (Input.GetKeyDown(KeyCode.E))
            {
                Interact();
            }
#endif

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform == transform)
                {
                    Interact();
                }
            }
        }
        else if (!hasInteracted)
        {
            interactPrompt.gameObject.SetActive(false);
        }
    }

    void Interact()
    {
        Debug.Log("Phone answered!");
        interactPrompt.gameObject.SetActive(false);
        hasInteracted = true;

        // 👉 Do stuff here: play audio, timeline, cutscene, etc.
    }
}