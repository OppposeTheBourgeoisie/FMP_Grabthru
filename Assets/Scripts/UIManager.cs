using UnityEngine;
using UnityEngine.UI;  // If using standard Text
using TMPro;  // Uncomment if using TextMeshPro

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI jumpCountText;  // Uncomment if using TextMeshPro

    public PlayerShmove playerShmove;

    private void Start()
    {
        // Find the PlayerShmove script in the scene (if not assigned in the Inspector)
        playerShmove = FindObjectOfType<PlayerShmove>();
    }

    private void Update()
    {
        // Update the jump count UI
        if (playerShmove != null)
        {
            int currentJumps = playerShmove.GetCurrentJumps();
            
            // If jumps are 0, hide the text
            if (currentJumps == 0)
            {
                jumpCountText.gameObject.SetActive(false);  // Hide the text object
            }
            else
            {
                jumpCountText.gameObject.SetActive(true);   // Show the text object
                jumpCountText.text = "Jumps: " + currentJumps.ToString();  // Update the text
            }
        }
    }
}
