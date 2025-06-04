using UnityEngine;

public class TubeFloat : MonoBehaviour
{
    [Header("Tube Float Settings")]
    public float liftSpeed = 15f;
    public float launchForce = 200f;
    public Transform exitPoint;
    public Rigidbody playerRigidBody;

    private void OnTriggerEnter(Collider other)
    {
        // Disable gravity and start lifting the player up
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.velocity = Vector3.up * liftSpeed;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Keep the player moving upward while inside the tube
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            rb.velocity = new Vector3(rb.velocity.x, liftSpeed, rb.velocity.z);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Launch the player out of the tube and re-enable gravity
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            rb.useGravity = true;
            Vector3 launchDirection = (exitPoint.position - transform.position).normalized;
            rb.velocity = launchDirection * launchForce;
            playerRigidBody.velocity = Vector3.zero;
            playerRigidBody.angularVelocity = Vector3.zero;
            playerRigidBody.AddForce(exitPoint.forward * launchForce, ForceMode.Impulse);
        }
    }
}
