using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// This class will handle all of the player movement functionality.
public class PlayerMotor : MonoBehaviour
{
    private CharacterController m_characterController;

    private Vector3 m_playerVelocity;

    private bool m_isGrounded;

    private float m_gravity = -9.81f;   

    [SerializeField]
    private float m_speed = 5f;

    [SerializeField]
    private float m_jumpHeight = 1.5f;

    private bool m_CrouchAnimation = false;

    // How long the crouch/stand transition should take (seconds)
    [SerializeField]
    private float m_crouchDuration = 0.2f;

    private float m_crouchTimer = 0f;

    private bool m_Crouch = false;

    private bool m_isSprinting = false;

    private  float m_characterHeight;

    private bool m_CheckedStandUp = false;
    // Start is called before the first frame update

    private float m_crouchHeight;
    void Start()
    {
        m_characterController = GetComponent<CharacterController>();
        // record the controller's default standing values
        m_characterHeight = m_characterController.height;
        m_crouchHeight = m_characterHeight / 2f;
        if (m_crouchDuration <= 0f)
        {
            m_crouchDuration = 0.1f;
        }

    }

    // Update is called once per frame
    void Update()
    {
        //always update if the player is grounded or not.
        m_isGrounded = m_characterController.isGrounded;
        CrouchAnimation(); 
    }
    /// <summary>
    /// Receives the inputs from the InputManager script and applies them to the character controler to move the player.
    /// Its called in FixedUpdate for better physics calculations.As well as for the gravity effect on the player.Only the jump function is called separately since its a one time action and its physics based.
    /// </summary>
    /// <param name="input"></param>
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        m_characterController.Move(transform.TransformDirection(moveDirection) * m_speed * Time.deltaTime);
        m_playerVelocity.y += m_gravity * Time.deltaTime;
        if(m_isGrounded && m_playerVelocity.y <0)
        {
            m_playerVelocity.y = -2f;
        }
        m_characterController.Move(m_playerVelocity * Time.deltaTime);
    }

    /// <summary>
    /// This function gets called by the InputManager when the player presses the jump button.
    /// Checks if the player is grounded before allowing the jump.It then applies an upward velocity to the player based on the jump height and gravity.
    /// </summary>
    public void Jump()
    {
        if(m_isGrounded)
        {
            m_playerVelocity.y = Mathf.Sqrt(m_jumpHeight * -3f * m_gravity);
        }
    }

    /// <summary>
    /// This function gets called by the InputManager when the player presses the crouch button.
    /// </summary>
    public void Crouch()
    {
        m_Crouch = !m_Crouch;
        m_CrouchAnimation = true;
        m_crouchTimer = 0f;
        m_CheckedStandUp = false;
    }

    /// <summary>
    /// This method handles the crouch animation by smoothly transitioning the character controller's height to the full height from crouch or from the full height 
    /// to the crouch position over a specified duration.It does this by finding how much time has elapsed from the start of the animation makes it from 0-1 by dibving it with the total duration
    /// and then uses lerp to smoothly transition between the two heights.
    /// Example: If the player is crouching, it will lerp from the full height to half height, and if the player is standing up, it will lerp from half height to full height.
    /// Example: If the crouch duration is 4 seconds, after 2 seconds the height will be halfway between the two heights.
    /// </summary>
    private void CrouchAnimation()
    {

        if (!m_CrouchAnimation)
        {
            return;
        }
        m_crouchTimer += Time.deltaTime;
        float transitionStep = m_crouchTimer/m_crouchDuration;
        transitionStep = transitionStep * transitionStep;
        if (m_Crouch)
        {
            m_characterController.height = Mathf.Lerp(m_characterHeight, m_crouchHeight, transitionStep);
        }
        else
        {
        
            if(!m_CheckedStandUp)
            {
                m_CheckedStandUp = true;
                if (!CanStandUp())
                {
                    // cannot stand up yet, wait and try again next try
                    m_CrouchAnimation = false;
                    m_crouchTimer = 0f;
                    m_Crouch = true;
                    return;
                }
            }

            m_characterController.height = Mathf.Lerp(m_crouchHeight, m_characterHeight, transitionStep);
        }
        if (transitionStep >= 1f)
        {
            m_CrouchAnimation = false;
            m_crouchTimer = 0f;
        }
       
    }

    /// <summary>
    /// Checks if there is enough space above the player to stand up from a crouch position.
    /// </summary>
    /// <returns></returns>
    private bool CanStandUp()
    {
        // radius to test with (use slightly smaller than controller radius to avoid false positives)
        float radius = Mathf.Max(0.05f, m_characterController.radius * 0.9f);

        // world center of the character controller
        Vector3 worldCenter = transform.position + m_characterController.center;

        // top positions for crouch and stand (world space)
        Vector3 crouchTop = worldCenter + Vector3.up * (m_crouchHeight * 0.5f);
        Vector3 standTop = worldCenter + Vector3.up * (m_characterHeight * 0.5f);

        // Overlap the capsule between the current crouch top and the stand top
        Collider[] hits = Physics.OverlapCapsule(crouchTop, standTop, radius, ~0, QueryTriggerInteraction.Ignore);

        // Ignore the player's own collider(s)
        foreach (var hit in hits)
        {
            // ignore colliders that belong to this player (same GameObject or children)
            if (hit == null || hit.gameObject == gameObject || hit.transform.IsChildOf(transform))
            {
                continue;
            }
            return false;
        }
        return true;
    }

    public void Sprint()
    {
        m_isSprinting = !m_isSprinting;
        if (m_isSprinting)
        {
            m_speed = 8f;
        }
        else
        {
            m_speed = 5f;
        }
    }
}
