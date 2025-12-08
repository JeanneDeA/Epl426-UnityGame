using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class SaveLoadManagment : MonoBehaviour
{
    public static SaveLoadManagment m_instance { get; private set; }

    private readonly string m_highScoreKey = "BestWaveSavedValue";

    private void Awake()
    {
        if (m_instance != null && m_instance != this)
        {
            Destroy(gameObject);

        }
        else
        {
            m_instance = this;
        }

        DontDestroyOnLoad(this);
    }

    public void SaveHighScore(int wave)
    {
        PlayerPrefs.SetInt(m_highScoreKey, wave);
    }

    public int LoadHighScore()
    {
        return PlayerPrefs.GetInt(m_highScoreKey, 0);
    }
}
