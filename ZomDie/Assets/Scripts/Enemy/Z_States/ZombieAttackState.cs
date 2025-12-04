using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAttackState : StateMachineBehaviour
{
    [SerializeField]
    private float m_stopAttackingDistance = 2.5f;

    private Transform m_player;
    private NavMeshAgent m_agent;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_player = GameObject.FindGameObjectWithTag("Player").transform;
        m_agent = animator.GetComponent<NavMeshAgent>();
        m_agent.updateRotation = false;



    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (m_player == null) return;
        if (m_agent == null) return;
        //LookAtPlayer();

        float distanceFromPlayer = Vector3.Distance(m_player.position, animator.transform.position);
        if (distanceFromPlayer > m_stopAttackingDistance)
        {
            animator.SetBool("isAttacking", false);
        }

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (m_agent != null)
        {
            m_agent.updateRotation = true;
        }

    }

    private void LookAtPlayer()
    {
        Vector3 direction = (m_player.position - m_agent.transform.position).normalized;
        m_agent.transform.rotation =Quaternion.LookRotation(direction);

        var yRotation = m_agent.transform.rotation.eulerAngles.y;
        m_agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }


}
