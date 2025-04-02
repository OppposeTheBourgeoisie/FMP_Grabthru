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
        xSpin = Random.Range(5f, 30f);
        ySpin = Random.Range(5f, 30f);
        zSpin = Random.Range(5f, 30f);
    }

    private void Update()
    {
        transform.Rotate(xSpin * Time.deltaTime, ySpin * Time.deltaTime, zSpin * Time.deltaTime);
    }
}
