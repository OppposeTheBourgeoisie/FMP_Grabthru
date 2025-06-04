using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRotate : MonoBehaviour
{
    float xSpin;
    float ySpin;
    float zSpin;

    private void Start()
    {
        // Initialize random rotation speeds for each axis
        xSpin = Random.Range(5f, 30f);
        ySpin = Random.Range(5f, 30f);
        zSpin = Random.Range(5f, 30f);
    }

    private void Update()
    {
        // Rotate the cube based on the random speeds
        transform.Rotate(xSpin * Time.deltaTime, ySpin * Time.deltaTime, zSpin * Time.deltaTime);
    }
}
