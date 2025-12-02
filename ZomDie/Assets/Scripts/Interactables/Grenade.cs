using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Interactable
{
    protected override void Interact()
    {
       WeaponManager.m_instance.PickUpThrowable(gameObject);
       
    }
}
