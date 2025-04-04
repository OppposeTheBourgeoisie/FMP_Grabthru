using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI JumpCountText; 

    public PlayerShmove PlayerShmove;

    private void Start()
    {
        PlayerShmove = FindObjectOfType<PlayerShmove>();
    }

    private void Update()
    {
        if (PlayerShmove != null)
        {
            int CurrentJumps = PlayerShmove.GetCurrentJumps();
            
            // Hide the text if the player has no jumps left
            if (CurrentJumps == 0)
            {
                JumpCountText.gameObject.SetActive(false);
            }
            else
            {
                // Show the current jumps
                JumpCountText.gameObject.SetActive(true);
                JumpCountText.text = "Jumps: " + CurrentJumps.ToString();
            }
        }
    }
}
