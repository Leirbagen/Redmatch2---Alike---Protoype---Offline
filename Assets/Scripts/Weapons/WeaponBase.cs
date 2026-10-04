using UnityEngine;
using System.Collections;
using System;

public abstract class WeaponBase : MonoBehaviour
{
    public int maxAmmo = 8;
    public float fireInterval = 0.5f;
    public float reloadTime = 1f;
    public Sprite weaponIcon;
    public Sprite ammoIcon;
    public int currentAmmo { get; protected set; }
    protected bool canShoot = true;
    protected bool isReloading = false;
    public bool isAutomatic = false;
    public static event Action<int, int> OnAmmoChanged;

    protected virtual void Start()
    {
        currentAmmo = maxAmmo;
    }
    protected virtual void OnEnable()
    {
        canShoot = true;
        isReloading = false;
    }
    public virtual void TryShoot()
    {
        if (canShoot && !isReloading && currentAmmo > 0 && gameObject.activeInHierarchy)
        {
            StartCoroutine(ShootRoutine());
        }
    }
    public virtual void SetAiming(bool aiming) { }
    public void RefreshAmmoUI()
    {
        OnAmmoChanged?.Invoke(currentAmmo, maxAmmo);
    }
    private IEnumerator ShootRoutine()
    {
        canShoot = false;
        currentAmmo--;
        OnAmmoChanged?.Invoke(currentAmmo, maxAmmo);
        ExecuteShoot();
        yield return new WaitForSeconds(fireInterval);
        canShoot = true;
    }
    public virtual void StartReload()
    {
        if (!isReloading && currentAmmo < maxAmmo && gameObject.activeInHierarchy)
        {
            StartCoroutine(ReloadRoutine());
        }
    }
    private IEnumerator ReloadRoutine()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        OnAmmoChanged?.Invoke(currentAmmo, maxAmmo);
        isReloading = false;
    }
    protected abstract void ExecuteShoot();
}