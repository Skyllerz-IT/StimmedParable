using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public float totalTime = 300f; // 5 minutes in seconds
    private float timeRemaining;
    private bool isGameOver = false;

    public GameObject gameOverUI;
    public GameObject HUD;

    void Start()
    {
        timeRemaining = totalTime;
        UpdateTimerText();
        gameOverUI.SetActive(false);
    }

    void Update()
    {
        if (isGameOver) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerText();
        }
        else
        {
            EndGame();
        }
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
    }

    void EndGame()
    {
        isGameOver = true;
        timeRemaining = 0;
        UpdateTimerText();
        HUD.SetActive(false);
        gameOverUI.SetActive(true);
        Debug.Log("Game Over");
    }
}