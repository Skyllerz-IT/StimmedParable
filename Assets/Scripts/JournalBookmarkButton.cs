using UnityEngine;
using UnityEngine.UI;

public class JournalBookmarkButton : MonoBehaviour
{
    [Header("Page Switching")]
    public GameObject currentPage; // The page to deactivate
    public GameObject targetPage;  // The page to activate
    public Button button;          // Optional: assign if not on same GameObject

    private void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();
        if (button != null)
            button.onClick.AddListener(SwitchPage);
    }

    public void SwitchPage()
    {
        if (currentPage != null)
            currentPage.SetActive(false);
        if (targetPage != null)
            targetPage.SetActive(true);
        // Optionally: play sound or animation here
    }
} 