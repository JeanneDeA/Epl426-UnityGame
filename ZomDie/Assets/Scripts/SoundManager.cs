using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager m_instance { get; private set; }

    public AudioSource ShootingChannel;
    public AudioSource m_reloadingSound1911; 
    public AudioSource m_emptyMagazineSound1911;
    public AudioSource m_reloadingSoundAK74;
    public AudioSource m_throwablesChanel;
    public AudioSource m_zombieChannel;
    public AudioSource m_zombieChannel2;
    public AudioClip m_shotAK74;
    public AudioClip m_shotM1911;
    public AudioClip m_grenadeExplosion;
    public AudioClip m_smokeGrenadeSound;

    public AudioClip m_zombieWalkSound;
    public AudioClip m_zombieChaseSound;
    public AudioClip m_zombieAttackSound;
    public AudioClip m_zombieDieSound;
    public AudioClip m_zombieHurtSound;

    

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
    }

    public void PlayShootingSound(WeaponModel weapon)
    {
       switch(weapon)
        {
            case WeaponModel.M1911:
                ShootingChannel.PlayOneShot(m_shotM1911);
                break;
            case WeaponModel.AK74:
                ShootingChannel.PlayOneShot(m_shotAK74);
                break;
            default:
                break;
        }
    }

    public void PlayEmptyMagazineSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.M1911:
                m_emptyMagazineSound1911.Play();
                break;
            case WeaponModel.AK74:
                m_emptyMagazineSound1911.Play();
                break;
            default:
                break;
        }
    }

    public void PlayReloadingSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.M1911:
                m_reloadingSound1911.Play();
                break;
            case WeaponModel.AK74:
                m_reloadingSoundAK74.Play();
                break;
            default:
                break;
        }
    }
}

