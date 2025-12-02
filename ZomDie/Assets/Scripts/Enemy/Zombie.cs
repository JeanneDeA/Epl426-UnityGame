using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [SerializeField]
    private int m_health = 100;

    private Animator m_animator;

    private NavMeshAgent m_navMeshAgent;

    private void Start()
    {
        m_animator = GetComponentInChildren<Animator>();
        m_navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(int damage)
    {
        m_health -= damage;
        if (m_health <= 0)
        {

            Die();
        }
        else
        {
           // m_animator.SetTrigger("Hurt");
        }
        Debug.Log($"Zombie took {damage} damage, remaining health: {m_health}");
    }

    private void Die()
    {
       //m_animator.SetTrigger("Die");
       Destroy(gameObject); // Delay to allow death animation to play
    }

    private void Update()
    {
       if(m_navMeshAgent.velocity.magnitude > 0.1f)
       {
            m_animator.SetBool("isWalking", true);
       }
       else
       {
            m_animator.SetBool("isWalking", false);
        }
    }
}
