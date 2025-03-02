using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float inGameCountDuration = 0.4f; // Duration of one in-game count in seconds
    public int timeUnits = 400; // Initial time units
    public TextMeshProUGUI timeText;

    private float targetTime;
    private bool isTimerRunning = true;

    void Start()
    {
        targetTime = timeUnits * inGameCountDuration;
        UpdateTimeText();
    }

    void Update()
    {
        if (!isTimerRunning)
            return;

        targetTime -= Time.deltaTime;

        UpdateTimeText();

        // Check if the timer has ended
        if (targetTime <= 0.0f)
        {
            targetTime = 0.0f; // Ensure the timer doesn't go below zero
            timerEnded();
        }
    }

    void UpdateTimeText()
    {
        int remainingTimeUnits = Mathf.CeilToInt(targetTime / inGameCountDuration);
        timeText.text = $"Time\n{remainingTimeUnits:000}";
    }

    void timerEnded()
    {
        isTimerRunning = false;
        GameOverManager.Instance.GameOver();
        Debug.Log("Timer ended!");
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }
}