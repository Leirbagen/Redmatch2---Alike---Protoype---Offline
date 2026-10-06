using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public int concurrentEnemies = 15;
    public int totalEnemiesForLevel = 25;
    public float respawnDelay = 3f;
    private int enemiesSpawned = 0;

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
        GameObject enemy = EnemyPool.Instance.RequestEnemy();
        Vector3 randomOffset = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
        enemy.transform.position = transform.position + randomOffset;
        enemiesSpawned++;
    }
    private IEnumerator RespawnMonitor()
    {
        while (true)
        {
            if (enemiesSpawned >= totalEnemiesForLevel)
            {
                yield break;
            }
            if (EnemyPool.Instance.ActiveEnemiesCount() < concurrentEnemies)
            {
                SpawnSingleEnemy();
            }
            yield return new WaitForSeconds(respawnDelay);
        }
    }
}