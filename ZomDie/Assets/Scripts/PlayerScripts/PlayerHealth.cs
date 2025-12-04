using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    private float m_currentHealth;
    private float m_lerpTimer;                  // Timer for lerping health bar
    private float m_maxHealth = 100f;

    [Header("Health Bar")]
    public float m_chipSpeed = 2f;              // Speed at which the health bar chips away
    public TMP_Text m_healthText;
    public Image m_frontHealthBar;
    public Image m_backHealthBar;

    [Header("Damage Overlay")]
    public Image m_damageOverlay;
    public float m_fadeSpeed = 1.5f;            // Speed at which the damage overlay fades
    public float m_damageOverlayDuration = 1f;  // Duration the damage overlay stays fully visible
    private float m_damageOverlayDurationTimer;// Timer for damage overlay duration

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
        m_currentHealth -= damage;
        m_lerpTimer = 0f;
        m_damageOverlayDurationTimer = 0f;
        m_damageOverlay.color = new Color(m_damageOverlay.color.r, m_damageOverlay.color.g, m_damageOverlay.color.b, 1);

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
}

