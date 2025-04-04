using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float XSens = 10f;
    [SerializeField] private float YSens = 10f;

    private Vector3 offset;
    private float XRotation = 0f;
    private float YRotation = 0f;

    private void Start()
    {
        //Set the camera to the player's head position
        offset = transform.position - target.position;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        //Update according to mouse movement
        float mouseX = Input.GetAxisRaw("Mouse X") * XSens * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * YSens * Time.deltaTime;

        YRotation += mouseX;

        XRotation -= mouseY;
        XRotation = Mathf.Clamp(XRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(XRotation, YRotation, 0f);

        //Update player rotation to match camera Y rotation
        target.rotation = Quaternion.Euler(0f, YRotation, 0f);
    }

    private void LateUpdate()
    {
        // Keep the camera at the fixed offset from the player
        transform.position = target.position + offset;
    }
}
