using UnityEngine;

public class StoreTripleJump : MonoBehaviour
{
    public float jumpForce = 10f; // Force of each jump
    public int maxStoredJumps = 3; // Maximum stored jumps
    public float groundCheckDistance = 0.2f; // Distance for ground detection
    public LayerMask groundLayer; // Set this to "Ground" layer in Inspector

    private Rigidbody rb;
    private int storedJumps = 0;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        CheckGround();

        if (isGrounded)
        {
            storedJumps = maxStoredJumps; // Reset stored jumps when landing
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PerformJump();
        }
    }

    void CheckGround()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
    }

    void PerformJump()
    {
        if (isGrounded || storedJumps > 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // Reset vertical velocity
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            if (!isGrounded)
            {
                storedJumps--; // Consume a stored jump
            }
        }
    }
}
