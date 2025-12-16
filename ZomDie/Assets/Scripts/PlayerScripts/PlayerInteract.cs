using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera m_camera;

    [SerializeField]
    private float m_rayDistance = 3f;

    [SerializeField]
    private LayerMask m_mask;

    private PlayerUI m_playerUI;

    private InputManager m_inputManager;

    private GameObject m_hoveredWeapon =null;

    private GameObject m_hoveredAmmoBox = null;

    private GameObject m_hoveredGrenade =null;

    private GameObject m_hoveredHealthPack = null;

    void Start()
    {
        m_camera = GetComponentInChildren<Camera>();
        m_playerUI = GetComponent<PlayerUI>();
        m_inputManager = GetComponent<InputManager>();
    }


    void Update()
    {
        m_playerUI.UpdateText(""); //clear the prompt text each frame
        
        //create a ray from the camera forward,shooting outwards
        Ray ray = new Ray(m_camera.transform.position, m_camera.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * m_rayDistance, Color.red);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, m_rayDistance, m_mask))
        {
            //Debug.Log("Hit: " + hitInfo.collider.name);
            //check if the object we hit has an interactable component
            Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
            if (interactable != null)
            { 
                if(interactable.CompareTag("Weapon"))
                {
                    m_hoveredWeapon = hitInfo.collider.gameObject;
                    OutlineWeapon();
                }
                if(interactable.CompareTag("Ammo"))
                {
                    m_hoveredAmmoBox = hitInfo.collider.gameObject;
                    OutlineAmmoBox();
                }
                if(interactable.CompareTag("Grenade"))
                {
                    m_hoveredGrenade = hitInfo.collider.gameObject;
                    OutlineGrenade();
                }
                if(interactable.CompareTag("HealthPack"))
                {
                    m_hoveredHealthPack = hitInfo.collider.gameObject;
                    OutlineHealthPack();
                }
                //update the ui text to show the interactable's prompt message
                m_playerUI.UpdateText(interactable.m_promtMessage);
                //if we press the interact button,call the interact method on the interactable object
                if (m_inputManager.m_PlayerInput.OnFoot.Interact.triggered)
                {
                    interactable.BaseInteract();
                }
                return; //exit early if we found an interactable
            }
           
        }
        if (m_hoveredWeapon != null)
        {
           // Debug.Log("No longer hovering over weapon");
            RemoveOutlineWeapon();
        }
        if(m_hoveredAmmoBox != null)
        {
           // Debug.Log("No longer hovering over ammo box");
            RemoveOutlineAmmoBox();
        }
        if(m_hoveredGrenade != null)
        {
           // Debug.Log("No longer hovering over grenade");
            RemoveOutlineGrenade();
        }
        if(m_hoveredHealthPack != null)
        {
           // Debug.Log("No longer hovering over health pack");
            RemoveOutlineHealthPack();
        }
    }

    public void OutlineWeapon()
    {
            var outlineComp = m_hoveredWeapon.GetComponent<Outline>();
            if (outlineComp != null)
            {
                outlineComp.enabled = true;
            }
    }

    public void OutlineAmmoBox()
    {
        var outlineComp = m_hoveredAmmoBox.GetComponent<Outline>();
        if (outlineComp != null)
        {
            outlineComp.enabled = true;
        }
    }

    public void OutlineHealthPack()
    {
        var outlineComp = m_hoveredHealthPack.GetComponent<Outline>();
        if (outlineComp != null)
        {
            outlineComp.enabled = true;
        }
    }
    public void OutlineGrenade()
    {
        var outlineComp = m_hoveredGrenade.GetComponent<Outline>();
        if (outlineComp != null)
        {
            outlineComp.enabled = true;
        }
    }

    public void RemoveOutlineHealthPack()
    {
       // Debug.Log("Removing Outline from health pack");
        var outlineComp = m_hoveredHealthPack.GetComponent<Outline>();
            if (outlineComp != null)
            {
                outlineComp.enabled = false;
            }
            m_hoveredHealthPack = null;
    }



    public void RemoveOutlineGrenade()
    {
       // Debug.Log("Removing Outline from grenade");
        var outlineComp = m_hoveredGrenade.GetComponent<Outline>();
            if (outlineComp != null)
            {
                outlineComp.enabled = false;
            }
            m_hoveredGrenade = null;
    }

    public void RemoveOutlineAmmoBox()
    {
        //Debug.Log("Removing Outline from ammo box");
        var outlineComp = m_hoveredAmmoBox.GetComponent<Outline>();
            if (outlineComp != null)
            {
                outlineComp.enabled = false;
            }
            m_hoveredAmmoBox = null;
    }

    public void RemoveOutlineWeapon()
    {
       // Debug.Log("Removing Outline from weapon");
        var outlineComp = m_hoveredWeapon.GetComponent<Outline>();
            if (outlineComp != null)
            {
                outlineComp.enabled = false;
            }
            m_hoveredWeapon = null;
    }
}
