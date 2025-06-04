using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePadInteract : MonoBehaviour
{
    public Rigidbody playerRigidBody;

    public TimedDoor timedDoor;

    private void OnTriggerEnter(Collider other)
    {
        // Check for collision
        SteppedOn();
    }

    void SteppedOn()
    {
        // Open the door when the player steps on the pressure pad
        timedDoor.OpenDoor();
    }
}
