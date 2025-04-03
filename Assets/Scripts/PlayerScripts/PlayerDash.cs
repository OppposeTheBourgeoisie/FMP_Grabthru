using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Make sure this is included for UI Image
using TMPro; // If using TextMeshPro for countdown text

public class PlayerDash : MonoBehaviour
{
    public float DashSpeed;
    public float DashCooldown = 3f;
    
    private Rigidbody rb;
    private bool isDashing;
    private bool canDash = true;
    private float cooldownTimer;

    public Image dashCooldownBar; // Reference to the Image (your circle)

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cooldownTimer = 0;
    }

    private void Update()
    {
        // Dash input (mouse button 1 in this case)
        if (Input.GetMouseButtonDown(1) && canDash)
        {
            isDashing = true;
            canDash = false;
            cooldownTimer = DashCooldown;
            StartCoroutine(DashCooldownRoutine());
        }

        // Update UI Cooldown
        if (!canDash)
        {
            cooldownTimer -= Time.deltaTime;
            dashCooldownBar.fillAmount = 1 - (cooldownTimer / DashCooldown); // Update circle fill
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
            Dashing();
    }

    private void Dashing()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 inputDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

        if (inputDirection.magnitude > 0)
        {
            Vector3 dashDirection = (transform.forward * verticalInput + transform.right * horizontalInput).normalized;
            rb.AddForce(dashDirection * DashSpeed, ForceMode.Impulse);
        }

        isDashing = false;
    }

    private IEnumerator DashCooldownRoutine()
    {
        yield return new WaitForSeconds(DashCooldown); // Wait until cooldown is over
        canDash = true;
        dashCooldownBar.fillAmount = 0; // Reset circle to empty
    }
}
