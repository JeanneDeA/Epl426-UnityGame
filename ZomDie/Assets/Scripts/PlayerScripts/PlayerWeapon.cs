using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;


//TODO:Combine animations 
public class PlayerWeapon : MonoBehaviour
{
    [Header("General")]
    //General
    private Camera m_PlayerCamera;

    private Animator m_animator;

    [Header("Shooting Settings")]
    //Shotting
    [SerializeField]
    private GameObject m_muzzleFlashEffect;

    [SerializeField]
    private Transform m_FirePoint;

    [SerializeField]
    public WeaponADS m_weaponADS;

    private float m_spreadIntensity = 0f;

    [SerializeField]
    private float m_hipSpreadIntensity = 0f;

    [SerializeField]
    private float m_adsSpreadIntensity = 0f;

    [SerializeField]
    private float m_timeBetweenShooting = 0.1f;

    private bool m_isShooting = false;

    private bool m_isAiming = false;

    private bool m_readyToShoot = false;

    private Coroutine m_shootCoroutine; // internal coroutine handle

    public ShootingModes m_currentShootingMode = ShootingModes.SingleMode;

    public WeaponModel m_currentWeaponModel;

    [Header("Bullet Settings")]
    //Bullet 
    [SerializeField]
    private GameObject m_BulletPrefab;

    [SerializeField]
    private int m_bulletsPerTap = 3;

    [SerializeField]
    private float m_bulletLifeTime = 5f;

    [SerializeField]
    private float m_BulletSpeed = 40f;

    [SerializeField]
    private int m_bulletDamage = 10;

    public LayerMask hitLayers; // assign in inspector to Environment, Enemies, etc.

    //Loading
    [SerializeField]
    private float m_reloadTime;
    [SerializeField]
    private int m_magazineSize;

    private int m_bulletsLeft;

    private bool m_isReloading;

    public bool m_canUpdateAmmoDisplay = false;


    [Header("WeaponSpawnPosition")]

    [SerializeField]
    private Vector3 m_weaponSpawnPosition;

    [SerializeField]
    private Vector3 m_weaponSpawnRotation;

    private void Awake()
    {
        m_readyToShoot = true;
        m_animator = GetComponentInChildren<Animator>();

        m_bulletsLeft = m_magazineSize;
        // Try to find a Camera on this GameObject or any parent
        AssignCamera();
        m_spreadIntensity = m_hipSpreadIntensity;
    }

    private void Update()
    {
        if (!m_canUpdateAmmoDisplay)
        {
            return;
        }
        
    }   

    public void Fire()
    {
        // cannot shoot while reloading
        if (m_bulletsLeft<=0)
        {
            SoundManager.m_instance.PlayEmptyMagazineSound(m_currentWeaponModel);
            Debug.LogWarning("No bullets left. Press R to Reload");
            return;
        }

        // validate prefab and fire point
        if (m_BulletPrefab == null || m_FirePoint == null)
        {
            Debug.LogWarning("PlayerWeapon.Fire: missing prefab or fire point.");
            return;
        }

        // spawn bullet at fire point
        GameObject bullet = Instantiate(m_BulletPrefab, m_FirePoint.position, m_FirePoint.rotation);
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        bulletComponent.SetDamage(m_bulletDamage);
        if (bullet == null)
        {
            return;
        }

        // spawn muzzle flash effect
        if (m_muzzleFlashEffect == null)
        {
            Debug.LogWarning("PlayerWeapon.Fire: missing muzzle flass efect.");
            return;
        }
        m_bulletsLeft--;
        m_muzzleFlashEffect.GetComponent<ParticleSystem>().Play();
        SoundManager.m_instance.PlayShootingSound(m_currentWeaponModel);
        ExecuteAnimation(AnimationEnums.RECOIL, m_isAiming);
        ExecuteAnimation(AnimationEnums.SHOOT, m_isAiming);

        // ensure it has a non-kinematic Rigidbody and collider on prefab
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = bullet.AddComponent<Rigidbody>();
        }
        rb.isKinematic = false;
        rb.useGravity = true;

