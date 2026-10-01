using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI labelText;
    private void OnEnable()
    {
        LevelManager.OnTimeUpdated += UpdateTimeUI;
        LevelManager.OnTimeOutLost += DisableTimerUI;

    }
    private void OnDisable()
    {
        LevelManager.OnTimeUpdated -= UpdateTimeUI;
        LevelManager.OnTimeOutLost -= DisableTimerUI;
    }

    private void UpdateTimeUI(float timeRemaining) 
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(Mathf.Max(0, timeRemaining));
        timeText.text = string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
    }
    private void DisableTimerUI() 
    {
        timeText.gameObject.SetActive(false);
        labelText.gameObject.SetActive(false);
    }
}
