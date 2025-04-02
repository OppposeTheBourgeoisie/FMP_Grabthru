using UnityEngine;

public class CannonLauncher : MonoBehaviour
{
    public Rigidbody playerRigidbody;
    public Transform cannonMouth;
    public float launchForce = 1000f;
    public KeyCode launchKey = KeyCode.Space;

    private bool hasLaunched = false;


    void Update()
    {
        if (Input.GetKeyDown(launchKey) && !hasLaunched)
        {
            LaunchPlayer();
        }
    }

    void LaunchPlayer()
    {
        if (playerRigidbody != null && cannonMouth != null)
        {
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
            playerRigidbody.transform.position = cannonMouth.position;
            playerRigidbody.AddForce(cannonMouth.forward * launchForce, ForceMode.Impulse);
            hasLaunched = true;
        }
    }
}
