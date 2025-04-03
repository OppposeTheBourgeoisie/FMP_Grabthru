using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShmove : MonoBehaviour
{
    public Transform orientation;

    public float MoveSpeed;
    public float Deceleration;
    float HorizontalInput;
    float VerticalInput;
    public float speed;
    public float MaxSpeed;
    private float FallMultiplier = 2.5f;
    private float RiseMultiplier = 2f;

    Vector3 MoveDirection;
    Vector3 oldPosition;

    Rigidbody rb;

    // Triple Jump variables
    private int maxJumps = 3;  // Max number of jumps
    private int currentJumps;  // Current number of available jumps

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = true;
    }

    private void Update()
    {
        MyInput();
        StopPlayer();
        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -MaxSpeed, MaxSpeed), rb.velocity.y, Mathf.Clamp(rb.velocity.z, -MaxSpeed, MaxSpeed));
    }

    private void MyInput()
    {
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        VerticalInput = Input.GetAxisRaw("Vertical");

        // Jumping logic
        if (Input.GetButtonDown("Jump") && currentJumps > 0)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
        ApplyCustomGravity();
        speed = Vector3.Distance(oldPosition, transform.position) * 100f;
        oldPosition = transform.position;
    }

    private void MovePlayer()
    {
        MoveDirection = orientation.forward * VerticalInput + orientation.right * HorizontalInput;

        // Reduce force to slow acceleration
        rb.AddForce(MoveDirection.normalized * MoveSpeed * 5f, ForceMode.Force);
    }

    private void StopPlayer()
    {
        if (HorizontalInput == 0 && VerticalInput == 0)
        {
            Vector3 currentVelocity = rb.velocity;
            currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, 0, Deceleration * Time.fixedDeltaTime);
            currentVelocity.z = Mathf.MoveTowards(currentVelocity.z, 0, Deceleration * Time.fixedDeltaTime);
            rb.velocity = currentVelocity;
        }
    }

    private void ApplyCustomGravity()
    {
        if (rb.velocity.y < 0) // Falling
        {
            rb.AddForce(Vector3.down * (Mathf.Abs(Physics.gravity.y) * (FallMultiplier - 1)), ForceMode.Acceleration);
        }
        else if (rb.velocity.y > 0) // Moving up
        {
            rb.AddForce(Vector3.down * (Mathf.Abs(Physics.gravity.y) * (RiseMultiplier - 1)), ForceMode.Acceleration);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);  // Reset y velocity to prevent stacking jumps

        rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);  // Add jump force
        currentJumps--;  // Decrement the available jumps
    }

    private void OnCollisionEnter(Collision other)
    {
        // Check if the player is grounded and only reset jump counter when they land if you want to
        if (other.gameObject.CompareTag("Ground"))
        {
            // Do nothing, don't reset jumps
        }
    }

    // Method to be called when the player picks up an item (e.g., triple jump power-up)
    public void AddJumps(int extraJumps)
    {
        currentJumps = Mathf.Min(currentJumps + extraJumps, maxJumps);  // Add jumps but cap at maxJumps
    }

    public int GetCurrentJumps()
    {
        return currentJumps;  // This returns the current jump count
    }
}
