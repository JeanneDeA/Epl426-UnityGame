using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class will act as the base class for all interactable objects in the game.I am using the Template Method Design Pattern here.
/// Here is a usefull link if you want to learn more about this design pattern: https://dotnettutorials.net/lesson/template-method-design-pattern/
/// </summary>
public abstract class Interactable : MonoBehaviour
{

    //Add or remove an InteractionEvent component to this gameobject to enable or disable event-based interactions.
    public bool m_useEvents;

    public string m_promtMessage;

    /// <summary>
    /// This is the base Interact method that will be called when the player interacts with this object.It makes sure that the order of the operations is the same for all interactable objects.
    /// Here first if the interactable object has the m_useEvents boolean set to true, it will try to get the InteractionEvent component from this gameobject and invoke the OnInteract UnityEvent.
    /// Reardless of whether the m_useEvents is true or false, it will then call the Interact method which is meant to be overridden by the derived classes to implement their specific interaction logic.
    /// </summary>
    public void BaseInteract()
    {
        if(m_useEvents)
        {
            InteractionEvent interactionEvent = GetComponent<InteractionEvent>();
            if (interactionEvent != null)
            {
                interactionEvent.OnInteract.Invoke();
            }
        }
        Interact();
    }

    protected virtual void Interact()
    {

    }

}
