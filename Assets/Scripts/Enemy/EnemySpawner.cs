using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int concurrentEnemies = 15;
    public int totalEnemiesForLevel = 25;
    public float respawnDelay = 3f;
    private int enemiesSpawned = 0;
    private int currentActiveEnemies = 0;

    private void OnEnable()
    {
        EnemyController.OnEnemyDefeated += HandleEnemyDeath;
    }

    private void OnDisable()
    {
        EnemyController.OnEnemyDefeated -= HandleEnemyDeath;
    }
    void Start()
    {
        int initialSpawns = Mathf.Min(concurrentEnemies, totalEnemiesForLevel);

        for (int i = 0; i < initialSpawns; i++)
        {
            SpawnSingleEnemy();
        }

        StartCoroutine(RespawnMonitor());
    }
    private void SpawnSingleEnemy()
    {
        Vector3 randomOffset = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
        Vector3 spawnPosition = transform.position + randomOffset;
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        enemiesSpawned++;
        currentActiveEnemies++;
    }
    private void HandleEnemyDeath()
    {
        currentActiveEnemies--; 
    }
    private IEnumerator RespawnMonitor()
    {
        while (enemiesSpawned < totalEnemiesForLevel)
        {
            if (currentActiveEnemies < concurrentEnemies)
            {
                SpawnSingleEnemy();
            }
            yield return new WaitForSeconds(respawnDelay);
        }
    }
}