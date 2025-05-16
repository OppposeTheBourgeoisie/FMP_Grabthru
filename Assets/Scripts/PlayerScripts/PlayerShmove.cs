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

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = true;

        inputActions = new PlayerInputActions();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Player.Jump.performed += ctx => jumpPressed = true;
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

        StopPlayer();

        rb.velocity = new Vector3(
            Mathf.Clamp(rb.velocity.x, -MaxSpeed, MaxSpeed),
            rb.velocity.y,
            Mathf.Clamp(rb.velocity.z, -MaxSpeed, MaxSpeed)
        );
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
        MoveDirection = orientation.forward * moveInput.y + orientation.right * moveInput.x;
        rb.AddForce(MoveDirection.normalized * MoveSpeed * 5f, ForceMode.Force);
    }

    private void StopPlayer()
    {
        if (moveInput == Vector2.zero)
        {
            Vector3 currentVelocity = rb.velocity;
            currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, 0, Deceleration * Time.fixedDeltaTime);
            currentVelocity.z = Mathf.MoveTowards(currentVelocity.z, 0, Deceleration * Time.fixedDeltaTime);
            rb.velocity = currentVelocity;
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

    public void AddJumps(int extraJumps)
    {
        currentJumps = Mathf.Min(currentJumps + extraJumps, maxJumps);
    }

    public int GetCurrentJumps()
    {
        return currentJumps;
    }
}
