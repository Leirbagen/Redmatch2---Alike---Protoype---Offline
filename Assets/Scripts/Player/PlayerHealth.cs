using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, IDamageable
{

    public int maxHealth = 100;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
    }
    public void TakeDamage(int damageInt)
    {
        currentHealth -= damageInt;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateUI();
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateUI()
    {
        if (HealthUI.Instance != null)
        {
            HealthUI.Instance.UpdateHealth(currentHealth);
        }
    }

    private void Die()
    {
        string actualScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(actualScene);
    }
}