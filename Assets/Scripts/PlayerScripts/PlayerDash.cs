using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDash : MonoBehaviour
{
    public float DashSpeed = 15f;
    public float DashDuration = 0.3f;
    public float DashCooldown = 3f;
    private bool IsDashing = false;
    private float DashTimer = 0f;
    private Vector3 DashDirection;

    private bool CanDash = true;
    private float CooldownTimer = 0f;

    private Rigidbody Rb;
    public Image DashCooldownBar;

    public Transform Orientation;

    private void Start()
    {
        Rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && CanDash && !IsDashing)
        {
            StartDash();
        }

        if (!CanDash)
        {
            // When the dash is on cooldown, decrease the cooldown timer
            CooldownTimer -= Time.deltaTime;
            if (CooldownTimer <= 0)
            {
                CanDash = true;
            }

            if (DashCooldownBar != null)
            {
                DashCooldownBar.fillAmount = 1 - (CooldownTimer / DashCooldown);
            }
        }
    }

    private void FixedUpdate()
    {
        if (IsDashing)
        {
            DashMovement();
        }
    }

    private void StartDash()
    {
        // Start the dash and set the dash direction and timer
        IsDashing = true;
        DashTimer = DashDuration;

        float HorizontalInput = Input.GetAxisRaw("Horizontal");
        float VerticalInput = Input.GetAxisRaw("Vertical");

        // Dash in the direction of the input
        if (HorizontalInput == 0 && VerticalInput == 0)
        {
            // Dash forward if no input is given
            DashDirection = Orientation.forward;
        }
        else
        {
            DashDirection = Orientation.forward * VerticalInput + Orientation.right * HorizontalInput;
        }

        // Temporarily disable gravity to allow for dash movement
        Rb.useGravity = false;

        // Prevent dashing until the cooldown is over
        CanDash = false;
        CooldownTimer = DashCooldown;
    }

    private void DashMovement()
    {
        // Dash in the specified direction
        if (DashTimer > 0)
        {
            Rb.velocity = new Vector3(DashDirection.x * DashSpeed, Rb.velocity.y, DashDirection.z * DashSpeed);
        }

        DashTimer -= Time.fixedDeltaTime;

        if (DashTimer <= 0)
        {
            // Set the dash to false and re-enable gravity
            Rb.useGravity = true;
            IsDashing = false;
        }
    }
}
