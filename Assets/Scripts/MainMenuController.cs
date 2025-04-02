using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        // Load the GameScene (make sure the name matches your scene's name)
        SceneManager.LoadScene("ShowcaseLevel");
    }

    public void ControlsMenu()
    {
        SceneManager.LoadScene("ControlScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
