using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class QuitButtonHandler : MonoBehaviour
{
    private void Start()
    {
        // Get the button component and add the quit listener
        Button button = GetComponent<Button>();
        button.onClick.AddListener(HandleQuit);
    }

    private void HandleQuit()
    {
        GameManager.GetInstance()?.QuitGame();
    }
} 