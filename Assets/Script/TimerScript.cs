using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    public float timerDuration = 300.0f; // 5 minutes in seconds
    private float remainingTime;
    private TextMeshProUGUI timerText; // Using TextMeshProUGUI instead of Text
    public TMP_Text victoryText;

    private bool isTimerRunning = false; // To check if the timer has started

    private void Start()
    {
        victoryText.gameObject.SetActive(false);
        remainingTime = timerDuration;
        timerText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (isTimerRunning && remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(remainingTime / 60F);
            int seconds = Mathf.FloorToInt(remainingTime - minutes * 60);
            timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        }
        else if (isTimerRunning && remainingTime <= 0)
        {
            // Timer has ended
            TimerEnded();
        }
    }

    public void StartTimer() // Call this method to start the timer
    {
        isTimerRunning = true;
    }

    private void TimerEnded()
    {
        timerText.text = "Time's Up!";
        victoryText.gameObject.SetActive(true);
    }

    public bool getIsTimerRunning() {
        return isTimerRunning;

    }
}
