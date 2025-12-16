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
        WeaponManager.m_instance.PickUpAmmo(gameObject.GetComponentInChildren<AmmoBox>());
        //Debug.Log($"RefillManager={RefillManager.m_instance}, GlobalRefs={GlobalReferences.m_instance}, Prefab={(GlobalReferences.m_instance ? GlobalReferences.m_instance.m_ammoBoxPrefab : null)}");
        RefillManager.m_instance.RegisterAmmoBox(GlobalReferences.m_instance.m_ammoBoxPrefab, transform.parent,this);
        Destroy(transform.parent.gameObject);
    }

    public int GetAmmoAmount()
    {
        return m_ammoAmount;
    }

    public void SetAmmoAmount(int amount)
    {
        m_ammoAmount = amount;
    }
}
