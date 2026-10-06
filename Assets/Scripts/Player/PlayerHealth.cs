using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;
    public event Action<int> OnHealthChange;
    public event Action OnPlayerDied;

    private void Start()
    {
        isDead = false;
        currentHealth = maxHealth;
        OnHealthChange?.Invoke(currentHealth);
    }
    public void TakeDamage(int damageInt)
    {
        if (isDead) 
        {
            return;
        }
        currentHealth -= damageInt;
        currentHealth = Mathf.Max(currentHealth, 0);
        OnHealthChange?.Invoke(currentHealth);
        if (currentHealth <= 0)
        {
            isDead = true;
            OnPlayerDied?.Invoke();
        }
    }
}