using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private Vector3 Jump;
    public float JumpForce = 2.0f;

    private bool IsGrounded;
    Rigidbody Rb;

    void Start()
    {
        Rb = GetComponent<Rigidbody>();
        Jump = new Vector3(0.0f, 2.0f, 0.0f);
    }

    void OnCollisionStay()
    {
        IsGrounded = true;
    }

    private void OnCollisionExit(Collision Collision)
    {
        IsGrounded = false;
    }

    void Update()
    {
        //Jump when the space key is pressed and the player is grounded
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {
            Rb.AddForce(Jump * JumpForce, ForceMode.Impulse);
        }
    }
}
