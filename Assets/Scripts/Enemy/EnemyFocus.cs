using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFocus : MonoBehaviour
{
    public Transform target;

    private void Update()
    {
        Vector3 relativePos = target.position - transform.position;

        // the second argument, upwards, defaults to Vector3.up
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = rotation;
    }
}
