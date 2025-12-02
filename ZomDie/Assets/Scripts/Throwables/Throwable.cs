using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField]
    private float m_delay = 5.0f;

    [SerializeField]
    private float m_explosionRadius = 3.0f;

    [SerializeField]
    private float m_explosionForce = 700.0f;

    private float m_countdown;

    private bool m_hasExploded = false;

    private bool m_hasBeenThrown = false;

    public ThrowableType m_type;
    // Start is called before the first frame update
    void Start()
    {
        m_countdown = m_delay;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_hasBeenThrown)
        {
            m_countdown -= Time.deltaTime;
            if(m_countdown <= 0f && !m_hasExploded)
            {
                Explode();
                m_hasExploded = true;
            }
        }
    }

    private void Explode()
    {
        GetThrowableEffect();
        Destroy(gameObject);
    }

    private void GetThrowableEffect()
    {
       switch(m_type)
        {
            case ThrowableType.Grenade:
                GrenadeEffect();
                break;
            case ThrowableType.Smoke_Grenade:
                SmokeGrenadeEffect();
                break;

            default:
                Debug.Log("Unknown Throwable Type!");
                break;
        }
    }

    private void SmokeGrenadeEffect()
    {
        //visual effect
        GameObject smokeEffect = GlobalReferences.m_instance.m_smokeGrenadeEffect;
        Instantiate(smokeEffect, transform.position, transform.rotation);
        //Apply explosion force to nearby objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //Apply blindness effect to players/enemys in range
            }
        }
        //PlaySound
        SoundManager.m_instance.m_throwablesChanel.PlayOneShot(SoundManager.m_instance.m_smokeGrenadeSound);
        //Also apply damage to enemys in range
    }

    private void GrenadeEffect()
    {
        //visual effect
        GameObject exposionEffect = GlobalReferences.m_instance.m_grenadeExposionEffect;
        Instantiate(exposionEffect, transform.position, transform.rotation);
        //Apply explosion force to nearby objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_explosionRadius);
        foreach(Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(m_explosionForce, transform.position, m_explosionRadius);
            }
        }
        //PlaySound
        SoundManager.m_instance.m_throwablesChanel.PlayOneShot(SoundManager.m_instance.m_grenadeExplosion);
        //Also apply damage to enemys in range
    }

    public void Throw()
    {
        m_hasBeenThrown= true;
    }
}
