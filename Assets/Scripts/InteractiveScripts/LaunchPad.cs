using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPad : MonoBehaviour
{
    public Rigidbody playerRigidbody;
    public Transform cannonMouth;
    public float launchForce = 1000f;

    private void OnTriggerEnter(Collider other)
    {
        // Launch the player when they enter the trigger
        LaunchPlayer();
    }

    void LaunchPlayer()
    {
        // Move player to cannon mouth and launch with force
        if (playerRigidbody != null && cannonMouth != null)
        {
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
            playerRigidbody.transform.position = cannonMouth.position;
            playerRigidbody.AddForce(cannonMouth.forward * launchForce, ForceMode.Impulse);
        }
    }
}