        // use continuous collision detection for fast projectiles
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

      
        Vector3 shotDirection = CalculateDirectionAndSpread(m_spreadIntensity);
 
        // set velocity
        rb.velocity = shotDirection * m_BulletSpeed;

        // rotate the bullet so its forward faces the velocity direction
        bullet.transform.rotation = Quaternion.LookRotation(rb.velocity.normalized);
   
        // destroy after life time
        Destroy(bullet, m_bulletLifeTime);
    }

    public void Reload()
    {
        if(!WeaponManager.m_instance.CanReload(m_currentWeaponModel))
        {
            return;
        }
        if (!(m_bulletsLeft < m_magazineSize) || m_isReloading || m_isShooting)
        {
            return;
        }
        ExecuteAnimation(AnimationEnums.RELOAD, m_isAiming);
        SoundManager.m_instance.PlayReloadingSound(m_currentWeaponModel);
        Debug.Log("PlayerWeapon.Reload: Reloading weapon.");
        m_isReloading = true;
        Invoke("ReloadCompleted", m_reloadTime);
    }

    private void ReloadCompleted()
    {
        if (!m_isReloading)
        {
            return;
        }
        int total = WeaponManager.m_instance.GetTotalAmmoAmount(m_currentWeaponModel);
        if (total >= m_magazineSize)
        {
            WeaponManager.m_instance.ConsumeAmmo(m_currentWeaponModel, m_magazineSize-m_bulletsLeft);
            m_bulletsLeft = m_magazineSize;
            
        }
        else 
        {
            m_bulletsLeft +=total;
            WeaponManager.m_instance.SetTotalAmmoAmount(m_currentWeaponModel, m_bulletsLeft-m_magazineSize);
            if(m_bulletsLeft > m_magazineSize)
            {
                m_bulletsLeft = m_magazineSize;
            }

        }

            m_isReloading = false;
    }


    public Vector3 CalculateDirectionAndSpread(float spreadIntensity)
    {
        Ray ray = m_PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        Vector3 targetpoint;

        if (Physics.Raycast(ray, out hit, 1000f, hitLayers))
        {
            targetpoint = hit.point;
        }
        else
        {
            targetpoint = ray.GetPoint(100);
        }

        Vector3 direction = targetpoint - m_FirePoint.position;

        float spreadZ = Random.Range(-spreadIntensity, spreadIntensity);
        float spreadY = Random.Range(-spreadIntensity, spreadIntensity);
        Vector3 spreadDirection = direction + new Vector3(0, spreadY, spreadZ);

        return spreadDirection.normalized;
    }

    public void StartShooting()
    {
        // Called on input started (Shoot.started)
        switch (m_currentShootingMode)
        {
            case ShootingModes.AutoMaticMode:
                // start continuous firing while held
                if (m_shootCoroutine == null)
                {
                    m_isShooting = true;
                    m_shootCoroutine = StartCoroutine(AutomaticFire());
                }
                break;

            case ShootingModes.BurstMode:
                // require ready flag, start burst, and block further bursts until release
                if (!m_readyToShoot) return;
                m_readyToShoot = false; // require release to enable next burst
                if (m_shootCoroutine == null)
                    m_shootCoroutine = StartCoroutine(BurstFire());
                break;

            case ShootingModes.SingleMode:
            default:
                // single: only fire once per press; require release to allow another shot
                if (!m_readyToShoot) return;
                Fire();
                m_readyToShoot = false; // will be re-enabled on StopShooting (release)
                break;
        }
    }

    private IEnumerator AutomaticFire()
    {
        try
        {
            // Fire immediately, then wait fixed interval between shots while the trigger is held.
            while (m_isShooting)
            {
                Fire();
                yield return new WaitForSeconds(m_timeBetweenShooting);
            }
        }
        finally
        {
            // ensure state is cleaned up when coroutine ends or is stopped
            m_shootCoroutine = null;
            m_isShooting = false;
            m_readyToShoot = true; // allow firing again after release
        }
    }

    private IEnumerator BurstFire()
    {
        try
        {
            int shots = m_bulletsPerTap;
            for (int i = 0; i < shots; i++)
            {
                Fire();
                // spacing between shots in the burst
                yield return new WaitForSeconds(m_timeBetweenShooting);
            }

            // after burst, keep m_readyToShoot == false until user releases (StopShooting)
        }
        finally
        {
            // clear handle; do not re-enable m_readyToShoot here (user must release)
            m_shootCoroutine = null;
        }
    }

    public void StopShooting()
    {
        // Called on input canceled (Shoot.canceled)
        switch (m_currentShootingMode)
        {
            case ShootingModes.AutoMaticMode:
                // stop automatic fire immediately
                m_isShooting = false;
                if (m_shootCoroutine != null)
                {
                    StopCoroutine(m_shootCoroutine);
                    m_shootCoroutine = null;
                }
                m_readyToShoot = true;
                break;

            case ShootingModes.BurstMode:
                // user released after burst: allow next burst
                m_readyToShoot = true;
                break;

            case ShootingModes.SingleMode:
            default:
                // single shot mode: release allows next shot
                m_readyToShoot = true;
                break;
        }
    }

    public void AssignCamera()
    {
        if (m_PlayerCamera == null)
        {
            m_PlayerCamera = GetComponentInParent<Camera>();

            // if the camera is on a child of the parent (rare), try parent's children
            if (m_PlayerCamera == null && transform.parent != null)
            {
                m_PlayerCamera = transform.parent.GetComponentInChildren<Camera>();
            }


            // fallback to the tagged main camera
            if (m_PlayerCamera == null)
            {
                m_PlayerCamera = Camera.main;
            }


            if (m_PlayerCamera == null)
            {
                Debug.LogWarning("PlayerWeapon: no Camera found on parent or Camera.main. Assign in Inspector if needed.");
            }

        }
    }

    public void AimWeapon(bool isAiming)
    {

        m_weaponADS.SetAiming(isAiming);
        HUDManager.m_instance.ShowSights(!isAiming);
        m_spreadIntensity = isAiming ? m_adsSpreadIntensity : m_hipSpreadIntensity;

        m_isAiming = isAiming;
    }

    public Vector3 GetWeaponSpawnPosition()
    {
        return m_weaponSpawnPosition;
    }

    public Vector3 GetWeaponSpawnRotation()
    {
        return m_weaponSpawnRotation;
    }

    public int GetBulletsLeft()
    {
        return m_bulletsLeft;
    }

    public int GetBulletsPerBurst()
    {
        return m_bulletsPerTap;
    }

    public int GetMagazineSize()
    {
        return m_magazineSize;
    }

    private void ExecuteAnimation(AnimationEnums animationEnum,bool isAiming)
    {
    
        switch (animationEnum)
            {
                case AnimationEnums.RELOAD:
                    m_animator.SetTrigger("RELOAD");
                    break;
                case AnimationEnums.RECOIL:
                    m_animator.SetTrigger("RECOIL");
                    break;
                case AnimationEnums.SHOOT:
                    m_animator.SetTrigger("SHOOT");
                    break;
                default:
                    Debug.LogWarning("No Animation Found");
                    break;
            }
        
    }

    public void SetWeaponToRenderCameraLayer()
    {
        int layer = LayerMask.NameToLayer("WeaponRender");
        foreach (Transform t in GetComponentsInChildren<Transform>(true))
        {
            t.gameObject.layer = layer;
        }
    }

    public void SetWeaponToDefaultLayer()
    {
        int layer = LayerMask.NameToLayer("Interactable");
        foreach (Transform t in GetComponentsInChildren<Transform>(true))
        {
            t.gameObject.layer = layer;
        }
    }


}
