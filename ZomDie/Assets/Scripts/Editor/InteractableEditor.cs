using UnityEditor;

[CustomEditor(typeof(Interactable),true)]
public class InteractableEditor : Editor
{

    /// <summary>
    ///  This function overrides the default inspector GUI for the Interactable class.
    ///  First, it checks if the target object is of type EventOnlyInteractable.If it is , it displays a text field for the prompt message and a help box explaining that this interactable only uses events for interaction.
    ///  It also export the prompt message to be editable in the inspector.
    ///  If the object is an norma Interactable, it calls the base OnInspectorGUI method to display the default inspector GUI.And finally, it checks the m_useEvents boolean to add or remove the InteractionEvent component accordingly.
    /// </summary>
    public override void OnInspectorGUI()
    {
        Interactable interactable = (Interactable)target;
        if (target.GetType() == typeof(EventOnlyInteractable))
        {
            interactable.m_promtMessage = EditorGUILayout.TextField("Prompt Message",interactable.m_promtMessage);
            EditorGUILayout.HelpBox("This interactable only uses events for interaction. You can add listeners to the InteractionEvent component attached to this gameobject.",MessageType.Info);
            if(interactable.GetComponent<InteractionEvent>() == null)
            {
                interactable.gameObject.AddComponent<InteractionEvent>();
                interactable.m_useEvents = true;
                return;
            }
        }
        base.OnInspectorGUI();
        if(interactable.m_useEvents)
        {
            if(interactable.GetComponent<InteractionEvent>() == null)
            {
                interactable.gameObject.AddComponent<InteractionEvent>();
            }
                
        }
        else
                    {
            InteractionEvent interactionEvent = interactable.GetComponent<InteractionEvent>();
            if(interactionEvent != null)
            {
                DestroyImmediate(interactionEvent);
            }
        }
    }


}
