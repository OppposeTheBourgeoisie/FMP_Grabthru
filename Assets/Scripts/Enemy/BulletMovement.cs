using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public float Speed = 10f;
    public float Lifetime = 5f;
    public LayerMask CollisionLayer;
    public ParticleSystem BulletEffect;

    private Rigidbody rb;
    private Transform respawnLocation;

    void Start()
    {
        // Set bullet velocity and schedule destruction
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = transform.forward * Speed;
        }
        Destroy(gameObject, Lifetime);
    }

    void OnCollisionEnter(Collision other)
    {
        // Handle collision with player and play effect
        if (other.gameObject.CompareTag("Player"))
        {
            FindRespawnLocation();

            if (respawnLocation != null)
            {
                TeleportPlayer(other.gameObject);
            }

            Destroy(gameObject);

            if (BulletEffect != null)
            {
                ParticleSystem effect = Instantiate(BulletEffect, transform.position, Quaternion.identity);
                effect.Play();
                Destroy(effect.gameObject, effect.main.duration);
            }
        }
    }

    void FindRespawnLocation()
    {
        // Find the respawn point in the scene
        respawnLocation = GameObject.FindWithTag("RespawnPoint")?.transform;
    }

    void TeleportPlayer(GameObject player)
    {
        // Teleport the player to the respawn location
        if (respawnLocation != null)
        {
            player.transform.position = respawnLocation.position;
        }
    }
}
