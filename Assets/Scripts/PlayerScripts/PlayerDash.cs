using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    public float DashSpeed;
    private Rigidbody rb;
    private bool isDashing;
    public float DashCooldown;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
            isDashing = true;
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
}
