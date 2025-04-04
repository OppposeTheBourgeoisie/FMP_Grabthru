using UnityEngine;

public class TubeFloat : MonoBehaviour
{
    public float LiftSpeed = 15f;
    public float LaunchForce = 200f;
    public Transform ExitPoint;

    public Rigidbody PlayerRigidBody;

    public PlayerShmove playerShmove;

    private void OnTriggerEnter(Collider other)
    {
        //When the player enters the trigger, disable gravity 
        if (other.CompareTag("Player"))
        {
            Rigidbody Rb = other.GetComponent<Rigidbody>();

            Rb.useGravity = false;
            Rb.velocity = Vector3.up * LiftSpeed;
            //Disable player movement
            playerShmove.enabled = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //While the player is inside the trigger, move them upwards
        if (other.CompareTag("Player"))
        {
            Rigidbody Rb = other.GetComponent<Rigidbody>();

            Rb.velocity = new Vector3(Rb.velocity.x, LiftSpeed, Rb.velocity.z);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //When the player exits the trigger, re-enable gravity and launch them in the direction of the exit point
        if (other.CompareTag("Player"))
        {
            Rigidbody Rb = other.GetComponent<Rigidbody>();

            Rb.useGravity = true;
            Vector3 LaunchDirection = (ExitPoint.position - transform.position).normalized;
            Rb.velocity = LaunchDirection * LaunchForce;
            PlayerRigidBody.velocity = Vector3.zero;
            PlayerRigidBody.angularVelocity = Vector3.zero;
            PlayerRigidBody.AddForce(ExitPoint.forward * LaunchForce, ForceMode.Impulse);
            //Enable player movement
            playerShmove.enabled = true;
        }
    }
}
