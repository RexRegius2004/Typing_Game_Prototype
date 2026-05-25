using UnityEngine;
using TMPro;
using System;
public class TimerScript : MonoBehaviour
{
    [Header("Timer Settings")]
    public float timelimit = 60f;

    [Header("UI")]
    public TextMeshProUGUI timerTextUI;

    private float currentTime;
    private bool isRunning = true;

    public Action OnTimerEnd;

    public UpgradeManager upgradeManager;

    // Start is called once before the first execution of Update after the MonoBehavior is created
    void Start()
    {
        currentTime = timelimit + upgradeManager.currentDelayBonus;
        UpdateTimerUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRunning) return;

        currentTime -= Time.deltaTime;
        Debug.Log("Timer started with " + currentTime + " seconds.");

        if (currentTime <= 0)
        {
            currentTime = 0;
            isRunning = false;

            UpdateTimerUI();
            OnTimerEnd?.Invoke(); // Notify listeners
            return;
        }

        UpdateTimerUI();
    }
    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        timerTextUI.text = $"{minutes:00}:{seconds:00}";

        
        if (currentTime <= 10f)
            timerTextUI.color = Color.red;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public float GetRemainingTime()
    {
        return currentTime;
    }

}
