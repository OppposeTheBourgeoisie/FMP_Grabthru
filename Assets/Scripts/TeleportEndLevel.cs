using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro; // Import TextMeshPro namespace
using UnityEngine.UI;

public class TeleportEndLevel : MonoBehaviour
{
    public GameObject menuUI; // Reference to the menu UI panel
    public TMP_Text performanceText; // TextMeshPro reference for performance (e.g., time, score)
    public Button nextLevelButton;
    public Button mainMenuButton;

    private bool isTeleporting = false;

    private PlayerShmove playerMovement; // Reference to the player movement script
    private CameraController cameraController; // Reference to the camera control script (if separate)

    private void Start()
    {
        // Hide the menu initially
        menuUI.SetActive(false);

        // Button event listeners
        nextLevelButton.onClick.AddListener(GoToNextLevel);
        mainMenuButton.onClick.AddListener(GoToMainMenu);

        // Get references to the player movement and camera controller
        playerMovement = FindObjectOfType<PlayerShmove>();
        cameraController = FindObjectOfType<CameraController>(); // Assuming CameraController is the name of the camera control script
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTeleporting)
        {
            isTeleporting = true;
            Timer.Instance.StopTimer(); // Stop and hide the timer immediately when the player collides with the object
            DisplayMenu(other); // Pass the collider to the DisplayMenu method
        }
    }

    private void DisplayMenu(Collider player)
    {
        // Get the final time from the Timer script
        float finalTime = Timer.Instance.GetElapsedTime();
        
        // Format the time into minutes, seconds, and milliseconds
        int minutes = Mathf.FloorToInt(finalTime / 60);
        int seconds = Mathf.FloorToInt(finalTime % 60);
        int milliseconds = Mathf.FloorToInt((finalTime * 100) % 100);

        // Create the performance string with time
        string performance = string.Format("Level Completed!\nTime: {0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);

        // Display the performance text using TextMeshPro
        performanceText.text = performance;

        // Show the menu UI
        menuUI.SetActive(true);

        // Disable player movement and camera control
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
        if (cameraController != null)
        {
            cameraController.enabled = false;
        }
    }

    private void GoToNextLevel()
    {
        // Load the next scene (make sure to set the correct next scene name in the Inspector)
        string nextSceneName = "NextLevel"; // Replace with the actual scene name
        SceneManager.LoadScene(nextSceneName);
    }

    private void GoToMainMenu()
    {
        // Load the main menu scene (replace with your actual main menu scene name)
        SceneManager.LoadScene("MainMenu");
    }
}
