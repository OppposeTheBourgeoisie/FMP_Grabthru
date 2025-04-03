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

        // Enable mouse cursor for UI interaction
        Cursor.lockState = CursorLockMode.None;  // Unlock the cursor
        Cursor.visible = true;  // Make the cursor visible

        // Ensure buttons are interactable
        nextLevelButton.interactable = true;
        mainMenuButton.interactable = true;
    }

    private void GoToNextLevel()
    {
        // Hide and lock the cursor before transitioning
        Cursor.lockState = CursorLockMode.Locked;  // Lock the cursor back to the center
        Cursor.visible = false;  // Hide the cursor

        // Get the current scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Load the next scene by its build index
        int nextSceneIndex = currentSceneIndex + 1; // The next scene's build index

        // Ensure that the next scene index is valid (i.e., the scene exists in the build settings)
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);  // Load the next scene
        }
        else
        {
            Debug.LogWarning("There are no more scenes to load in the build settings!");
            // Optionally, load a main menu or show a message if no next scene is available
        }
    }

    private void GoToMainMenu()
    {
        // Load the main menu scene (replace with your actual main menu scene name)
        SceneManager.LoadScene("MainMenu");
    }
}
