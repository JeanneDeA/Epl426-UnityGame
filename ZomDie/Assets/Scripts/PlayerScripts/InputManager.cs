using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//This script is the centralized input manager for handling player inputs.All of the inputs we created with the Input control will pass through this script.
public class InputManager : MonoBehaviour
{
    //This is the reference to the PlayerInput component that handles input actions.
    public PlayerInput m_PlayerInput { get; private set; }

    //This is the reference to the OnFoot action map within the PlayerInput component.
    private PlayerMotor m_playerMotor;

    private PlayerLook m_playerLook;

    // Weapon (may be null if player has no weapon equipped)
    private PlayerWeapon m_playerWeapon;

    private bool m_ThrowableLethalIsBeingHeld = false;

    private bool m_ThrowableTacticalIsBeingHeld = false;

    private bool m_isAiming = false;
    private void Awake()
    {
        m_PlayerInput = new PlayerInput();
        m_playerMotor = GetComponent<PlayerMotor>();
        m_playerLook = GetComponent<PlayerLook>();
        AssignWeapon();
        //Subscribe to the jump action performed event to call the Jump method in the PlayerMotor script when the jump input is detected.
        //As well as the sprint and crouch actions.

        m_PlayerInput.OnFoot.Jump.performed += ctx => m_playerMotor.Jump();
        m_PlayerInput.OnFoot.Sprint.performed += ctx => m_playerMotor.Sprint();
        m_PlayerInput.OnFoot.Crouch.performed += ctx => m_playerMotor.Crouch();

        //// Reload
        m_PlayerInput.OnFoot.Reload.performed += ctx =>
        {
            if (m_playerWeapon != null) m_playerWeapon.Reload();
        };

        // Shoot: use started/canceled so we can support hold (automatic) and single press
        m_PlayerInput.OnFoot.Shoot.started += ctx =>
        {
            if (m_playerWeapon != null) m_playerWeapon.StartShooting();
        };
        m_PlayerInput.OnFoot.Shoot.canceled += ctx =>
        {
            if (m_playerWeapon != null) m_playerWeapon.StopShooting();
        };
        m_PlayerInput.OnFoot.Drop.performed += ctx =>
        {
            if (m_playerWeapon != null) WeaponManager.m_instance.DropCurrentWeapon();
        };
        // Switch weapon slots
        m_PlayerInput.OnFoot.ChangeWeaponSlot.performed += ctx =>
        {
            var bindingIndex = ctx.action.GetBindingIndexForControl(ctx.control);
            SwitchSlot(bindingIndex);
        };
        // Aim
        m_PlayerInput.OnFoot.Aim.started += ctx =>
        {
            if (!m_isAiming)
            {
                m_isAiming = true;

                m_playerWeapon.AimWeapon(m_isAiming);
            }

        };
        m_PlayerInput.OnFoot.Aim.canceled += ctx =>
        {
            if (m_isAiming)
            {
                m_isAiming = false;
                m_playerWeapon.AimWeapon(m_isAiming);
            }
        };
        m_PlayerInput.OnFoot.Throw.started += ctx =>
        {
            if(ctx.control.name == "g" && !m_ThrowableTacticalIsBeingHeld)
            {
                m_ThrowableLethalIsBeingHeld = true;
            }
            else if(ctx.control.name == "t" && !m_ThrowableLethalIsBeingHeld)
            {
                m_ThrowableTacticalIsBeingHeld = true;
            }

        };

        m_PlayerInput.OnFoot.Throw.canceled += ctx =>
        {
            if(ctx.control.name == "t" && m_ThrowableTacticalIsBeingHeld)
            {
                if(WeaponManager.m_instance.GetTacticalCount() > 0)
                {
                    WeaponManager.m_instance.ThrowTactical();
                }
                m_ThrowableTacticalIsBeingHeld = false;
                WeaponManager.m_instance.ResetThrowableThrowForce();
                return;
            }
            if(ctx.control.name == "g" && m_ThrowableLethalIsBeingHeld)
            {
                if (WeaponManager.m_instance.GetLethalCount() > 0)
                {
                    WeaponManager.m_instance.ThrowLethal();
                }
                m_ThrowableLethalIsBeingHeld = false;
                WeaponManager.m_instance.ResetThrowableThrowForce();
                return;

            }
        };
    }

    private void Update()
    {
        AssignWeapon();
        HandleThrowableCharging();
    }

    private void SwitchSlot(int slotIndex)
    {
        WeaponManager.m_instance.SwitchActiveSlot(slotIndex);
        AssignWeapon();
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        //tell the playermotor to move using the value from the input system(movment action)
        m_playerMotor.ProcessMove(m_PlayerInput.OnFoot.Movement.ReadValue<Vector2>());
        

    }

    private void LateUpdate()
    {
        m_playerLook.ProcessLook(m_PlayerInput.OnFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        m_PlayerInput.OnFoot.Enable();
    }
    private void OnDisable()
    {
        m_PlayerInput.OnFoot.Disable();
    }

    // TODO: Refactor this to use events or another method to notify when the weapon is changed/equipped.
    public void AssignWeapon()
    {
        m_playerWeapon = GetComponentInChildren<PlayerWeapon>();
    }

    private void HandleThrowableCharging()
    {
        if (m_ThrowableLethalIsBeingHeld || m_ThrowableTacticalIsBeingHeld)
        {
            WeaponManager.m_instance.ChargeThrowableThrowForce();
        }
    }


}
