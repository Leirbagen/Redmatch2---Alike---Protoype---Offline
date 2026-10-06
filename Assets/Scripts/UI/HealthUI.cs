using UnityEngine;
using TMPro;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    public PlayerHealth playerHealth;

    private void OnEnable() 
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChange += UpdateHealth; 
        }
    }

    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChange -= UpdateHealth;
        }
    }
    public void UpdateHealth(int currentHealth)
    {
        if (healthText != null)
        {
            healthText.text = currentHealth.ToString();
        }
    }
}