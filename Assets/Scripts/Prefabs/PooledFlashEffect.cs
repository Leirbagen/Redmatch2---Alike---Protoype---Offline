using System.Collections;
using UnityEngine;

public class PooledFlashEffect : MonoBehaviour, IPooleable
{
    public string poolID = "ShotFlashEffect";
    public float lifeTime = 0.1f;

    public void OnObjectSpawn()
    {
        StartCoroutine(ReturnToPoolRoutine());
    }

    private IEnumerator ReturnToPoolRoutine()
    {
        yield return new WaitForSeconds(lifeTime);
        PoolManager.Instance.Release(poolID, gameObject);
    }
}