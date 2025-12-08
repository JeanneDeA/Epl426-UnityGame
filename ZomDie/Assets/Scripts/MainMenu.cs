using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_hightScoreUI;

    private readonly string m_newGameScene = "Test 1";

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //Set hight score
        int hightScore = SaveLoadManagment.m_instance.LoadHighScore();
        m_hightScoreUI.text = $"Top Wave Survived: {hightScore}";
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(m_newGameScene);
    }

    public void ExitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
