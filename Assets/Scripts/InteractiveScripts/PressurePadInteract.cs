using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePadInteract : MonoBehaviour
{
    public Rigidbody PlayerRigidBody;

    public TimedDoor TimedDoor;

    private void OnTriggerEnter(Collider other)
    {
        SteppedOn();
    }

    void SteppedOn()
    {
        //When stepped on, the door open event will trigger
        TimedDoor.OpenDoor();
    }
}
