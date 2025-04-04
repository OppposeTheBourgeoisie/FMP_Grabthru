using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPad : MonoBehaviour
{
    public Rigidbody PlayerRigidbody;
    public Transform CannonMouth;
    public float LaunchForce = 1000f;
    [Range(0f, 1f)] public float UpwardModifier = 0.3f;

    private void OnTriggerEnter(Collider Other)
    {
        LaunchPlayer();
    }

    void LaunchPlayer()
    {
        if (PlayerRigidbody != null && CannonMouth != null)
        {
            // Reset the player's velocity and position and then apply the launch force
            PlayerRigidbody.velocity = Vector3.zero;
            PlayerRigidbody.angularVelocity = Vector3.zero;
            PlayerRigidbody.transform.position = CannonMouth.position;

            Vector3 LaunchDirection = (CannonMouth.forward + Vector3.up * UpwardModifier).normalized;

            PlayerRigidbody.AddForce(LaunchDirection * LaunchForce, ForceMode.Impulse);
        }
    }
}
