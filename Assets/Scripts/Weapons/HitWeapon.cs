using DamageNumbersPro;
using System;
using UnityEngine;
public class HitWeapon : WeaponBase
{
    public bool useScope = false;
    public float scopedZoom = 10f; 
    public GameObject weaponMesh;
    public float fireRange = 200f;
    public LayerMask wallsHitLayer;
    public LayerMask hitLayer;
    public float backForce = 4f;
    private Camera playerCam;
    private float defaultZoom;
    private bool isAiming = false;

    [SerializeField] private int weaponDamage;
    public Transform weaponNozzle;
    public GameObject bulletHole;
    public AudioSource weaponAudio;
    public AudioClip shootSound;
    private Transform cameraPlayerTransform;
    public DamageNumber numberPrefab;

    public event Action<bool> OnScopeUI;

    protected override void Start()
    {
        base.Start(); 
    }

    public override void InjectCamera(Camera cam)
    {
        playerCam = cam;
        cameraPlayerTransform = cam.transform;
        if (playerCam != null)
        {
            defaultZoom = playerCam.fieldOfView;
        }
    }
    private void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * 5f);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, Time.deltaTime * 5f);

        if (useScope && playerCam != null)
        {
            float targetZoom = isAiming ? scopedZoom : defaultZoom;
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, targetZoom, Time.deltaTime * 15f);
        }
    }
    public override void SetAiming(bool aiming)
    {
        if (isAiming == aiming) return;
        isAiming = aiming;
        if (useScope)
        {
            if (weaponMesh != null)
            {
                weaponMesh.SetActive(!aiming);
            }
            OnScopeUI?.Invoke(aiming);
        }
    }
    protected override void ExecuteShoot()
    {
        if (weaponAudio != null && shootSound != null)
        {
            weaponAudio.PlayOneShot(shootSound);
        }
        if (weaponNozzle != null)
        {
            GameObject flashClone = PoolManager.Instance.Get("ShotFlashEffect", weaponNozzle.position, weaponNozzle.rotation);

            if (flashClone != null)
            {
                flashClone.transform.SetParent(transform);
            }
        }
        AddRecoil();
        if (Physics.Raycast(cameraPlayerTransform.position, cameraPlayerTransform.forward, out RaycastHit hit, fireRange, hitLayer | wallsHitLayer))
        {
            IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
            if (damageableObject != null)
            {
                damageableObject.TakeDamage(weaponDamage);
                numberPrefab.Spawn(hit.point, weaponDamage);
            }
            else 
            {
                GameObject bulletHoleClone = Instantiate(bulletHole, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal));
                Destroy(bulletHoleClone, 4f);
            }
        }
    }
    private void AddRecoil()
    {
        transform.Rotate(-backForce, 0, 0);
        transform.position -= transform.forward * (backForce / 50f);
    }
}