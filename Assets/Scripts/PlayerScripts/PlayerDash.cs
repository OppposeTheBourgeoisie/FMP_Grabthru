using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem; // Add this for the new Input System

public class PlayerDash : MonoBehaviour
{
    public float DashSpeed;
    public float DashCooldown = 3f;

    private Rigidbody rb;
    private bool isDashing;
    private bool canDash = true;
    private float cooldownTimer;

    public Image dashCooldownBar;

    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private bool dashPressed;

    public ParticleSystem dashEffect;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        inputActions.Player.Dash.performed += ctx => dashPressed = true; // You need to add a "Dash" action in your input actions
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cooldownTimer = 0;
    }

    private void Update()
    {
        // Dash input (using new input system)
        if (dashPressed && canDash)
        {
            isDashing = true;
            canDash = false;
            cooldownTimer = DashCooldown;
            StartCoroutine(DashCooldownRoutine());

            if (dashEffect != null)
            {
                dashEffect.Play();
                StartCoroutine(StopDashEffectAfterTime(0.2f)); // Adjust duration as needed
            }
        }
        dashPressed = false; // Reset flag

        // Update UI Cooldown
        if (!canDash)
        {
            cooldownTimer -= Time.deltaTime;
            dashCooldownBar.fillAmount = 1 - (cooldownTimer / DashCooldown);
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
            Dashing();
    }

    private void Dashing()
    {
        Vector3 inputDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        if (inputDirection.magnitude > 0)
        {
            Vector3 dashDirection = (transform.forward * moveInput.y + transform.right * moveInput.x).normalized;
            rb.AddForce(dashDirection * DashSpeed, ForceMode.Impulse);
        }

        isDashing = false;
    }

    private IEnumerator DashCooldownRoutine()
    {
        yield return new WaitForSeconds(DashCooldown);
        canDash = true;
        dashCooldownBar.fillAmount = 0;
    }

    private IEnumerator StopDashEffectAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (dashEffect != null)
        {
            dashEffect.Stop();
        }
    }
}
