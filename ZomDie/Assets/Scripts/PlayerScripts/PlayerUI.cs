using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// This class manages the player's UI elements, such as updating prompt text on the screen.It takes a Text UI element and changes its text.
/// </summary>
public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_promtText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

   public void UpdateText(string promptMessage)
   {
        m_promtText.text = promptMessage;
    }
}
