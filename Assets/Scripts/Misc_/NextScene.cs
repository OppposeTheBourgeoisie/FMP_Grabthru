using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public GameObject UiCanvas;

    public void LoadNextScene()
    {
        //Load the next scene and destroy the UI canvas
        Destroy(UiCanvas);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
