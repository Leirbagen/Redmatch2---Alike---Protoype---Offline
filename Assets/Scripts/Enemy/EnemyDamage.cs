using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damageTouch = 10; 
    public float damageInterval = 1f; 
    private float nextDamageTime;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time >= nextDamageTime)
            {
                IDamageable damageableTarget = collision.gameObject.GetComponent<IDamageable>();
                if (damageableTarget != null)
                {
                    damageableTarget.TakeDamage(damageTouch);
                    nextDamageTime = Time.time + damageInterval;
                }
            }
        }
    }
}
