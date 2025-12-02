using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : Interactable
{
    [SerializeField]
    private int m_ammoAmount = 30;

    [SerializeField]
    public AmmoType m_ammoType = AmmoType.Rifle_Ammo;

    protected override void Interact()
    {
        WeaponManager.m_instance.PickUpAmmo(gameObject.GetComponent<AmmoBox>());
        Destroy(gameObject);
    }

    public int GetAmmoAmount()
    {
        return m_ammoAmount;
    }
}
