using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad :Interactable
{
    private bool m_doorOpen = false;

    [SerializeField]
    private GameObject m_door;

    /// <summary>
    /// This method is called when the player interacts with the keypad.It is overridden from the base Interactable class.
    /// It toggles the door's open state and triggers the door's animation by passing the value of the bool in the parameter of the animation.
    /// </summary>
    protected override void Interact()
    {
        Debug.Log("Interacted with"+gameObject.name);
        m_doorOpen = !m_doorOpen;
        m_door.GetComponent<Animator>().SetBool("IsOpen", m_doorOpen);
    }
}
