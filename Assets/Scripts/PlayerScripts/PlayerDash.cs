using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{
    [Header("Dash Settings")]
    public float DashSpeed;
    public float DashCooldown = 3f;

    [Header("References")]
    public Image dashCooldownBar;
    public ParticleSystem dashEffect;

    private Rigidbody rb;
    private bool isDashing;
    private bool canDash = true;
    private float cooldownTimer;
    private Vector2 moveInput;
    private bool dashPressed;
    private Vector2 lastMoveInput;

    private PlayerInputActions inputActions;

    private void Awake()
    {
        // References and input actions setup
        inputActions = new PlayerInputActions();
        inputActions.Player.Move.performed += ctx => {
            moveInput = ctx.ReadValue<Vector2>();
            if (moveInput != Vector2.zero)
                lastMoveInput = moveInput;
        };
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        inputActions.Player.Dash.performed += ctx => dashPressed = true;
    }

    private void OnEnable() => inputActions.Enable();

    private void OnDisable() => inputActions.Disable();

    private void Start()
    {
        // Get rigidbody and initialize state
        rb = GetComponent<Rigidbody>();
        cooldownTimer = 0;
        if (dashEffect != null)
            dashEffect.Stop();
    }

    private void Update()
    {
        // Update dash cooldown bar and check for dash input
        if (dashPressed && canDash)
        {
            isDashing = true;
            canDash = false;
            cooldownTimer = DashCooldown;
            StartCoroutine(DashCooldownRoutine());

            if (dashEffect != null)
            {
                dashEffect.Play();
                StartCoroutine(StopDashEffectAfterTime(0.2f));
            }
            AudioSystem.Instance.PlaySound("Dash");
        }
        dashPressed = false;

        if (!canDash)
        {
            cooldownTimer -= Time.deltaTime;
            dashCooldownBar.fillAmount = 1 - (cooldownTimer / DashCooldown);
        }
    }

    private void FixedUpdate()
    {
        // Physics update for dashing
        if (isDashing)
            Dashing();
    }

    private void Dashing()
    {
        // Make the player dash in the direction of the last move input
        Vector2 dashInput = lastMoveInput != Vector2.zero ? lastMoveInput : new Vector2(0, 1);
        Vector3 dashDirection = (transform.forward * dashInput.y + transform.right * dashInput.x).normalized;
        rb.AddForce(dashDirection * DashSpeed, ForceMode.Impulse);
        isDashing = false;
    }

    private IEnumerator DashCooldownRoutine()
    {
        // Wait for the cooldown before the player can dash again
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
