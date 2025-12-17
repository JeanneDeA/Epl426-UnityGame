using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class PlayerGame : MonoBehaviour
{

    private PlayerHealth m_healthComponent;
    
    private void Awake()
    {
        m_healthComponent = GetComponent<PlayerHealth>();
        SoundManager.m_instance.PlayBackgroundMusic();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZombieHand"))
        {
            if (!m_healthComponent.IsDead())
            {
               // Debug.Log("Player hit by zombie hand!");
                m_healthComponent.TakeDamage(10);
            }
          
        }
    }

}
