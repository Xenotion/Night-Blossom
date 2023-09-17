using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timerDuration = 300.0f; // 5 minutes in seconds
    private float remainingTime;
    private TextMeshProUGUI timerText; // Using TextMeshProUGUI instead of Text

    private void Start()
    {
        remainingTime = timerDuration;
        timerText = GetComponent<TextMeshProUGUI>(); // Get TextMeshProUGUI component
    }

    private void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(remainingTime / 60F);
            int seconds = Mathf.FloorToInt(remainingTime - minutes * 60);
            timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        }
        else
        {
            // Timer has ended
            TimerEnded();
        }
    }

    private void TimerEnded()
    {
        timerText.text = "Time's Up!";
        // Implement additional actions here
    }
}
