using UnityEngine;
using TMPro;

public class HealthUI : MonoBehaviour
{
    public static HealthUI Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI healthText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void UpdateHealth(int currentHealth)
    {
        if (healthText != null)
        {
            healthText.text = currentHealth.ToString();
        }
    }
}