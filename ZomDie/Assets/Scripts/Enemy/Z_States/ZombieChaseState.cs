using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieChaseState : StateMachineBehaviour
{
    [SerializeField]
    private float m_chasingSpeed = 6f;

    [SerializeField]
    private float m_stopChaseDistance = 21f;
    [SerializeField]
    private float m_attackDistance = 2f;


    private Transform m_player;
    private NavMeshAgent m_agent;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_player = GameObject.FindGameObjectWithTag("Player").transform;
        m_agent = animator.GetComponent<NavMeshAgent>();

        m_agent.speed = m_chasingSpeed;

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_agent.SetDestination(m_player.position);
        animator.transform.LookAt(m_player);

        float distanceFromPlayer = Vector3.Distance(m_player.position, animator.transform.position);
        if (distanceFromPlayer > m_stopChaseDistance)
        {
            animator.SetBool("isChasing", false);
        }
        // To attack state
        if (distanceFromPlayer < m_attackDistance)
        {
            animator.SetBool("isAttacking", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_agent.SetDestination(animator.transform.position);
    }
}
