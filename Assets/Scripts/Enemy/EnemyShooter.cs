using System.Collections;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject projectilePrefab;  // The projectile to be shot
    public Transform shootPoint;         // The point from where the projectile will be shot
    public float shootInterval = 5f;     // Time between each shot
    public float projectileSpeed = 5f;   // Speed of the projectile

    private void Start()
    {
        // Start the shooting behavior
        StartCoroutine(ShootContinuously());
    }

    private IEnumerator ShootContinuously()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(shootInterval);
        }
    }

    private void Shoot()
    {
        // Create the projectile at the shootPoint's position, facing the correct direction
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

        // Get the Rigidbody component of the projectile
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Set the direction of the projectile in the world space (e.g., along the z-axis)
            rb.velocity = shootPoint.forward * projectileSpeed;  // Shoot forward based on the shootPoint's orientation
        }
    }
}
