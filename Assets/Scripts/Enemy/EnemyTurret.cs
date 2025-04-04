using UnityEngine;

public class EnemyTurret : MonoBehaviour
{
    public Transform PointA;
    public Transform PointB;
    public float MoveSpeed = 2f;
    public float FireRate = 1f;
    public GameObject BulletPrefab;
    public Transform FirePoint;
    public float BulletSpeed = 10f;

    private float fireCooldown = 0f;

    private bool movingToB = true;

    void Update()
    {
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
        //Move the turret between Point A and Point B
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
        //Fire a projectile from the turret in the direction it is facing
        if (BulletPrefab != null && FirePoint != null)
        {
            GameObject bullet = Instantiate(BulletPrefab, FirePoint.position, FirePoint.rotation);

            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

            if (bulletRb != null)
            {
                bulletRb.velocity = FirePoint.forward * BulletSpeed;
            }
        }
    }
}
