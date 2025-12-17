using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAudio : MonoBehaviour
{
    [SerializeField] 
    private AudioSource m_source;

    private void Awake()
    {
        if (m_source == null)
        {
            m_source = GetComponent<AudioSource>();
        }
       
    }

    private void Reset()
    {
        m_source = GetComponent<AudioSource>();
    }

    public void PlayChaseLoop(AudioClip chaseClip)
    {
        Debug.Log("Entered Play chase loop");
        if (m_source.clip != chaseClip)
        {
            m_source.clip = chaseClip;
            m_source.loop = true;
            m_source.Play();
            Debug.Log("Playing chase loop");
        }
        else if (!m_source.isPlaying)
        {
            m_source.Play();
        }
    }

    public void StopLoop()
    {
        if (m_source.isPlaying) m_source.Stop();
    }
}
