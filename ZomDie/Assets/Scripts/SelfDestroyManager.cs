using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroyManager : MonoBehaviour
{
    public float m_timeToSelfDestruct = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroSelf(m_timeToSelfDestruct));
    }

    private IEnumerator DestroSelf(float m_timeForDestruction)
    {
        yield return new WaitForSeconds(m_timeForDestruction);
        Destroy(gameObject);
    }
}
