using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private Vector3 offset;
    [SerializeField] private Transform target;
    private Vector3 CurrentVelocity = Vector3.zero;

    private void Awake()
    {
        offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        Vector3 TargetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, TargetPosition, ref CurrentVelocity, 0f);
    }
}
