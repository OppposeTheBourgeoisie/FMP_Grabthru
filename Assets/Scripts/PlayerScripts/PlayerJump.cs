using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    public float jumpForce = 2.0f;

    private Vector3 jump;
    private bool isGrounded;
    private Rigidbody rb;
    private PlayerInputActions inputActions;

    void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Jump.performed += OnJump;
    }

    void OnEnable() => inputActions.Enable();
    void OnDisable() => inputActions.Disable();

    void Start()
    {
        // Get Rigidbody and set jump vector
        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 2.0f, 0.0f);
    }

    void OnCollisionStay()
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        // Unset grounded state when leaving ground
        isGrounded = false;
    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        // Perform jump if grounded
        if (isGrounded)
        {
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
        }
    }
}
