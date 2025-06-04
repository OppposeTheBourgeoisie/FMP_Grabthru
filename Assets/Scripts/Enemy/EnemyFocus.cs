using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFocus : MonoBehaviour
{
    public Transform target;

    private void Update()
    {
        // Makes the enemy always face the target
        Vector3 relativePos = target.position - transform.position;

        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = rotation;
    }
}
