using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombiePatrolingState : StateMachineBehaviour
{ 
    [SerializeField]
    private float m_patrolingTime = 10f;
    [SerializeField]
    private float m_patrolSpeed = 2f;

    [SerializeField]
    private float m_detectionRadius = 18f;


    private float m_timer;
    private Transform m_player;
    private NavMeshAgent m_agent;

    List<Transform> m_patrolPoints = new List<Transform>();


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       m_player = GameObject.FindGameObjectWithTag("Player").transform;
       m_agent = animator.GetComponent<NavMeshAgent>();    

        m_agent.speed = m_patrolSpeed;
        m_timer = 0f;

        GameObject waypointCluster = GameObject.FindGameObjectWithTag("Waypoints");
        foreach(Transform waypoint in waypointCluster.transform)
        {
            m_patrolPoints.Add(waypoint);
        }
        if(m_patrolPoints.Count > 0)
        {
            int randomIndex = Random.Range(0, m_patrolPoints.Count);
            m_agent.SetDestination(m_patrolPoints[randomIndex].position);
        }

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(SoundManager.m_instance != null && !SoundManager.m_instance.m_zombieChannel.isPlaying)
        {
            SoundManager.m_instance.m_zombieChannel.clip = SoundManager.m_instance.m_zombieWalkSound;
            SoundManager.m_instance.m_zombieChannel.PlayDelayed(5f);
        }
        //Patroling behavior
        if (m_agent.remainingDistance <= m_agent.stoppingDistance)
        {
            int randomIndex = Random.Range(0, m_patrolPoints.Count);
            m_agent.SetDestination(m_patrolPoints[randomIndex].position);
        }
        m_timer += Time.deltaTime;
        if(m_timer > m_patrolingTime)
        {
            animator.SetBool("isPatroling", false);
        }

        //To chase state
        float distanceFromPlayer = Vector3.Distance(m_player.position, animator.transform.position);
        if (distanceFromPlayer < m_detectionRadius)
        {
            animator.SetBool("isChasing", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Stop Agent
        m_agent.SetDestination(animator.transform.position);
        SoundManager.m_instance.m_zombieChannel.Stop();
    }
}
