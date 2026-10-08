using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public int maxHealth = 100;
    private int currentHealth;

    public static event Action<int> OnHealthChange;
    public static event Action OnPlayerDied;

    private void Start()
    {
        currentHealth = maxHealth;
        OnHealthChange?.Invoke(currentHealth);
    }
    public void TakeDamage(int damageInt)
    {
        currentHealth -= damageInt;
        currentHealth = Mathf.Max(currentHealth, 0);
        OnHealthChange?.Invoke(currentHealth);
        if (currentHealth <= 0)
        {
            OnPlayerDied?.Invoke();
        }
    }
}