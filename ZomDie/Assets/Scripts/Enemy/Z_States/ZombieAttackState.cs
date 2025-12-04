using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAttackState : StateMachineBehaviour
{
    [SerializeField]
    private float m_stopAttackingDistance = 2.5f;

    [SerializeField] private Vector2 m_attackSoundDelayRange = new Vector2(0.3f, 1.0f);
    private float m_nextAttackSoundTime;

    private Transform m_player;
    private NavMeshAgent m_agent;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_player = GameObject.FindGameObjectWithTag("Player").transform;
        m_agent = animator.GetComponent<NavMeshAgent>();
        m_agent.updateRotation = false;

        // first random delay
    m_nextAttackSoundTime = Time.time + Random.Range(m_attackSoundDelayRange.x, m_attackSoundDelayRange.y);

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if (SoundManager.m_instance != null && !SoundManager.m_instance.m_zombieChannel.isPlaying)
        //{
        //    SoundManager.m_instance.m_zombieChannel.clip = SoundManager.m_instance.m_zombieAttackSound;
        //    SoundManager.m_instance.m_zombieChannel.PlayDelayed(1f);
        //}

        if (SoundManager.m_instance != null && !SoundManager.m_instance.m_zombieChannel.isPlaying)
        {
            //SoundManager.m_instance.m_zombieChannel.clip = SoundManager.m_instance.m_zombieWalkSound;
            SoundManager.m_instance.m_zombieChannel.PlayOneShot(SoundManager.m_instance.m_zombieAttackSound);
        }

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
        SoundManager.m_instance.m_zombieChannel.Stop();

    }

    private void LookAtPlayer()
    {
        Vector3 direction = (m_player.position - m_agent.transform.position).normalized;
        m_agent.transform.rotation =Quaternion.LookRotation(direction);

        var yRotation = m_agent.transform.rotation.eulerAngles.y;
        m_agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }


}
