using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalReferences : MonoBehaviour
{
    public static GlobalReferences m_instance { get; private set; }

    public GameObject m_bulletImpactStoneEffectPrefab;

    public GameObject m_grenadeExposionEffect;

    public GameObject m_smokeGrenadeEffect;

    public GameObject m_bloodSplatterEffectPrefab;

    public int m_waveNumber = 0;
    /// <summary>
    /// Make sure there is only one instance of GlobalReferences
    /// </summary>
    private void Awake()
    {
        if (m_instance != null  && m_instance != this)
        {
            Destroy(gameObject);

        }
        else
        {
            m_instance = this;
        }
    }
}
