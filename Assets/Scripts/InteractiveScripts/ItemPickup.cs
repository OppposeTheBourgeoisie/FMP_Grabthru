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
        MatchPickup = false;
        OtherPickup = false;
        JumpItemPickup = false;
        playerShmove = FindObjectOfType<PlayerShmove>();
    }

    void OnTriggerEnter(Collider other)
    {
        //Check if the obejct has a specific tag
        PickupRespawner respawner = other.GetComponent<PickupRespawner>();

        if (other.CompareTag("Match"))
        {
            MatchPickedUp();
            if (respawner != null) respawner.StartRespawn();
            else Destroy(other.gameObject);
        }
        else if (other.CompareTag("Other"))
        {
            OtherPickedUp();
            if (respawner != null) respawner.StartRespawn();
            else Destroy(other.gameObject);
        }
        else if (other.CompareTag("JumpItem"))
        {
            JumpItemPickedUp();
            if (respawner != null) respawner.StartRespawn();
            else Destroy(other.gameObject);
        }
    }

    //Methods to see what items have been picked up
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
