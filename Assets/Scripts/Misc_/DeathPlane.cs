using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    public Transform respawnPoint;
    public string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player collided with the death plane
        if (other.CompareTag(playerTag))
        {
            Respawn(other.gameObject);
        }
    }

    void Respawn(GameObject player)
    {
        // Respawn the player at the referenced respawn point, also resetting velocity
        if (respawnPoint != null)
        {
            player.transform.position = respawnPoint.position;
            player.transform.rotation = respawnPoint.rotation;
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}
