using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public bool MatchPickup;
    public bool OtherPickup;
    public bool JumpItemPickup;
    public PlayerShmove playerShmove;

    private void Start()
    {
        // Initialize pickup states and find PlayerShmove reference
        MatchPickup = false;
        OtherPickup = false;
        JumpItemPickup = false;
        playerShmove = FindObjectOfType<PlayerShmove>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Handle pickup based on collider tag
        if (other.CompareTag("Match"))
        {
            Destroy(other.gameObject);
            MatchPickedUp();
        }
        else if (other.CompareTag("Other"))
        {
            Destroy(other.gameObject);
            OtherPickedUp();
        }
        else if (other.CompareTag("JumpItem"))
        {
            Destroy(other.gameObject);
            JumpItemPickedUp();
        }
    }

    // Methods to set pickup states
    public void MatchPickedUp()
    {
        MatchPickup = true;
    }

    public void OtherPickedUp()
    {
        OtherPickup = true;
    }

    public void JumpItemPickedUp()
    {
        // Set jump item pickup state and add jumps to player
        JumpItemPickup = true;
        if (playerShmove != null)
        {
            playerShmove.AddJumps(3);
        }
    }
}
