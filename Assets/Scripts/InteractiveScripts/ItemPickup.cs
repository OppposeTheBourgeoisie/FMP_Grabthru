using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public bool MatchPickup;
    public bool OtherPickup;
    public bool JumpItemPickup;
    public PlayerShmove playerShmove; // Reference to the PlayerShmove script

    private void Start()
    {
        MatchPickup = false;
        OtherPickup = false;
        JumpItemPickup = false;
        playerShmove = FindObjectOfType<PlayerShmove>(); // Automatically find the PlayerShmove script attached to the player
    }

    void OnTriggerEnter(Collider other)
    {
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
        JumpItemPickup = true;

        if (playerShmove != null)
        {
            playerShmove.AddJumps(3);
        }
    }
}
