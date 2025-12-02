using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class HUDManager : MonoBehaviour
{
    public static HUDManager m_instance { get; private set; }

    [Header("Sights")]
    public Image m_sight;
    [Header("Ammo")]
    public TextMeshProUGUI m_magazineAmmo;
    public TextMeshProUGUI m_totalAmmo;
    public Image m_ammoType;
    [Header("Weapon")]
    public Image m_ActiveWeapon;
    public Image m_UnActiveWeapon;
    public Sprite m_emptySprite;
    [Header("Throwables")]
    public TextMeshProUGUI m_LethalAmount;
    public Image m_LethalUI;
    [Header("Tactical")]
    public TextMeshProUGUI m_tacticalAmount;
    public Image m_tacticalUI;

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
    }

    private void Update()
    {
        PlayerWeapon activeWeapon = WeaponManager.m_instance.m_currentWeaponSlot.GetComponentInChildren<PlayerWeapon>();
        PlayerWeapon unActiveWeapon = WeaponManager.m_instance.GetUnactiveWeaponSlot().GetComponentInChildren<PlayerWeapon>();
        if (activeWeapon != null)
        {
            m_magazineAmmo.text = $"{activeWeapon.GetBulletsLeft() / activeWeapon.GetBulletsPerBurst()}";
            m_totalAmmo.text = $"{WeaponManager.m_instance.GetTotalAmmoAmount(activeWeapon.m_currentWeaponModel)}";

            WeaponModel weaponModel = activeWeapon.m_currentWeaponModel;
            m_ammoType.sprite = GetAmmoSprite(weaponModel);
            m_ActiveWeapon.sprite = GetWeaponSprite(weaponModel);

           
        }
        else
        {
            m_magazineAmmo.text = "0";
            m_totalAmmo.text = "0";
            m_ammoType.sprite = m_emptySprite;
            m_ActiveWeapon.sprite = m_emptySprite;
        }
        if (unActiveWeapon != null)
        {
            WeaponModel unActiveWeaponModel = unActiveWeapon.m_currentWeaponModel;
            m_UnActiveWeapon.sprite = GetWeaponSprite(unActiveWeaponModel);
        }
        else
        {
            m_UnActiveWeapon.sprite = m_emptySprite;
        }

    }

    private Sprite GetAmmoSprite(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.M1911:
                return Resources.Load<GameObject>("UI/Pistol_Ammo").GetComponent<SpriteRenderer>().sprite;

            case WeaponModel.AK74:
                return Resources.Load<GameObject>("UI/Rifle_Ammo").GetComponent<SpriteRenderer>().sprite;

            default:
                return m_emptySprite;
        }
    }

    private Sprite GetWeaponSprite(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.M1911:
                return Resources.Load<GameObject>("UI/M1911").GetComponent<SpriteRenderer>().sprite;

            case WeaponModel.AK74:
                return Resources.Load<GameObject>("UI/AK74").GetComponent<SpriteRenderer>().sprite;

            default:
                return m_emptySprite;
        }
    }

    public void ShowSights(bool show)
    {
        if(show)
        {
            m_sight.enabled = true;
        }
        else
        {
            m_sight.enabled = false;
        }
    }

    internal void UpdateThrowables()
    {
        switch(WeaponManager.m_instance.m_equipedLethalType)
        {
            case ThrowableType.Grenade:
                m_LethalAmount.text = $"{WeaponManager.m_instance.GetLethalCount()}";
                m_LethalUI.sprite = Resources.Load<GameObject>("UI/Grenade_Icon").GetComponent<SpriteRenderer>().sprite;
                break;
            case ThrowableType.None:
                m_LethalAmount.text = "0";
                m_LethalUI.sprite = m_emptySprite;
                break;
            default:
                Debug.LogError("UpdateThrowables: Unknown throwable type");
                break;
        }

        switch(WeaponManager.m_instance.m_equipedTacticalType)
        {
            case ThrowableType.Smoke_Grenade:
                m_tacticalAmount.text = $"{WeaponManager.m_instance.GetTacticalCount()}";
                m_tacticalUI.sprite = Resources.Load<GameObject>("UI/Smoke_Grenade_Icon").GetComponent<SpriteRenderer>().sprite;
                break;
            case ThrowableType.None:
                m_tacticalAmount.text = "0";
                m_tacticalUI.sprite = m_emptySprite;
                break;
            default:
                Debug.LogError("UpdateThrowables: Unknown throwable type");
                break;
        }
    }
}
