using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class WeaponManager : MonoBehaviour
{

    public static WeaponManager m_instance { get; private set; }

    public List<GameObject> m_weaponsInInventory = new List<GameObject>();

    public GameObject m_currentWeaponSlot = null;

    private int m_TotalRifleAmmo = 31;

    private int m_TotalPistolAmmo = 16;

    private int m_lethalCount = 0;

    private int m_tacticalCount = 0;




    [Header("Throwable Settings")]
    public GameObject m_throwableSpawnPoint;
    private float m_forceMultiplier = 0.0f;
    public float m_maxforceMultiplier = 3.0f;
 

    [Header("Lethal throwable Settings")]
    public float m_lethalThrowForce = 10f;
    public int m_lethalCountMax = 2;
    public GameObject m_grenadePrefab;
    public ThrowableType m_equipedLethalType;

    [Header("Tactical throwable Settings")]
    public float m_tacticalThrowForce = 10f;
    public int m_tacticalCountMax = 2;
    public GameObject m_smokeGrenadePrefab;
    public ThrowableType m_equipedTacticalType;

    private void Awake()
    {
        if (m_instance != null && m_instance != this)
        {
            Destroy(gameObject);

        }
        else
        {
            m_instance = this;
        }
        if(m_currentWeaponSlot !=null)
        {
            PlayerWeapon currentPlayerWeapon = m_currentWeaponSlot.transform.GetChild(0).GetComponent<PlayerWeapon>();
            currentPlayerWeapon.GetComponent<BoxCollider>().enabled = false;
        }
        
    }

    private void Start()
    {
        m_currentWeaponSlot = m_weaponsInInventory[0];
        m_equipedLethalType = ThrowableType.None;
        m_equipedTacticalType = ThrowableType.None;
    }

    public void PickUpWeapon(GameObject weapon)
    {
        
        DropCurrentWeapon();
        AddWeaponIntoActiveSlot(weapon);
        ActivateWeaponComponets(weapon);
        weapon.GetComponent<PlayerWeapon>().SetWeaponToRenderCameraLayer();
    }

    private void ActivateWeaponComponets(GameObject weapon)
    {
        weapon.GetComponentInChildren<Animator>().enabled = true;
        weapon.GetComponent<BoxCollider>().enabled = false;
        // disable physics on the weapon so it won't be simulated while held
        DisablePhysics(weapon);
        PlayerWeapon playerWeapon = weapon.GetComponent<PlayerWeapon>();
        playerWeapon.m_canUpdateAmmoDisplay = true;
    }

    private void DisablePhysics(GameObject weapon)
    {
        Rigidbody rb = weapon.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;            // stops physics simulation
            rb.useGravity = false;            // optional: stop gravity while held
            rb.velocity = Vector3.zero;       // clear motion
            rb.angularVelocity = Vector3.zero;
            // optionally disable collision detection so it doesn't hit the holder
            Collider col = weapon.GetComponent<Collider>();
            if (col != null) col.enabled = false;
        }
    }

    private void DeactivateWeaponComponents(GameObject weapon)
    {
        weapon.GetComponentInChildren<Animator>().enabled = false;
        PlayerWeapon playerWeapon = weapon.GetComponent<PlayerWeapon>();
        playerWeapon.m_canUpdateAmmoDisplay = false;

    }

    public void DropCurrentWeapon()
    {
        if (m_currentWeaponSlot.transform.childCount > 0)
        {
            GameObject weaponToDrop = m_currentWeaponSlot.transform.GetChild(0).gameObject;
            WeaponADS weaponADS = weaponToDrop.GetComponent<WeaponADS>();

            weaponADS.SetIsActive(false);
            weaponADS.SetAiming(false);

            // Deactivate weapon components (stop updating UI/animator as a held weapon)
            DeactivateWeaponComponents(weaponToDrop);
            weaponToDrop.GetComponent<BoxCollider>().enabled = true;
            // Detach from parent -> becomes root in scene hierarchy
            weaponToDrop.transform.SetParent(null, true);
            weaponToDrop.GetComponent<PlayerWeapon>().SetWeaponToDefaultLayer();
            // Ensure there's a collider (prefer existing; add BoxCollider fallback)
            Collider col = weaponToDrop.GetComponent<Collider>();
            if (col == null)
            {
                col = weaponToDrop.AddComponent<BoxCollider>();
            }
            col.enabled = true;

            // Ensure Rigidbody exists and is simulated
            Rigidbody rb = weaponToDrop.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = weaponToDrop.AddComponent<Rigidbody>();
            }
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;


            // Apply a small toss so it drops/rolls naturally (use player's forward if available)
            Vector3 tossDirection = Vector3.up * 0.25f;
            rb.AddForce(tossDirection, ForceMode.Impulse);
        }
    }

    private void AddWeaponIntoActiveSlot(GameObject weapon)
    {
        if (weapon == null)
        {
            Debug.LogError("AddWeaponIntoActiveSlot: weapon is null");
            return;
        }
       
        weapon.transform.SetParent(m_currentWeaponSlot.transform, false);
        PlayerWeapon playerWeapon = weapon.GetComponent<PlayerWeapon>();
        WeaponADS weaponADS = weapon.GetComponent<WeaponADS>();
        weaponADS.SetIsActive(true);
        playerWeapon.m_canUpdateAmmoDisplay = true;
        Debug.Log("Weapon Spawn Position: " + playerWeapon.GetWeaponSpawnPosition());
        weapon.transform.localPosition = playerWeapon.GetWeaponSpawnPosition();
        weapon.transform.localRotation = Quaternion.Euler(playerWeapon.GetWeaponSpawnRotation());
    }

    public void SwitchActiveSlot(int slotnumber)
    {
        if(m_currentWeaponSlot.transform.childCount > 0)
        {
           PlayerWeapon currentPlayerWeapon = m_currentWeaponSlot.transform.GetChild(0).GetComponent<PlayerWeapon>();
           DeactivateWeaponComponents(m_currentWeaponSlot.transform.GetChild(0).gameObject);
        }
        m_currentWeaponSlot = m_weaponsInInventory[slotnumber];
        if(m_currentWeaponSlot.transform.childCount > 0)
        {
            PlayerWeapon newPlayerWeapon = m_currentWeaponSlot.transform.GetChild(0).GetComponent<PlayerWeapon>();
            ActivateWeaponComponets(m_currentWeaponSlot.transform.GetChild(0).gameObject);
        }
    }

    public void Update()
    {
        foreach (GameObject weaponSlot in m_weaponsInInventory)
        {
            if (weaponSlot == m_currentWeaponSlot)
            {
                weaponSlot.SetActive(true);
            }
            else
            {
                weaponSlot.SetActive(false);
            }
        }
    }

    public GameObject GetUnactiveWeaponSlot()
    {
        foreach (GameObject weaponSlot in m_weaponsInInventory)
        {
            if (weaponSlot != m_currentWeaponSlot)
            {
                return weaponSlot;
            }
        }
        return null;
    }

    public void PickUpAmmo(AmmoBox ammoBox)
    {
        if(ammoBox == null)
        {
            Debug.LogError("PickUpAmmo: ammoBox is null");
            return;
        }
        switch(ammoBox.m_ammoType)
        {
            case AmmoType.Rifle_Ammo:
                m_TotalRifleAmmo += ammoBox.GetAmmoAmount();
                break;
            case AmmoType.Pistol_Ammo:
                m_TotalPistolAmmo += ammoBox.GetAmmoAmount();
                break;
            default:
                Debug.LogError("PickUpAmmo: Unknown ammo type");
                break;
        }
    }

    public int GetTotalAmmoAmount(WeaponModel weapon)
    {
        AmmoType ammoType;
        switch (weapon)
        {
            case WeaponModel.AK74:
                ammoType = AmmoType.Rifle_Ammo;
                break;
            case WeaponModel.M1911:
                ammoType = AmmoType.Pistol_Ammo;
                break;
            default:
                return 0;
        }
        switch (ammoType)
        {
            case AmmoType.Rifle_Ammo:
                return m_TotalRifleAmmo;
            case AmmoType.Pistol_Ammo:
                return m_TotalPistolAmmo;
            default:
                return 0;
        }
    }

    public bool CanReload(WeaponModel weapon)
    {
       int totalAmmo = GetTotalAmmoAmount(weapon);
       if(totalAmmo <= 0)
        {
            return false;
        }
        return true;
    }

    public void ConsumeAmmo(WeaponModel weapon, int amount)
    {
        switch(weapon)
        {
            case WeaponModel.AK74:
                m_TotalRifleAmmo -= amount;
                if(m_TotalRifleAmmo < 0)
                {
                    m_TotalRifleAmmo = 0;
                }
                break;
            case WeaponModel.M1911:
                m_TotalPistolAmmo -= amount;
                if(m_TotalPistolAmmo < 0)
                {
                    m_TotalPistolAmmo = 0;
                }
                break;
            default:
                Debug.LogError("ConsumeAmmo: Unknown weapon model");
                break;
        }
    }

    public void SetTotalAmmoAmount(WeaponModel weapon,int amount)
    {
        if(amount < 0)
        {
           amount = 0;
        }
        switch (weapon)
        {
            case WeaponModel.AK74:
                m_TotalRifleAmmo = amount;
                break;
            case WeaponModel.M1911:
                m_TotalPistolAmmo = amount;
                break;
            default:
                Debug.LogError("ConsumeAmmo: Unknown weapon model");
                break;
        }
    }


    #region Throwable Management
    public void PickUpThrowable(GameObject gameObject)
    {
        bool destroy = false;
        Throwable throwable = gameObject.GetComponent<Throwable>();
        if(throwable == null)
        {
            Debug.LogError("PickUpThrowable: throwable is null");
            return;
        }
        switch(throwable.m_type)
        {
            case ThrowableType.Grenade:
               destroy = PickUpThrowableLethal(ThrowableType.Grenade);
                break;
            case ThrowableType.Smoke_Grenade:
                destroy = PickUpThrowableTactical(ThrowableType.Smoke_Grenade);
                break;
            default:
                Debug.LogError("PickUpThrowable: Unknown throwable type");
                break;
        }
        if(destroy)
        {
            //RefillManager.m_instance.RegisterRefillObject(gameObject);
            Destroy(gameObject);
        }
    }

    private bool PickUpThrowableTactical(ThrowableType tacticalThrowable)
    {
        if (tacticalThrowable == m_equipedTacticalType || m_equipedTacticalType == ThrowableType.None)
        {
            m_equipedTacticalType = tacticalThrowable;
            if (m_tacticalCount < m_tacticalCountMax)
            {
                m_tacticalCount += 1;
                HUDManager.m_instance.UpdateThrowables();
                return true;
            }
            else
            {
                Debug.Log("Cannot carry more than 2 tactical throwables");
                return false;
            }
        }
        else
        {
            Debug.Log("Cannot pick up different type of tactical throwable");
            return false;
        }
    }

    private bool PickUpThrowableLethal(ThrowableType lethalThrowable)
    {
        if(lethalThrowable == m_equipedLethalType || m_equipedLethalType == ThrowableType.None)
        {
            m_equipedLethalType = lethalThrowable;
            if (m_lethalCount < m_lethalCountMax)
            {
                m_lethalCount += 1;
                HUDManager.m_instance.UpdateThrowables();
                return true;
            }
            else
            {
                Debug.Log("Cannot carry more than 2 lethal throwables");
                return false;
            }
        }
        else
        {
            Debug.Log("Cannot pick up different type of lethal throwable");
            return false;
        }
    }

 
    public int GetLethalCount()
    {
        return m_lethalCount;
    }

    public int GetTacticalCount()
    {
        return m_tacticalCount;
    }

    public void ThrowLethal ()
    {
        GameObject lethalPrefab = GetThrowablePrefab(m_equipedLethalType);
        GameObject lethalInstance = Instantiate(lethalPrefab, m_throwableSpawnPoint.transform.position, m_throwableSpawnPoint.transform.rotation);
        Rigidbody rb = lethalInstance.GetComponent<Rigidbody>();
        rb.AddForce(m_throwableSpawnPoint.transform.forward * (m_lethalThrowForce * m_forceMultiplier), ForceMode.Impulse);
        lethalInstance.GetComponent<Throwable>().Throw();
        m_lethalCount -= 1;
        if(m_lethalCount <= 0)
        {
            m_equipedLethalType = ThrowableType.None;
            m_lethalCount = 0;
        }
        HUDManager.m_instance.UpdateThrowables();
    }

    public void ThrowTactical()
    {
        GameObject tacticalPrefab = GetThrowablePrefab(m_equipedTacticalType);
        GameObject tacticalInstance = Instantiate(tacticalPrefab, m_throwableSpawnPoint.transform.position, m_throwableSpawnPoint.transform.rotation);
        Rigidbody rb = tacticalInstance.GetComponent<Rigidbody>();
        rb.AddForce(m_throwableSpawnPoint.transform.forward * (m_tacticalThrowForce * m_forceMultiplier), ForceMode.Impulse);
        tacticalInstance.GetComponent<Throwable>().Throw();
        m_tacticalCount -= 1;
        if (m_tacticalCount <= 0)
        {
            m_equipedTacticalType = ThrowableType.None;
            m_tacticalCount = 0;
        }
        HUDManager.m_instance.UpdateThrowables();
    }

    private GameObject GetThrowablePrefab(ThrowableType throwableType)
    {
        switch(throwableType)
        {
            case ThrowableType.Grenade:
                return m_grenadePrefab;
            case ThrowableType.Smoke_Grenade:
                return m_smokeGrenadePrefab;
            default:
                Debug.LogError("GetThrowablePrefab: Unknown throwable type");
                return null;
        }
    }

    public void ResetThrowableThrowForce()
    {
        m_forceMultiplier=0f;
    }

    public void ChargeThrowableThrowForce()
    {
       
        if (m_forceMultiplier >= m_maxforceMultiplier)
        {
            m_forceMultiplier = m_maxforceMultiplier;
            return;
        }
        //Debug.Log($"Throw Multyplier {m_forceMultiplier}\n");
        m_forceMultiplier+=Time.deltaTime;
    }

    #endregion
}
