using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : BaseState
{
    /// <summary>
    /// Track what waypoint we are currently moving towards
    /// </summary>
    private int m_waypointIndex { get; set; }

    private float m_waitTimer = 0f; 

    private float m_waitDuration = 2f;
    public override void Enter()
    {
       
    }

    public override void Exit()
    {
        
    }

    public override void Perform()
    {
        PatrolCycle();
        if(m_Enemy.CanSeePlayer())
        {
            m_stateMachine.ChangeState(new AttackState());
        }
    }

    /// <summary>
    /// This will make the enemy patrol between the waypoints defined in the EnemyPath component
    /// </summary>
    public void PatrolCycle()
    {
        NavMeshAgent agent = m_Enemy.GetNavMeshAgent();
        if (agent.remainingDistance <0.2f)
        {
            m_waitTimer += Time.deltaTime;  
            if(m_waitTimer < m_waitDuration)
            {
                return;
            }
            if (m_waypointIndex <m_Enemy.GetEnemyPath().GetWaypoints().Count -1)
            {
                m_waypointIndex++;
            }
            else
            {
                m_waypointIndex = 0;        
            }
            m_waitTimer = 0f;
            agent.SetDestination(m_Enemy.GetEnemyPath().GetWaypoints()[m_waypointIndex].position);
        }
    }
}
