using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class TeleportEndLevel : MonoBehaviour
{
    public GameObject MenuUI;
    public TMP_Text PerformanceText;
    public Button NextLevelButton;
    public Button MainMenuButton;

    private bool IsTeleporting = false;

    private PlayerShmove PlayerMovement;
    private CameraController CameraController;

    private void Start()
    {
        //Initialize the menu UI and buttons
        MenuUI.SetActive(false);

        NextLevelButton.onClick.AddListener(GoToNextLevel);
        MainMenuButton.onClick.AddListener(GoToMainMenu);

        PlayerMovement = FindObjectOfType<PlayerShmove>();
        CameraController = FindObjectOfType<CameraController>();
    }

    private void OnTriggerEnter(Collider Other)
    {
        if (Other.CompareTag("Player") && !IsTeleporting)
        {
            IsTeleporting = true;
            Timer.Instance.StopTimer();
            DisplayMenu(Other);
        }
    }

    private void DisplayMenu(Collider Player)
    {
        // Get the final time from the Timer script
        float FinalTime = Timer.Instance.GetElapsedTime();
        
        int Minutes = Mathf.FloorToInt(FinalTime / 60);
        int Seconds = Mathf.FloorToInt(FinalTime % 60);
        int Milliseconds = Mathf.FloorToInt((FinalTime * 100) % 100);

        string Performance = string.Format("Level Completed!\nTime: {0:00}:{1:00}:{2:00}", Minutes, Seconds, Milliseconds);

        PerformanceText.text = Performance;

        MenuUI.SetActive(true);

        //Disable player movement and camera control
        if (PlayerMovement != null)
        {
            PlayerMovement.enabled = false;
        }
        if (CameraController != null)
        {
            CameraController.enabled = false;
        }

        //Enable mouse cursor for UI interaction
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        NextLevelButton.interactable = true;
        MainMenuButton.interactable = true;
    }

    private void GoToNextLevel()
    {
        //Hide the cursor and menu UI
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;

        int CurrentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        //Load the next scene in the build index
        int NextSceneIndex = CurrentSceneIndex + 1;

        if (NextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(NextSceneIndex);
        }
        else
        {
            GoToMainMenu();  // If no next level, go to main menu
        }
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
