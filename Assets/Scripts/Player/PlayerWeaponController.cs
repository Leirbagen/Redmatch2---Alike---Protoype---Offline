using Rewired;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [Serializable]
    public struct WeaponLoad
    {
        public string categoryName;
        public WeaponBase[] weapons;
    }
    public List<WeaponBase> startingWeapons = new List<WeaponBase>();
    public Transform weaponParentSocket;
    public Transform defaultWeaponPosition;
    public Transform aimingPosition;
    public Camera playerCamera;
    public float aimSpeed = 9f;
    private WeaponBase[] weaponSlots = new WeaponBase[2];
    public WeaponLoad[] availableCategories;
    public int activeWeaponIndex { get; private set; }
    private WeaponBase currentWeapon;
    private bool isSwitchingAxis = false;
    public static event Action<int> OnWeaponSwitched;
    public static event Action OnWeaponsCleared;
    public static event Action<Sprite, int> OnWeaponIconReady;

    private void OnEnable()
    {
        WeaponUI.OnUIWakesUp += ForceUIUpdate;
        if (weaponSlots != null && weaponSlots[0] != null)
        {
            ForceUIUpdate();
        }
    }
    private void OnDisable()
    {
        WeaponUI.OnUIWakesUp -= ForceUIUpdate;
    }
    private void Awake()
    {
        activeWeaponIndex = -1;
        OnWeaponsCleared?.Invoke();
        int selectedIndex = PlayerPrefs.GetInt("SelectedWeaponCategory", 0);
        foreach (WeaponBase startingWeapon in availableCategories[selectedIndex].weapons)
        {
            AddWeapon(startingWeapon);
        }

        if (weaponSlots[0] != null) SwitchWeapon(0);
        ForceUIUpdate();
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;

        if (currentWeapon != null)
        {
            if (currentWeapon.isAutomatic)
            {
                if (InputController.Instance.GetButton(InputController.Input.FIRE_1)) currentWeapon.TryShoot();
            }
            else
            {
                if (InputController.Instance.GetButtonDown(InputController.Input.FIRE_1)) currentWeapon.TryShoot();
            }
            if (InputController.Instance.GetButtonDown(InputController.Input.RELOAD_WEAPON))
            {
                currentWeapon.StartReload();
            }
            if (InputController.Instance.GetButton(InputController.Input.AIM_WEAPON))
            {
                weaponParentSocket.position = Vector3.Lerp(weaponParentSocket.position, aimingPosition.position, Time.deltaTime * aimSpeed);
                currentWeapon.SetAiming(true);
            }
            else
            {
                weaponParentSocket.position = Vector3.Lerp(weaponParentSocket.position, defaultWeaponPosition.position, Time.deltaTime * aimSpeed);
                currentWeapon.SetAiming(false);
            }
            float scrollValue = InputController.Instance.GetAxis(InputController.Input.SCROLL_WHEEL);
            if (scrollValue > 0.1f)
            {
                if (!isSwitchingAxis)
                {
                    SwitchWeapon(activeWeaponIndex >= weaponSlots.Length - 1 ? 0 : activeWeaponIndex + 1);
                    isSwitchingAxis = true;
                }
            }
            else if (scrollValue < -0.1f)
            {
                if (!isSwitchingAxis)
                {
                    SwitchWeapon(activeWeaponIndex <= 0 ? weaponSlots.Length - 1 : activeWeaponIndex - 1);
                    isSwitchingAxis = true;
                }
            }
            else
            {
                isSwitchingAxis = false;
            }
        }
    }

    public void ForceUIUpdate()
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            OnWeaponIconReady?.Invoke(weaponSlots[i] != null ? weaponSlots[i].weaponIcon : null, i);
        }
        OnWeaponSwitched?.Invoke(activeWeaponIndex);
        if (currentWeapon != null) currentWeapon.RefreshAmmoUI();
    }

    private void AddWeapon(WeaponBase p_weaponPrefab)
    {
        weaponParentSocket.position = defaultWeaponPosition.position;
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] == null)
            {
                WeaponBase weaponClone = Instantiate(p_weaponPrefab, weaponParentSocket);
                weaponClone.gameObject.SetActive(false);
                weaponClone.InjectCamera(playerCamera);
                weaponSlots[i] = weaponClone;
                OnWeaponIconReady?.Invoke(weaponClone.weaponIcon, i);
                return;
            }
        }
    }

    private void SwitchWeapon(int newIndex)
    {
        if (weaponSlots[newIndex] == null) return;

        foreach (WeaponBase weapon in weaponSlots)
        {
            if (weapon != null)
            {
                weapon.SetAiming(false);
                weapon.gameObject.SetActive(false);
            }
        }
        weaponSlots[newIndex].gameObject.SetActive(true);
        activeWeaponIndex = newIndex;
        currentWeapon = weaponSlots[newIndex];
        OnWeaponIconReady?.Invoke(currentWeapon.weaponIcon, activeWeaponIndex);
        OnWeaponSwitched?.Invoke(activeWeaponIndex);
        currentWeapon.RefreshAmmoUI();
    }
}