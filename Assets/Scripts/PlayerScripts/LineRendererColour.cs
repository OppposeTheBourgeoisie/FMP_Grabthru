using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererColour : MonoBehaviour
{
    public Color StartColor = Color.green;
    public Color EndColor = Color.red;

    private LineRenderer Lr;

    void Start()
    {
        // Get the LineRenderer component and set its properties
        Lr = GetComponent<LineRenderer>();
        Lr.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        Vector3[] Positions = new Vector3[3];
        Positions[0] = new Vector3(-2.0f, -2.0f, 0.0f);
        Positions[1] = new Vector3(0.0f, 2.0f, 0.0f);
        Positions[2] = new Vector3(2.0f, -2.0f, 0.0f);
        Lr.positionCount = Positions.Length;
        Lr.SetPositions(Positions);
        float Alpha = 1.0f;
        
        // Set the color gradient
        Gradient Gradient = new Gradient();
        Gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(StartColor, 0.0f), new GradientColorKey(EndColor, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(Alpha, 0.0f), new GradientAlphaKey(Alpha, 1.0f) }
        );
        Lr.colorGradient = Gradient;
    }
}
