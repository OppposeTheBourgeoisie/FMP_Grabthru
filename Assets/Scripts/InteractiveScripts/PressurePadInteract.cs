using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePadInteract : MonoBehaviour
{
    public Rigidbody playerRigidBody;

    public TimedDoor timedDoor;

    private void OnTriggerEnter(Collider other)
    {
        SteppedOn();
    }

    void SteppedOn()
    {
        timedDoor.OpenDoor();
    }
}
