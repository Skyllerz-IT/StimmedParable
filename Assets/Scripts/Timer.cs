using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float totalTime = 300f; // 5 minutes in seconds
    private float timeRemaining;
    private bool isGameOver = false;

    public GameObject gameOverUI;
    public GameObject HUD;
    public TextMeshProUGUI timerText;
    
    public float GetProgress()
    {
        return 1f - Mathf.Clamp01(timeRemaining / totalTime);
    }

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
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
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