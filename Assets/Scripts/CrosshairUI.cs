using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrosshairUI : MonoBehaviour
{
    RopeSwing ropeSwing;

    // Update is called once per frame
    void Update()
    {
        if (ropeSwing.AbleToGrapple)
        {
            GameObject.Find("GrappleableCrosshair").SetActive(true);
            GameObject.Find("Crosshair").SetActive(false);
        }
        else
        {
            GameObject.Find("GrappleableCrosshair").SetActive(false);
            GameObject.Find("Crosshair").SetActive(false);
        }
    }
}
