using UnityEngine;

public class Bullet : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the bullet collides with the player (assuming the player has a tag "Player")
        if (collision.gameObject.CompareTag("Player"))
        {

            // Reset the player by calling the ResetPlayer method
            ResetPlayer(collision.gameObject);

            // Destroy the bullet after hitting the player
            Destroy(gameObject);
        }
    }

    private void ResetPlayer(GameObject player)
    {
        // Get the global respawn point from the RespawnManager
        RespawnManager respawnManager = FindObjectOfType<RespawnManager>();

        if (respawnManager != null && respawnManager.globalRespawnPoint != null)
        {
            // Set the player's position to the global respawn point
            player.transform.position = respawnManager.globalRespawnPoint.position;
        }
        else
        {
            Debug.LogError("RespawnManager or global respawn point not found!");
        }

        // Optionally, reset velocity, rotation, etc.
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;  // Stop the player’s movement
            rb.angularVelocity = Vector3.zero;  // Stop the player’s rotation
        }

        Debug.Log("Player has been reset!");
    }
}
