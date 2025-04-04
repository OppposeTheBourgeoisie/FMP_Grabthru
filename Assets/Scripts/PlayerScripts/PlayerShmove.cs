﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShmove : MonoBehaviour
{
    public Transform Orientation;

    public float MoveSpeed;
    public float Deceleration;
    float HorizontalInput;
    float VerticalInput;
    public float Speed;
    public float MaxSpeed;
    private float FallMultiplier = 2.5f;
    private float RiseMultiplier = 2f;

    Vector3 MoveDirection;
    Vector3 OldPosition;

    Rigidbody Rb;

    //Triple Jump variables
    private int MaxJumps = 3;
    private int CurrentJumps;

    private void Start()
    {
        Rb = GetComponent<Rigidbody>();
        Rb.freezeRotation = true;
        Rb.useGravity = true;
    }

    private void Update()
    {
        MyInput();
        StopPlayer();

        //Movement speed clamp
        Vector3 velocity = Rb.velocity;

        float maxStrafeSpeed = MaxSpeed; // or use separate values if you want
        float maxForwardSpeed = MaxSpeed;

        Vector3 localVel = transform.InverseTransformDirection(velocity);
        localVel.x = Mathf.Clamp(localVel.x, -maxStrafeSpeed, maxStrafeSpeed);
        localVel.z = Mathf.Clamp(localVel.z, -maxForwardSpeed, maxForwardSpeed);

        Rb.velocity = transform.TransformDirection(localVel);
        Rb.velocity = new Vector3(Rb.velocity.x, velocity.y, Rb.velocity.z); // retain vertical velocity
    }

    private void MyInput()
    {
        //Take player input
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        VerticalInput = Input.GetAxisRaw("Vertical");

        //Jumping logic
        if (Input.GetButtonDown("Jump") && CurrentJumps > 0)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        //Apply movement and custom gravity
        MovePlayer();
        ApplyCustomGravity();
        Speed = Vector3.Distance(OldPosition, transform.position) * 100f;
        OldPosition = transform.position;
    }

    private void MovePlayer()
    {
        //Calculate movement
        MoveDirection = Orientation.forward * VerticalInput + Orientation.right * HorizontalInput;

        // Apply directional acceleration without normalizing
        Vector3 movementForce = MoveDirection * MoveSpeed * 5f;
        Rb.AddForce(movementForce, ForceMode.Acceleration);
    }

    private void StopPlayer()
    {
        //Check if there's any movement input
        Vector3 currentVelocity = Rb.velocity;
        float inputDot = Vector3.Dot(MoveDirection.normalized, currentVelocity.normalized);

        //Decelerate if no input is given or in the opposite direction
        if (HorizontalInput == 0 && VerticalInput == 0)
        {
            currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, 0, Deceleration * Time.fixedDeltaTime);
            currentVelocity.z = Mathf.MoveTowards(currentVelocity.z, 0, Deceleration * Time.fixedDeltaTime);
        }
        else if (inputDot < 0)
        {
            float currentDeceleration = Deceleration * 1.5f;
            currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, 0, currentDeceleration * Time.fixedDeltaTime);
            currentVelocity.z = Mathf.MoveTowards(currentVelocity.z, 0, currentDeceleration * Time.fixedDeltaTime);
        }

        Rb.velocity = currentVelocity;
    }

    private void ApplyCustomGravity()
    {
        //Apply custom gravity so the player falls more naturally
        if (Rb.velocity.y < 0)
        {
            Rb.AddForce(Vector3.down * (Mathf.Abs(Physics.gravity.y) * (FallMultiplier - 1)), ForceMode.Acceleration);
        }
        else if (Rb.velocity.y > 0)
        {
            Rb.AddForce(Vector3.down * (Mathf.Abs(Physics.gravity.y) * (RiseMultiplier - 1)), ForceMode.Acceleration);
        }
    }

    private void Jump()
    {
        //Triple jump logic
        Rb.velocity = new Vector3(Rb.velocity.x, 0, Rb.velocity.z);

        Rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);
        CurrentJumps--;
    }

    public void AddJumps(int extraJumps)
    {
        //Add jumps to the player
        CurrentJumps = Mathf.Min(CurrentJumps + extraJumps, MaxJumps);
    }

    public int GetCurrentJumps()
    {
        //Update jumps the player has left
        return CurrentJumps;
    }
}
