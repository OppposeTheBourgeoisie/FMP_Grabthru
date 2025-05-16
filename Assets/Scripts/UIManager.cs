using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI jumpCountText;

    public PlayerShmove playerShmove;

    private void Start()
    {
        // Find the PlayerShmove script in the scene
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
                jumpCountText.gameObject.SetActive(false);
            }
            else
            {
                jumpCountText.gameObject.SetActive(true);
                jumpCountText.text = "Jumps: " + currentJumps.ToString();
            }
        }
    }
}
