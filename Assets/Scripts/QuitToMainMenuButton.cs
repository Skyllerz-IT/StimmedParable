using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitToMainMenuButton : MonoBehaviour
{
    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
} 