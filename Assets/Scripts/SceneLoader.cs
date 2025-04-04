using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void StartGame()
    {
        //Load the first level of the game
        SceneManager.LoadScene("Level1");
    }

    public void LoadControls()
    {
        //Load the controls scene
        SceneManager.LoadScene("ControlScene");
    }

    public void LoadMainMenu()
    {
        //Load the main menu scene
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        // Quit the application
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
