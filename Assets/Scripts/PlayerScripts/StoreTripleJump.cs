using UnityEngine;

public class StoreTripleJump : MonoBehaviour
{
    public float JumpForce = 10f;
    public int MaxStoredJumps = 3;
    public float GroundCheckDistance = 0.2f;
    public LayerMask GroundLayer;

    private Rigidbody Rb;
    private int StoredJumps = 0;
    private bool IsGrounded;

    void Start()
    {
        Rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        CheckGround();

        if (IsGrounded)
        {
            StoredJumps = MaxStoredJumps;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PerformJump();
        }
    }

    void CheckGround()
    {
        IsGrounded = Physics.Raycast(transform.position, Vector3.down, GroundCheckDistance, GroundLayer);
    }

    void PerformJump()
    {
        // Check if the player is grounded or has stored jumps available
        if (IsGrounded || StoredJumps > 0)
        {
            Rb.velocity = new Vector3(Rb.velocity.x, 0, Rb.velocity.z);
            Rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);

            if (!IsGrounded)
            {
                StoredJumps--;
            }
        }
    }
}
