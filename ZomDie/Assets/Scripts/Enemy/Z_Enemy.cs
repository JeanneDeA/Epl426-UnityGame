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


    private Transform m_player;

    [SerializeField]
    private Transform m_handHitbox_R;    
    [SerializeField]
    private Transform m_handHitbox_L;     

    private bool m_isDead = false;

    public bool m_chaseZombie = false;

    private void Start()
    {
        if (m_chaseZombie)
        {
            ChangeZombieBehaviourToChase();
        }
    }

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_player = GameObject.FindGameObjectWithTag("Player").transform;

        Transform handBone_R = m_animator.GetBoneTransform(HumanBodyBones.RightHand);
        Transform handBone_L = m_animator.GetBoneTransform(HumanBodyBones.LeftHand);
        if (handBone_R == null || handBone_L == null)
        {
            Debug.LogError(" hand bone not found on animator!");
            return;
        }
        // Parent the hitbox to the hand bone
        m_handHitbox_R.SetParent(handBone_R, false);
        m_handHitbox_L.SetParent(handBone_L, false);
        // Offset relative to the bone
        m_handHitbox_R.localPosition = new Vector3(-0.112f, 0f, 0f);
        m_handHitbox_L.localPosition = new Vector3(-0.112f, 0f, 0f);

        // Keep the same rotation as the bone
        m_handHitbox_R.localRotation = Quaternion.identity;
        m_handHitbox_L.localRotation = Quaternion.identity;
    }

    public void TakeDamage(int damage,Vector3 hitDirection)
    {
        m_health -= damage;
        if (m_health <= 0)
        {
            
            Debug.Log("Z_Enemy Died");
            Die(hitDirection);
            m_isDead = true;
        }
        else
        {
            m_animator.SetTrigger("DAMAGE");
            SoundManager.m_instance.m_zombieChannel2.PlayOneShot(SoundManager.m_instance.m_zombieHurtSound);
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
        SoundManager.m_instance.m_zombieChannel2.PlayOneShot(SoundManager.m_instance.m_zombieDieSound);
    }

    void LateUpdate()
    {
        if (!m_animator.GetBool("isAttacking"))
        {
            return;
        }
        if (!m_isDead)
        {
          LookAtPlayer();
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

    private void LookAtPlayer()
    {
        Vector3 direction = (m_player.position - m_navMeshAgent.transform.position).normalized;
        m_navMeshAgent.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = m_navMeshAgent.transform.rotation.eulerAngles.y;
        m_navMeshAgent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public bool IsDead()
    {
        return m_isDead;
    }

    private void ChangeZombieBehaviourToChase()
    {
        var animator = GetComponent<Animator>();
        // Get all state scripts of each type
        var idleStates = animator.GetBehaviours<ZombieIdleState>();
        var patrolStates = animator.GetBehaviours<ZombiePatrolingState>();
        var chaseStates = animator.GetBehaviours<ZombieChaseState>();

        // Set the value for all of them
        foreach (var s in idleStates)
        {
            s.m_chaseZombie = true;
        }

        foreach (var s in patrolStates)
        {
            s.m_chaseZombie = true;
        }

        foreach (var s in chaseStates)
        {
            s.m_chaseZombie = true;
        }
           
    }
}
