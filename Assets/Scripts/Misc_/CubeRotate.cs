using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRotate : MonoBehaviour
{
    float XSpin;
    float YSpin;
    float ZSpin;

    private void Start()
    {
        //Initialize the spin values
        XSpin = Random.Range(5f, 30f);
        YSpin = Random.Range(5f, 30f);
        ZSpin = Random.Range(5f, 30f);
    }

    private void Update()
    {
        //Rotate the cube randomly according to the spin values
        transform.Rotate(XSpin * Time.deltaTime, YSpin * Time.deltaTime, ZSpin * Time.deltaTime);
    }
}
