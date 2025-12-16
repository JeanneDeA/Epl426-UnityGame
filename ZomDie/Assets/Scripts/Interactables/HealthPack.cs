using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : Interactable
{

    [SerializeField]
    private int m_healthAmmount = 30;

    private PlayerHealth m_playerHealth;
    private void Awake()
    {
        // Option 1: Find once (safe, simple)
        m_playerHealth = FindObjectOfType<PlayerHealth>();

        if (m_playerHealth == null)
            Debug.LogError("HealthPack: PlayerHealthScript not found!");
    }
    protected override void Interact()
    {
        if(m_playerHealth.GetHealth() >= 100)
        {
            // Player health is full, do not pick up the health pack
            return;
        }
        m_playerHealth.RestoreHealth(m_healthAmmount);
        Transform pickupRoot = transform;
        RefillManager.m_instance.RegisterHealthPack(GlobalReferences.m_instance.m_healthPackPrefab, pickupRoot, m_healthAmmount );
        Destroy(pickupRoot.gameObject); 
    }

    public int GetHealthAmmount()
    {
        return m_healthAmmount;
    }

    public void SetHealthAmmount(int amount)
    {
        m_healthAmmount = amount;
    }
}
