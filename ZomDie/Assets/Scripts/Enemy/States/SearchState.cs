using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : BaseState
{
    /// <summary>
    /// How long the enmy has been searching for the player
    /// </summary>
    private float m_searchTimer = 0f;

    private float m_searchTime = 5f;

    private float m_randomSearchRadius = 10f;

    private float m_timeToNextRandomPoint = 0f;

    private float m_randomPointInterval = 2f;
    public override void Enter()
    {
      m_Enemy.GetNavMeshAgent().SetDestination(m_Enemy.GetLastPositionOfPlayer());
    }

    public override void Perform()
    {
        if(m_Enemy.CanSeePlayer())
        {
            //Change to attack state
            m_stateMachine.ChangeState(new AttackState());
        }
        else
        {
            m_searchTimer += Time.deltaTime;
            m_timeToNextRandomPoint += Time.deltaTime;
            //If the enemy has reached the last known position of the player
            if (!m_Enemy.GetNavMeshAgent().pathPending && m_Enemy.GetNavMeshAgent().remainingDistance <= m_Enemy.GetNavMeshAgent().stoppingDistance)
            {
                if(m_timeToNextRandomPoint > m_randomPointInterval)
                {
                    m_Enemy.GetNavMeshAgent().SetDestination(m_Enemy.transform.position + (Random.insideUnitSphere * m_randomSearchRadius));
                    m_timeToNextRandomPoint = 0f;
                }
               
                //After 5 seconds of searching, go back to patrolling
                if (m_searchTimer > m_searchTime)
                {
                    m_stateMachine.ChangeState(new PatrolState());
                }
            }
        }
    }

    public override void Exit()
    {
        m_searchTimer = 0f;
    }


}
