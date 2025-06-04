using UnityEngine;

public class EnemyTurret : MonoBehaviour
{
    [Header("Turret Settings")]
    public Transform PointA;
    public Transform PointB;
    public float MoveSpeed = 2f;
    public float FireRate = 1f;
    public GameObject BulletPrefab;
    public Transform FirePoint;
    public float BulletSpeed = 10f;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip FireSound;

    private float fireCooldown = 0f;
    private bool movingToB = true;

    void Update()
    {
        // Move turret and handle firing cooldown
        MoveTurret();

        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            FireProjectile();
            fireCooldown = FireRate;
        }
    }

    void MoveTurret()
    {
        // Move the turret between Point A and Point B
        if (movingToB)
        {
            transform.position = Vector3.MoveTowards(transform.position, PointB.position, MoveSpeed * Time.deltaTime);
            if (transform.position == PointB.position)
                movingToB = false;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, PointA.position, MoveSpeed * Time.deltaTime);
            if (transform.position == PointA.position)
                movingToB = true;
        }
    }

    void FireProjectile()
    {
        // Fires the bullet from the turret and plays the firing sound
        if (BulletPrefab != null && FirePoint != null)
        {
            GameObject bullet = Instantiate(BulletPrefab, FirePoint.position, FirePoint.rotation);

            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                bulletRb.velocity = FirePoint.forward * BulletSpeed;
            }

            if (FireSound != null)
            {
                AudioSource.PlayClipAtPoint(FireSound, FirePoint.position, 1f);
            }
        }
    }
}
