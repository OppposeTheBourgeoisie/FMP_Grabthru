using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private float XSens = 10f;
    [SerializeField] private float YSens = 10f;

    private Vector3 offset;
    private float XRotation = 0f;
    private float YRotation = 0f;

    private PlayerInputActions inputActions;
    private Vector2 lookInput;

    private void Awake()
    {
        // Setup input actions for camera look
        inputActions = new PlayerInputActions();
        inputActions.Player.Camera.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Camera.canceled += ctx => lookInput = Vector2.zero;
    }

    private void OnEnable() => inputActions.Enable();

    private void OnDisable() => inputActions.Disable();

    private void Start()
    {
        // Calculate the initial offset and lock the cursor
        offset = transform.position - target.position;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Handle camera rotation based on input
        float mouseX = lookInput.x * XSens * Time.deltaTime;
        float mouseY = lookInput.y * YSens * Time.deltaTime;

        YRotation += mouseX;
        XRotation -= mouseY;
        XRotation = Mathf.Clamp(XRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(XRotation, YRotation, 0f);
        target.rotation = Quaternion.Euler(0f, YRotation, 0f);
    }

    private void LateUpdate()
    {
        // Keep the camera at the fixed offset from the player
        transform.position = target.position + offset;
    }
}
