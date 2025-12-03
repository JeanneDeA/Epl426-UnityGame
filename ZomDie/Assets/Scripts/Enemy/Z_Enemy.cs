using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Z_Enemy : MonoBehaviour
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

    public void TakeDamage(int damage,Vector3 hitDirection)
    {
        m_health -= damage;
        if (m_health <= 0)
        {

            Die(hitDirection);
        }
        else
        {
            m_animator.SetTrigger("DAMAGE");
        }
        Debug.Log($"Z_Enemy took {damage} damage, remaining health: {m_health}");
    }

    private void Die(Vector3 hitDirection)
    {

        // Get the direction the zombie is facing
        Vector3 forward = transform.forward;

        // Normalize hit direction
        hitDirection.y = 0; // ignore vertical
        hitDirection.Normalize();

        // Dot product tells us which side the hit came from
        float dot = Vector3.Dot(forward, hitDirection);

        if (dot > 0)
        {
            // Hit came from the front → fall backwards
            m_animator.SetTrigger("DIE_B");
        }
        else
        {
            // Hit came from the back → fall forward
            m_animator.SetTrigger("DIE_F");
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.5f); // Attack range
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 18f); // Detection range
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 21f); // Stop chase range

    }
}
