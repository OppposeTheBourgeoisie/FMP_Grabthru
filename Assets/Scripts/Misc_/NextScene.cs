using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public GameObject uiCanvas; // Assign your UI Canvas in Inspector

    public void LoadNextScene()
    {
        Destroy(uiCanvas); // Destroy UI before switching scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
