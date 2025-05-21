using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public float Speed = 10f;
    public float Lifetime = 5f;
    private Rigidbody rb;

    public LayerMask CollisionLayer;
    private Transform respawnLocation;

    public ParticleSystem BulletEffect;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = transform.forward * Speed;
        }

        Destroy(gameObject, Lifetime);
    }

    void OnCollisionEnter(Collision other)
    {
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
        respawnLocation = GameObject.FindWithTag("RespawnPoint")?.transform;
    }

    void TeleportPlayer(GameObject player)
    {
        if (respawnLocation != null)
        {
            player.transform.position = respawnLocation.position;
        }
    }
}
