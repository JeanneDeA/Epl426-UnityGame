using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGame : MonoBehaviour
{

    private PlayerHealth m_healthComponent;
    
    private void Awake()
    {
        m_healthComponent = GetComponent<PlayerHealth>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZombieHand"))
        {
            Debug.Log("Player hit by zombie hand!");
            m_healthComponent.TakeDamage(10);
        }
    }

}
