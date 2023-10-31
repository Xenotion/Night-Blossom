using UnityEngine;
using TMPro;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float timerDuration = 10.0f; // 5 minutes in seconds
    private float remainingTime;
    private TextMeshProUGUI timerText; // Using TextMeshProUGUI instead of Text
    public GameObject victoryText;

    [Header("Player Settings")]
    public MonoBehaviour playerController; // Reference to the player controller script
    public MonoBehaviour mouseLookController; // Reference to the mouse look script
    public bool disablePlayerInputOnEnd = true; // Toggle to decide if player input should be disabled when timer ends

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

        StartCoroutine(AsyncLoadGameScene());
    }

    IEnumerator AsyncLoadGameScene()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("EndScreen", LoadSceneMode.Single);
        while (!asyncOperation.isDone)
        {
            Time.timeScale = 1f;

            yield return null; // Wait until the scene finishes loading
        }
    }

    public bool getIsTimerRunning() {
        return isTimerRunning;
    }
}
