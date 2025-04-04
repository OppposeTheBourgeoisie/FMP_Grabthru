using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    public Transform RespawnPoint;
    public string PlayerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        //Check if the player has entered the trigger
        if (other.CompareTag(PlayerTag))
        {
            Respawn(other.gameObject);
        }
    }

    void Respawn(GameObject player)
    {
        //Respawn the player at the RespawnPoint
        if (RespawnPoint != null)
        {
            player.transform.position = RespawnPoint.position;
            player.transform.rotation = RespawnPoint.rotation;
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}
