using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{

    public BaseState m_currentState;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_currentState !=null)
        {
            m_currentState.Perform();
        }
    }
    /// <summary>
    /// Sets up default state.
    /// </summary>
    public void Initialize()
    {
       
        ChangeState(new PatrolState());
    }

    public void ChangeState(BaseState newState)
    {
        if (m_currentState != null)
        {
            m_currentState.Exit();
        }
        m_currentState = newState;
        if (m_currentState != null)
        {
            m_currentState.m_stateMachine = this;
            m_currentState.m_Enemy = GetComponent<Enemy>();
            // Cancel any existing NavMeshAgent path so previous movement doesn't continue.
            var agent = m_currentState.m_Enemy?.GetNavMeshAgent();
            if (agent != null)
            {
                agent.ResetPath(); // clear any existing path
                agent.isStopped = false; // ensure agent can accept new destinations in Enter()
            }
            m_currentState.Enter();
        }
    }

  
}
