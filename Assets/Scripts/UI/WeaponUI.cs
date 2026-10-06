using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    public PlayerWeaponController playerWeaponController;
    public TMP_Text currentBullets;
    public TMP_Text totalBullets;
    public GameObject scopeDisplay;
    public Image[] weaponIcons;
    private Color activeColor = new Color(1f, 1f, 1f, 1f);
    private Color inactiveColor = new Color(0.6f, 0.6f, 0.6f, 0.4f);

    // Restauramos el evento que avisa que la UI despertó
    public event System.Action OnUIWakesUp;

    private void Start()
    {
        OnUIWakesUp?.Invoke();
    }

    private void OnEnable()
    {
        if (playerWeaponController != null)
        {
            playerWeaponController.OnAmmoChanged += UpdateBoth;
            playerWeaponController.OnWeaponSwitched += UpdateActiveWeaponIndex;
            playerWeaponController.OnWeaponsCleared += ClearWeaponUI;
            playerWeaponController.OnScopeUI += ToggleScope;
            playerWeaponController.OnWeaponIconReady += UpdateWeaponIcon;
        }
    }

    private void OnDisable()
    {
        if (playerWeaponController != null)
        {
            playerWeaponController.OnAmmoChanged -= UpdateBoth;
            playerWeaponController.OnWeaponSwitched -= UpdateActiveWeaponIndex;
            playerWeaponController.OnWeaponsCleared -= ClearWeaponUI;
            playerWeaponController.OnScopeUI -= ToggleScope;
            playerWeaponController.OnWeaponIconReady -= UpdateWeaponIcon;
        }
    }

    private void ToggleScope(bool isScoping)
    {
        if (scopeDisplay != null) scopeDisplay.SetActive(isScoping);
    }
    public void UpdateCurrent(int newCurrentBullets) => currentBullets.text = newCurrentBullets.ToString();
    public void UpdateTotal(int newTotalBullets) => totalBullets.text = newTotalBullets.ToString();

    public void UpdateBoth(int current, int total)
    {
        UpdateCurrent(current);
        UpdateTotal(total);
    }
    public void UpdateActiveWeaponIndex(int activeIndex)
    {
        for (int i = 0; i < weaponIcons.Length; i++)
        {
            if (weaponIcons[i] != null)
            {
                weaponIcons[i].color = (i == activeIndex) ? activeColor : inactiveColor;
            }
        }
    }
    public void UpdateWeaponIcon(Sprite newIcon, int index)
    {
        if (index >= 0 && index < weaponIcons.Length && weaponIcons[index] != null)
        {
            weaponIcons[index].sprite = newIcon;
            Color tempColor = weaponIcons[index].color;
            tempColor.a = newIcon != null ? 1f : 0f;
            weaponIcons[index].color = tempColor;
        }
    }
    public void ClearWeaponUI()
    {
        if (currentBullets != null) currentBullets.text = "--";
        if (totalBullets != null) totalBullets.text = "--";
        UpdateActiveWeaponIndex(-1);
    }
}