using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShmove : MonoBehaviour
{
    public Transform orientation;

    public float MoveSpeed;
    public float Deceleration;
    public float speed;
    public float MaxSpeed;
    private float FallMultiplier = 2.5f;
    private float RiseMultiplier = 2f;

    private Vector2 moveInput;
    private bool jumpPressed = false;

    Vector3 MoveDirection;
    Vector3 oldPosition;
    Rigidbody rb;

    private int maxJumps = 3;
    private int currentJumps;

    private PlayerInputActions inputActions;

    private bool isGrounded;

    public float airControlMultiplier = 0.3f; // Tune this value (0.2–0.5 is typical)

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Player.Jump.performed += ctx => jumpPressed = true;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = true;
        rb.drag = 0f; // Lower drag for more natural movement and falling
        oldPosition = transform.position;
        currentJumps = maxJumps;
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void Update()
    {
        if (jumpPressed && currentJumps > 0)
        {
            Jump();
            jumpPressed = false; // Reset flag
        }
    }

    private void FixedUpdate()
    {
        if (moveInput == Vector2.zero)
        {
            StopPlayer();
        }
        else
        {
            MovePlayer();
        }

        ApplyCustomGravity();

        speed = Vector3.Distance(oldPosition, transform.position) * 100f;
        oldPosition = transform.position;
    }

    private void MovePlayer()
    {
        MoveDirection = orientation.forward * moveInput.y + orientation.right * moveInput.x;
        float control = isGrounded ? 1f : airControlMultiplier;
        rb.AddForce(MoveDirection.normalized * MoveSpeed * control, ForceMode.VelocityChange);

        // Clamp horizontal speed
        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (horizontalVelocity.magnitude > MaxSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * MaxSpeed;
            rb.velocity = new Vector3(horizontalVelocity.x, rb.velocity.y, horizontalVelocity.z);
        }
    }

    private void StopPlayer()
    {
        if (isGrounded)
        {
            Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            float friction = Deceleration * 5f; // Increase multiplier for stronger stop
            horizontalVelocity = Vector3.MoveTowards(horizontalVelocity, Vector3.zero, friction * Time.fixedDeltaTime);
            rb.velocity = new Vector3(horizontalVelocity.x, rb.velocity.y, horizontalVelocity.z);
        }
    }

    private void ApplyCustomGravity()
    {
        if (rb.velocity.y < 0)
        {
            rb.AddForce(Vector3.down * (Mathf.Abs(Physics.gravity.y) * (FallMultiplier - 1)), ForceMode.Acceleration);
        }
        else if (rb.velocity.y > 0)
        {
            rb.AddForce(Vector3.down * (Mathf.Abs(Physics.gravity.y) * (RiseMultiplier - 1)), ForceMode.Acceleration);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);
        currentJumps--;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            // Do nothing
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }

    public void AddJumps(int extraJumps)
    {
        currentJumps = Mathf.Min(currentJumps + extraJumps, maxJumps);
    }

    public int GetCurrentJumps()
    {
        return currentJumps;
    }
}
