using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public float Speed = 10f;          // Speed at which the bullet moves
    public float Lifetime = 5f;        // Time before the bullet is destroyed (to avoid infinite life)
    private Rigidbody rb;              // Rigidbody reference for 3D games (use Rigidbody2D for 2D games)

    public LayerMask CollisionLayer;  // Layer mask to control bullet collisions
    private Transform respawnLocation; // Reference to the respawn location (assigned at runtime)

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // Get Rigidbody component (for 3D)

        if (rb != null)
        {
            rb.velocity = transform.forward * Speed;  // Move the bullet forward
        }

        // Destroy the bullet after its lifetime expires
        Destroy(gameObject, Lifetime);
    }

    void OnCollisionEnter(Collision other)
    {
        // Check if the bullet collides with the player
        if (other.gameObject.CompareTag("Player"))  // You can change the tag to whatever your player uses
        {
            // Find the respawn location in the scene (search by name or tag)
            FindRespawnLocation();

            if (respawnLocation != null)
            {
                // Teleport the player to the respawn location
                TeleportPlayer(other.gameObject);
            }
            else
            {
                Debug.LogError("No RespawnLocation found in the scene!");
            }

            // Destroy the bullet
            Destroy(gameObject);
        }
    }

    void FindRespawnLocation()
    {
        // Try to find the RespawnLocation by tag or name
        respawnLocation = GameObject.FindWithTag("RespawnPoint")?.transform;

        // Alternatively, you can use GameObject.Find if you're searching by name (if you don't use tags):
        // respawnLocation = GameObject.Find("RespawnLocation")?.transform;
    }

    void TeleportPlayer(GameObject player)
    {
        if (respawnLocation != null)
        {
            player.transform.position = respawnLocation.position;  // Teleport player to the respawn location
        }
    }
}
