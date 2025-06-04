using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public GameObject uiCanvas;

    public void LoadNextScene()
    {
        // Destroy the displayed UI canvas and load the next scene
        Destroy(uiCanvas);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
