using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ZombieIdleState : StateMachineBehaviour
{
    [SerializeField]
    private float m_idleDuration = 0.0f;

    [SerializeField]
    private float m_detectionRadius = 18f;

    [HideInInspector]
    public bool m_chaseZombie = false;

    private float m_timer;
    private Transform m_player;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_timer = 0;
        m_player = GameObject.FindGameObjectWithTag("Player").transform;
        if(m_chaseZombie)
        {
            m_detectionRadius = 1000f;
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if(m_player.GetComponent<PlayerHealth>().IsDead())
        {
            animator.SetBool("isChasing", false);
            animator.SetBool("isPatroling", false);
            return;
        }
        // To patrol state
        m_timer += Time.deltaTime;
        if(m_timer > m_idleDuration)
        {
          animator.SetBool("isPatroling", true);
        }

        //To chase state
        float distanceFromPlayer = Vector3.Distance(m_player.position, animator.transform.position);
        if(distanceFromPlayer < m_detectionRadius)
        {
            animator.SetBool("isChasing", true);
        }


    }


}
