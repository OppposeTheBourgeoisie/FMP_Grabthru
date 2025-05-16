using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;  // Reference to the player's transform
    [SerializeField] private float XSens = 10f;  // Mouse X sensitivity
    [SerializeField] private float YSens = 10f;  // Mouse Y sensitivity

    private Vector3 offset; // Offset between the camera and player
    private float XRotation = 0f;  // Rotation on X-axis (up/down, pitch)
    private float YRotation = 0f;  // Rotation on Y-axis (left/right, yaw)

    private void Start()
    {
        // Calculate the initial offset between the camera and player (at head level)
        offset = transform.position - target.position;

        // Lock the cursor in the middle of the screen and hide it for first-person control
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Get mouse input for rotation
        float mouseX = Input.GetAxisRaw("Mouse X") * XSens * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * YSens * Time.deltaTime;

        // Update Y-axis rotation (for horizontal camera movement)
        YRotation += mouseX;

        // Update X-axis rotation (for vertical camera movement)
        XRotation -= mouseY;
        XRotation = Mathf.Clamp(XRotation, -90f, 90f);  // Prevent flipping upside down

        // Apply rotation to the camera
        transform.rotation = Quaternion.Euler(XRotation, YRotation, 0f);

        // Update player body rotation (yaw) to follow camera's horizontal movement
        target.rotation = Quaternion.Euler(0f, YRotation, 0f);
    }

    private void LateUpdate()
    {
        // Keep the camera at the fixed offset from the player (at head level)
        transform.position = target.position + offset;
    }
}
