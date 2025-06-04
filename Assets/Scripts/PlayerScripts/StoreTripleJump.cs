using UnityEngine;

public class StoreTripleJump : MonoBehaviour
{
    [Header("Jump Settings")]
    public float jumpForce = 10f;
    public int maxStoredJumps = 3;
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private int storedJumps = 0;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Check if grounded and handle jump input
        CheckGround();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PerformJump();
        }
    }

    void CheckGround()
    {
        // Raycast down to check if player is grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
    }

    void PerformJump()
    {
        // Perform a jump if grounded or has stored jumps
        if (isGrounded || storedJumps > 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);

            if (!isGrounded)
            {
                storedJumps--;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Refill stored jumps when picking up a JumpItem
        if (other.CompareTag("JumpItem"))
        {
            storedJumps = maxStoredJumps;
        }
    }
}
