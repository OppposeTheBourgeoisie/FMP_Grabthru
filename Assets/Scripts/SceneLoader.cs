using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Loads scenes based on button clicks in the UI
    public void StartGame()
    {
        SceneManager.LoadScene("IntroScene");
    }

    public void LoadControls()
    {
        SceneManager.LoadScene("ControlScene");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        // Quit the application or stop the scene in the editor
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
