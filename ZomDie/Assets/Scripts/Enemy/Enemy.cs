using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
   // [Header("Debugging")]
   // public GameObject m_debugSphere;

    private StateMachine m_stateMachine;

    [SerializeField]
    private EnemyPath m_Path;
    /// <summary>
    /// This is the Navmesh the Enemy will use to navigate the scene
    /// </summary>
    private NavMeshAgent m_navMeshAgent;

    private GameObject m_player;

    private Vector3 m_LastPositionofPlayer;

    private Animator m_animator;

    [SerializeField]
    private float m_detectionRange = 20f;
    [SerializeField]
    private float m_fieldOfView = 85f;
    [SerializeField]
    private float m_eyeHeight = 2f;
    [SerializeField]
    private float m_waitTimeAfterLosingPlayer = 1f;

    [Header("Weapon Values")]
    [SerializeField]
    private Transform m_gunBarrel;
    [SerializeField]
    private GameObject m_muzzleFlashEffect;
    [SerializeField]
    [Range(0.1f, 10f)]


    private float m_fireRate = 5f;

    /// <summary>
    /// Only for debugging purposes, shows the current state name in the inspector
    /// </summary>
    [SerializeField]
    private string m_currentStateName;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_stateMachine = GetComponent<StateMachine>();
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_stateMachine.Initialize();
        m_player = GameObject.FindGameObjectWithTag("Player");
    }


    // Update is called once per frame
    void Update()
    {
        CanSeePlayer();
        m_currentStateName = m_stateMachine.m_currentState.GetType().Name;
        //m_debugSphere.transform.position = m_LastPositionofPlayer;
    }


    public NavMeshAgent GetNavMeshAgent()
    {
        return m_navMeshAgent;
    }

    public Vector3 GetLastPositionOfPlayer()
    {
        return m_LastPositionofPlayer;
    }

    public void SetLastPositionOfPlayer(Vector3 position)
    {
        m_LastPositionofPlayer = position;
    }

    public EnemyPath GetEnemyPath()
    {
        return m_Path;
    }

    public float GetWaitTimeAfterLosingPlayer()
    {
        return m_waitTimeAfterLosingPlayer;
    }

    public float GetFireRate()
    {
        return m_fireRate;
    }

    public GameObject GetPlayer()
    {
        return m_player;
    }

    public Transform GetGunBarrel()
    {
        return m_gunBarrel;
    }
    public GameObject GetMuzzleFlashEffect()
    {
        return m_muzzleFlashEffect;
    }
    public Animator GetAnimator()
    {
        return m_animator;
    }

    public bool CanSeePlayer()
    {
        if (m_player == null)
        {
            return false;
        }
        if(Vector3.Distance(transform.position, m_player.transform.position) > m_detectionRange)
        {
            return false;
        }
        Vector3 directionToPlayer = m_player.transform.position - transform.position - (Vector3.up *m_eyeHeight);
        float angleBetweenEnemyAndPlayer = Vector3.Angle(directionToPlayer,transform.forward);
        if(angleBetweenEnemyAndPlayer >= -m_fieldOfView && angleBetweenEnemyAndPlayer <=m_fieldOfView)
        {

            Ray ray = new Ray(transform.position + (Vector3.up * m_eyeHeight), directionToPlayer);
            RaycastHit hitInfo = new RaycastHit();
            if(Physics.Raycast(ray,out hitInfo, m_detectionRange))
            {
                if(hitInfo.collider.gameObject == m_player)
                {
                    Debug.DrawRay(ray.origin, ray.direction * m_detectionRange, Color.red);
                    return true; 
                }
            }
           
        }
        return false;

    }


}