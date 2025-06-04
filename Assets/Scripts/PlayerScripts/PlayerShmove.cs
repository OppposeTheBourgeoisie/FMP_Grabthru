using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShmove : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;

    [Header("Movement Settings")]
    public float MoveSpeed;
    public float Deceleration;
    public float MaxSpeed;
    public float airControlMultiplier = 0.3f;

    [Header("Jump Settings")]
    public int maxJumps = 3;
    private int currentJumps;

    public float speed;
    private Vector2 moveInput;
    private bool jumpPressed = false;
    private Vector3 MoveDirection;
    private Vector3 oldPosition;
    private Rigidbody rb;
    private bool isGrounded;

    private PlayerInputActions inputActions;

    private void Awake()
    {
        // Setup input actions and references
        inputActions = new PlayerInputActions();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        inputActions.Player.Jump.performed += ctx => jumpPressed = true;
    }

    private void OnEnable() => inputActions.Enable();

    private void OnDisable() => inputActions.Disable();

    private void Start()
    {
        // Initialize rigidbody and state
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = true;
        rb.drag = 0f;
        oldPosition = transform.position;
    }

    private void Update()
    {
        // Handle jump input
        if (jumpPressed && currentJumps > 0)
        {
            Jump();
            jumpPressed = false;
        }
    }

    private void FixedUpdate()
    {
        // Handle movement and custom gravity
        if (moveInput == Vector2.zero)
            StopPlayer();
        else
            MovePlayer();

        ApplyCustomGravity();

        speed = Vector3.Distance(oldPosition, transform.position) * 100f;
        oldPosition = transform.position;
    }

    private void MovePlayer()
    {
        // Move the player and clamp horizontal speed
        MoveDirection = orientation.forward * moveInput.y + orientation.right * moveInput.x;
        float control = isGrounded ? 1f : airControlMultiplier;
        rb.AddForce(MoveDirection.normalized * MoveSpeed * control, ForceMode.VelocityChange);

        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (horizontalVelocity.magnitude > MaxSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * MaxSpeed;
            rb.velocity = new Vector3(horizontalVelocity.x, rb.velocity.y, horizontalVelocity.z);
        }
    }

    private void StopPlayer()
    {
        // Apply strong counter-force to stop the player quickly when grounded
        if (isGrounded)
        {
            Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            float stopStrength = 10f;
            rb.AddForce(-horizontalVelocity * stopStrength, ForceMode.Acceleration);
            if (horizontalVelocity.magnitude < 0.2f)
            {
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
            }
        }
    }

    private void ApplyCustomGravity()
    {
        // Apply custom gravity for better jump/fall feel
        if (rb.velocity.y < 0)
            rb.AddForce(Vector3.down * (Mathf.Abs(Physics.gravity.y) * (2.5f - 1)), ForceMode.Acceleration);
        else if (rb.velocity.y > 0)
            rb.AddForce(Vector3.down * (Mathf.Abs(Physics.gravity.y) * (2f - 1)), ForceMode.Acceleration);
    }

    private void Jump()
    {
        // Handle jumping and reset vertical velocity
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);
        currentJumps--;
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
        jumpPressed = false;
    }

    public int GetCurrentJumps()
    {
        return currentJumps;
    }
}
