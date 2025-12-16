using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieSpawController : MonoBehaviour
{
    [SerializeField]
    private int m_intialZombiesPerWave = 5;
    [SerializeField]
    private int m_currentWave = 1;
    [SerializeField]
    private float m_timeBetweenWaves = 20f;
    [SerializeField]
    private int m_ReffillWave= 2;
    [SerializeField]
    private int m_currentZombiesPerWave;
    [SerializeField]
    private float m_zombieSpawnDelay = 0.5f;// the delay between each zombie spawn in a wave
    private bool m_inCooldown = false;
    public float m_cooldownTimer = 10f;
    private List<Z_Enemy> m_currentZombiesAlive = new List<Z_Enemy>();

    public GameObject m_zombiePrefab;

    public TextMeshProUGUI m_waveOverUI;

    public TextMeshProUGUI m_waveCounterUI;

    public TextMeshProUGUI m_cooldownTimerUI;

    public List<GameObject> m_SpawnLocations;
    private void Start()
    {

        m_currentZombiesPerWave = m_intialZombiesPerWave;
        GlobalReferences.m_instance.m_waveNumber = 0;

        StartNextWave();

    }

    private void StartNextWave()
    {
       
       m_currentZombiesAlive.Clear();
       m_currentWave++;
        if (m_currentWave  % m_ReffillWave == 0)
        {
            RefillManager.m_instance.RefillAll();
        }
        GlobalReferences.m_instance.m_waveNumber = m_currentWave;
       StartCoroutine(SpawnZombiesInWave());
    }

    private IEnumerator SpawnZombiesInWave()
    {
        for(int i = 0; i < m_currentZombiesPerWave; i++)
        {
            //Generate a zombie at a random spawn point
            Vector3 spawnOffset = new Vector3(UnityEngine.Random.Range(-1, 1), 0, UnityEngine.Random.Range(-1, 1));
            int selectedLocationIndex = UnityEngine.Random.Range(0, m_SpawnLocations.Count);
            Vector3 spawnPosition = m_SpawnLocations[selectedLocationIndex].transform.position + spawnOffset;
            var zombie = Instantiate(m_zombiePrefab, spawnPosition, Quaternion.identity);
            Z_Enemy zombieComponent = zombie.GetComponent<Z_Enemy>();
            zombieComponent.m_chaseZombie = true; 
            m_currentZombiesAlive.Add(zombieComponent);
            yield return new WaitForSeconds(m_zombieSpawnDelay);
        }   
    }

    private void Update()
    {
        //Get all dead zombies and remove them from the list
        List<Z_Enemy> deadZombies = new List<Z_Enemy>();
        foreach(Z_Enemy zombie in m_currentZombiesAlive)
        {
            if(zombie.IsDead())
            {
                deadZombies.Add(zombie);
            }
        }
        foreach(Z_Enemy deadZombie in deadZombies)
        {
            m_currentZombiesAlive.Remove(deadZombie);
        }
        deadZombies.Clear();

        if(m_currentZombiesAlive.Count == 0 && !m_inCooldown)
        {
            
            m_inCooldown = true;
            m_cooldownTimer = m_timeBetweenWaves;
            m_cooldownTimerUI.gameObject.SetActive(true);
            m_waveOverUI.gameObject.SetActive(true);
            m_cooldownTimerUI.text = "Next Wave In: " + m_cooldownTimer.ToString("F1") + "s";
            m_waveCounterUI.text = $"Wave: {m_currentWave + 1}";
            StartCoroutine(WaveCooldown());
        }
        if(m_inCooldown)
        {
            m_cooldownTimer -= Time.deltaTime;
            m_cooldownTimerUI.text = "Next Wave In: " + m_cooldownTimer.ToString("F1") + "s";
        }
        else
        {
            // Reset the cooldown timer
            m_cooldownTimer = m_timeBetweenWaves;
            m_cooldownTimerUI.gameObject.SetActive(false);
            m_waveOverUI.gameObject.SetActive(false);
        }
    }

    private IEnumerator WaveCooldown()
    {
       m_inCooldown = true;
       yield return new WaitForSeconds(m_timeBetweenWaves);
       m_inCooldown = false;
       m_currentZombiesPerWave *= 2; // Double the number of zombies for the next wave
       StartNextWave();
    }

    public int GetCurrentWave()
    {
        return m_currentWave;
    }
}