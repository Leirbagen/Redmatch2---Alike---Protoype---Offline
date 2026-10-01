using UnityEngine;
using TMPro;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;

    private void OnEnable() 
    {
        PlayerHealth.OnHealthChange += UpdateHealth;
    }

    private void OnDisable()
    {
        PlayerHealth.OnHealthChange -= UpdateHealth;
    }
    public void UpdateHealth(int currentHealth)
    {
        if (healthText != null)
        {
            healthText.text = currentHealth.ToString();
        }
    }
}