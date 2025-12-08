using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    private float m_currentHealth;
    private float m_lerpTimer;                  // Timer for lerping health bar
    private float m_maxHealth = 100f;
    private bool m_isDead = false;

    [Header("Health Bar")]
    [SerializeField]
    private float m_chipSpeed = 2f;              // Speed at which the health bar chips away
    [SerializeField]
    private TMP_Text m_healthText;
    [SerializeField]
    private Image m_frontHealthBar;
    [SerializeField]
    private Image m_backHealthBar;

    [Header("Damage Overlay")]
    [SerializeField]
    private Image m_damageOverlay;
    [SerializeField]
    private float m_fadeSpeed = 1.5f;            // Speed at which the damage overlay fades
    [SerializeField]
    private float m_damageOverlayDuration = 1f;  // Duration the damage overlay stays fully visible
    private float m_damageOverlayDurationTimer;// Timer for damage overlay duration

    [Header("UI")]
    [SerializeField]
    private GameObject  m_gameOverUI;


    void Start()
    {
        m_currentHealth = m_maxHealth;
        m_damageOverlay.color = new Color(m_damageOverlay.color.r, m_damageOverlay.color.g, m_damageOverlay.color.b, 0);        

    }

    void Update()
    {
        m_currentHealth = Mathf.Clamp(m_currentHealth, 0, m_maxHealth);     // Clamp health between 0 and max health
        UpdateHealthUI();
        if(m_damageOverlay.color.a > 0)
        {
            if (m_currentHealth < 30)                                       // If health is low, keep the overlay visible
                return;
            m_damageOverlayDurationTimer += Time.deltaTime;                 
            if(m_damageOverlayDurationTimer > m_damageOverlayDuration)
            {    // After duration, start fading
                float tempAlpha = m_damageOverlay.color.a;
                tempAlpha -= Time.deltaTime * m_fadeSpeed;
                m_damageOverlay.color = new Color(m_damageOverlay.color.r, m_damageOverlay.color.g, m_damageOverlay.color.b, tempAlpha);
            }
        }
    }

    /// <summary>
    ///     Updates the health bar UI elements, including the front and back fill bars,
    ///     the health percentage text, and handles the chip-away animation effect 
    ///     when taking or restoring health.
    /// </summary>
    public void UpdateHealthUI()
    {
        //Debug.Log("Helath: " + m_currentHealth);
       // Debug.Log("Timer "+ m_damageOverlayDurationTimer);
        float fillF = m_frontHealthBar.fillAmount;          // Current fill amount of the front health bar
        float fillB = m_backHealthBar.fillAmount;           // Current fill amount of the back health bar
        float hFraction = m_currentHealth / m_maxHealth;    // Fraction of current health to max health

        m_healthText.text = Mathf.RoundToInt(hFraction * 100f) + " / " + ((int)m_maxHealth).ToString();
        if (fillB > hFraction)
        {   // Taking damage
            m_frontHealthBar.fillAmount = hFraction;
            m_backHealthBar.color = Color.red;
            m_lerpTimer += Time.deltaTime;
            float percentComplete = m_lerpTimer / m_chipSpeed;      // Calculate percentage of lerp completed
            percentComplete = percentComplete * percentComplete;    
            m_backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);    // Lerp back bar to new health fraction
        }

        if(fillF < hFraction)
        {   // Restoring health
            m_backHealthBar.fillAmount = hFraction;
            m_backHealthBar.color = Color.green;
            m_lerpTimer += Time.deltaTime;
            float percentComplete = m_lerpTimer / m_chipSpeed;
            percentComplete = percentComplete * percentComplete;
            m_frontHealthBar.fillAmount = Mathf.Lerp(fillF, hFraction, percentComplete);    // Lerp front bar to new health fraction
        }

    }

    /// <summary>
    ///     Reduces the player's current health by the specified damage amount.
    ///     Also triggers the damage overlay effect and resets the lerp timer
    ///     to smoothly update the health bar.
    /// </summary>
    /// <param name="damage">The amount of damage to subtract from the player's health.</param>
    public void TakeDamage(float damage)
    {
        SoundManager.m_instance.m_playerChannel.PlayOneShot(SoundManager.m_instance.m_playerHurtSound);
        m_currentHealth -= damage;
        m_lerpTimer = 0f;
        m_damageOverlayDurationTimer = 0f;
        m_damageOverlay.color = new Color(m_damageOverlay.color.r, m_damageOverlay.color.g, m_damageOverlay.color.b, 1);

        if (m_currentHealth <= 0)
        {
            PlayerDead();
        }
    }

    /// <summary>
    ///     Increases the player's current health by the specified heal amount.
    ///     Also resets the lerp timer to animate the health bar filling smoothly.
    /// </summary>
    /// <param name="healAmount">The amount of health to restore to the player.</param>
    public void RestoreHealth(float healAmount)
    {
        m_currentHealth += healAmount;
        m_lerpTimer = 0f;
    }

    public float GetHealth()
    {
        return m_currentHealth;
    }

    private void PlayerDead()
    {

        SoundManager.m_instance.m_playerChannel.PlayOneShot(SoundManager.m_instance.m_playerDieSound);
        
        SoundManager.m_instance.m_zombieChannel2.Stop();
        SoundManager.m_instance.m_zombieChannel.Stop();
        SoundManager.m_instance.m_playerChannel.clip = SoundManager.m_instance.m_playerDeadMusic;
        SoundManager.m_instance.m_playerChannel.PlayDelayed(1f);


        GetComponent<PlayerMotor>().enabled = false;
        GetComponent<InputManager>().enabled = false;
        m_isDead = true;

        //Dying Animation
        GetComponent<Animator>().enabled = true;
        GetComponent<ScreenFader>().StartFade();
        StartCoroutine(ShowGameOverUI());
    }
    private IEnumerator ShowGameOverUI()
    {
        yield return new WaitForSeconds(1f); // Wait for fade to complete
        m_gameOverUI.SetActive(true);

        int waveSurvived = GlobalReferences.m_instance.m_waveNumber;
        if(waveSurvived-1 > SaveLoadManagment.m_instance.LoadHighScore())
        {
            SaveLoadManagment.m_instance.SaveHighScore(waveSurvived - 1);
        }

        StartCoroutine(ReturnToMainMenu());
    }

    private IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(5f);
        GetComponent<InputManager>().enabled = false;

        SceneManager.LoadScene("MainMenu");
    }

    public bool IsDead()
    {
        return m_isDead;
    }
}

