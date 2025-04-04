using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextLookAt : MonoBehaviour
{
    public Transform Target;

    void Update()
    {
        //Make the text look at the target
        transform.LookAt(Target);
    }
}
