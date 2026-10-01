using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int totalEnemiesToKill;
    public float timeLimitSeconds;
    public string nextLevelName;

    public static event Action<float> OnTimeUpdated;
    public static event Action OnTimeOutLost;

    private int currentKills = 0;
    private float currentTime;
    private bool isLevelActive = true;
    private void OnEnable() 
    {
        EnemyController.OnEnemyDefeated += HandleEnemyKill;
    }
    private void OnDisable() 
    {
        EnemyController.OnEnemyDefeated -= HandleEnemyKill;
    } 

    private void Start()
    {
        currentTime = timeLimitSeconds;
    }

    private void Update()
    {
        if (!isLevelActive || Time.timeScale == 0f) return;
        currentTime -= Time.deltaTime;
        OnTimeUpdated?.Invoke(currentTime);
        if (currentTime <= 0)
        {
            TriggerDefeat();
        }
    }

    private void HandleEnemyKill()
    {
        if (!isLevelActive) return;

        currentKills++;
        if (currentKills >= totalEnemiesToKill)
        {
            TriggerVictory();
        }
    }

    private void TriggerVictory()
    {
        isLevelActive = false;
        SceneManager.LoadScene(nextLevelName);
    }

    private void TriggerDefeat()
    {
        isLevelActive = false;
        OnTimeOutLost?.Invoke();
    }
}
