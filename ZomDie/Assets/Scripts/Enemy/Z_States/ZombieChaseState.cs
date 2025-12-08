using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieChaseState : StateMachineBehaviour
{
    [SerializeField]
    private float m_chasingSpeed = 4f;

    [SerializeField]
    private float m_stopChaseDistance = 21f;
    [SerializeField]
    private float m_attackDistance = 2f;

    [HideInInspector]
    public bool m_chaseZombie = false;

    private Transform m_player;
    private NavMeshAgent m_agent;



    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_player = GameObject.FindGameObjectWithTag("Player").transform;
        m_agent = animator.GetComponent<NavMeshAgent>();

        m_agent.speed = m_chasingSpeed;
        if (m_chaseZombie)
        {
            m_stopChaseDistance = 1005f;
        }

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if (SoundManager.m_instance != null && !SoundManager.m_instance.m_zombieChannel.isPlaying)
        //{
        //    SoundManager.m_instance.m_zombieChannel.clip = SoundManager.m_instance.m_zombieChaseSound;
        //    SoundManager.m_instance.m_zombieChannel.PlayDelayed(1f);
        //}
           if(SoundManager.m_instance != null && !SoundManager.m_instance.m_zombieChannel.isPlaying)
        {
            //SoundManager.m_instance.m_zombieChannel.clip = SoundManager.m_instance.m_zombieWalkSound;
            SoundManager.m_instance.m_zombieChannel.PlayOneShot(SoundManager.m_instance.m_zombieChaseSound);
        }

        m_agent.SetDestination(m_player.position);
        animator.transform.LookAt(m_player);

        float distanceFromPlayer = Vector3.Distance(m_player.position, animator.transform.position);

        if (m_player.GetComponent<PlayerHealth>().IsDead())
        {
            animator.SetBool("isChasing", false);
            animator.SetBool("isPatroling", false);

        }
        else if (distanceFromPlayer > m_stopChaseDistance )
        {
            animator.SetBool("isChasing", false);
        }
        // To attack state
        else if (distanceFromPlayer < m_attackDistance)
        {
            animator.SetBool("isAttacking", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_agent.SetDestination(animator.transform.position);
        SoundManager.m_instance.m_zombieChannel.Stop();
    }
}
