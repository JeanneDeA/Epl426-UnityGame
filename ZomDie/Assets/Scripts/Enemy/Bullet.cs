using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private int m_damage = 0;


    public void SetDamage(int damage)
    {
       m_damage = damage;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="objectHit"></param>
    private void OnCollisionEnter(Collision objectHit)
    {
        Transform hitTransform = objectHit.transform;
        if (hitTransform.CompareTag("Player"))
        {
            Debug.Log("Hit Player");
            hitTransform.GetComponent<PlayerHealth>().TakeDamage(10);
        }
        else if(hitTransform.CompareTag("Zombie"))
        {
            Debug.Log("Hit Enemy");
            hitTransform.GetComponent<Zombie>().TakeDamage(m_damage);
            Destroy(gameObject);
        }
        else if(hitTransform.CompareTag("Bottle"))
        {
            hitTransform.GetComponent<Bottle>().Explode();
        }
        else 
        {
            CreateBulletImpactEffect(objectHit);
        }
        Destroy(gameObject);
    }

   private void CreateBulletImpactEffect(Collision objectHit)
   {
        ContactPoint contanct = objectHit.contacts[0];
        GameObject hole = Instantiate(GlobalReferences.m_instance.m_bulletImpactStoneEffectPrefab, contanct.point, Quaternion.LookRotation(contanct.normal));
        hole.transform.SetParent(objectHit.gameObject.transform);
   }

}
