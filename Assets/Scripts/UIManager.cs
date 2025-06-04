using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI jumpCountText;
<<<<<<< HEAD
=======

>>>>>>> c0b8d4f5f58296da443441af140f39f3b779bc47
    public PlayerShmove playerShmove;

    private void Start()
    {
<<<<<<< HEAD
        // Get reference to the PlayerShmove component
=======
        // Find the PlayerShmove script in the scene
>>>>>>> c0b8d4f5f58296da443441af140f39f3b779bc47
        playerShmove = FindObjectOfType<PlayerShmove>();
    }

    private void Update()
    {
        // Update the jump count UI and set the visibility based on the player's jump count
        if (playerShmove != null)
        {
            int currentJumps = playerShmove.GetCurrentJumps();

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
