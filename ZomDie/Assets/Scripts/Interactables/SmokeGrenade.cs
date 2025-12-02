using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeGrenade : Interactable
{
    // Start is called before the first frame update
    protected override void Interact()
    {
        WeaponManager.m_instance.PickUpThrowable(gameObject);

    }
}
