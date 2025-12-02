using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{

    private float m_moveTimer;

    private float m_losePlayerTimer;

    private float m_shootTimer;
    [SerializeField]
   private float m_bulletSpeed=40f;
    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void Perform()
    {
        if (m_Enemy.CanSeePlayer())
        {
            m_losePlayerTimer = 0f;
            m_moveTimer += Time.deltaTime;
            m_shootTimer += Time.deltaTime;
            m_Enemy.transform.LookAt(m_Enemy.GetPlayer().transform.position);
            if (m_shootTimer > m_Enemy.GetFireRate())
            {
                Shoot();
            }
            if (m_moveTimer > Random.Range(3, 7))
            {
                m_moveTimer = 0f;
                m_Enemy.GetNavMeshAgent().SetDestination(m_Enemy.transform.position + (Random.insideUnitSphere * 5));

            }
            m_Enemy.SetLastPositionOfPlayer(m_Enemy.GetPlayer().transform.position);
        }
        else
        {
            m_losePlayerTimer += Time.deltaTime;
            if (m_losePlayerTimer > m_Enemy.GetWaitTimeAfterLosingPlayer())
            {
                //Change to search state
                m_stateMachine.ChangeState(new SearchState());
            }
        }

    }
    public void Shoot()
    {
        //store a reference to the muzzle flash effect
        GameObject muzzleFlashEffect = m_Enemy.GetMuzzleFlashEffect();
        //store a reference to the gun barell
        Transform gunBarrel = m_Enemy.GetGunBarrel();
        //instantiate a bullet at the gun barrel position
        GameObject bullet = GameObject.Instantiate(Resources.Load("Prefabs/BulletV2") as GameObject, gunBarrel.position, m_Enemy.transform.rotation);

        //instantiate muzzle flash effect
        muzzleFlashEffect.GetComponent<ParticleSystem>().Play();

        //calculate direction to player
        Vector3 shootDirection = (m_Enemy.GetPlayer().transform.position - gunBarrel.transform.position).normalized;
        //set bullet velocity
        bullet.GetComponent<Rigidbody>().velocity = Quaternion.AngleAxis(Random.Range(-3f, 3f), Vector3.up) * shootDirection * m_bulletSpeed;
        m_shootTimer = 0f;

    }
}

