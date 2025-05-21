using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    private Vector3 jump;
    public float jumpForce = 2.0f;

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
        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 2.0f, 0.0f);
    }

    void OnCollisionStay()
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        if (isGrounded)
        {
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
        }
    }
}
