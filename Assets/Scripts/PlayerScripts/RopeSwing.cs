using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSwing : MonoBehaviour
{
    public KeyCode SwingKey = KeyCode.Mouse0;

    public LineRenderer Lr;
    public Transform GunTip, Cam, Player;
    public LayerMask WhatIsGrappleable;

    private float MaxSwingDistance = 25f;
    private Vector3 SwingPoint;
    private SpringJoint Joint;
    private Vector3 CurrentGrapplePosition;

    public RaycastHit PredictionHit;
    public float PredictionSphereCastRadius;
    public Transform PredictionPoint;

    public bool AbleToGrapple = false;

    void Update()
    {
        if (Input.GetKeyDown(SwingKey)) StartSwing();
        if (Input.GetKeyUp(SwingKey)) StopSwing();

        CheckForSwingPoints();
    }

    void StartSwing()
    {
        //If there is a valid swing point, start swinging
        if (PredictionHit.point == Vector3.zero) return;

        RaycastHit hit;

        if (Physics.Raycast(Cam.position, Cam.forward, out hit, MaxSwingDistance, WhatIsGrappleable))
        {
            SwingPoint = hit.point;
        }
        else if (Physics.SphereCast(Cam.position, PredictionSphereCastRadius, Cam.forward, out hit, MaxSwingDistance, WhatIsGrappleable))
        {
            SwingPoint = hit.point;
        }
        else
        {
            return;
        }

        //Create the spring joint for swinging
        Joint = Player.gameObject.AddComponent<SpringJoint>();
        Joint.autoConfigureConnectedAnchor = false;
        Joint.connectedAnchor = SwingPoint;

        float DistanceFromPoint = Vector3.Distance(Player.position, SwingPoint);
        Joint.maxDistance = DistanceFromPoint * 0.8f;
        Joint.minDistance = DistanceFromPoint * 0.25f;

        Joint.spring = 4.5f;
        Joint.damper = 7f;
        Joint.massScale = 4.5f;

        //Use line renderer to visualize the rope
        Lr.positionCount = 2;
        CurrentGrapplePosition = GunTip.position;
    }

    void StopSwing()
    {
        //When the player stops swinging, remove the spring joint and line renderer
        //Also release the player at the current velocity
        Lr.positionCount = 0;
        if (Joint != null)
        {
            Vector3 releaseVelocity = Player.GetComponent<Rigidbody>().velocity;

            float maxReleaseSpeed = 20f;
            if (releaseVelocity.magnitude > maxReleaseSpeed)
            {
                releaseVelocity = releaseVelocity.normalized * maxReleaseSpeed;
            }

            Player.GetComponent<Rigidbody>().velocity = releaseVelocity;

            Destroy(Joint);
        }
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    void DrawRope()
    {
        if (Joint == null) return;

        CurrentGrapplePosition = Vector3.Lerp(CurrentGrapplePosition, SwingPoint, Time.deltaTime * 8f);

        Lr.SetPosition(0, GunTip.position);
        Lr.SetPosition(1, SwingPoint);
    }

    void CheckForSwingPoints()
    {
        //Check if the player is swinging or if the prediction point is null
        if (Joint != null || PredictionPoint == null) return;

        //Check if the player is able to grapple
        bool wasActive = PredictionPoint.gameObject.activeSelf;
        PredictionPoint.gameObject.SetActive(false);

        //SphereCast to find the grapple point
        RaycastHit sphereCastHit;
        bool sphereHit = Physics.SphereCast(Cam.position, PredictionSphereCastRadius, Cam.forward, out sphereCastHit, MaxSwingDistance, WhatIsGrappleable);

        //raycast to find the grapple point
        RaycastHit raycastHit;
        bool rayHit = Physics.Raycast(Cam.position, Cam.forward, out raycastHit, MaxSwingDistance, WhatIsGrappleable);

        Vector3 newHitPoint = Vector3.zero;

        //Raycast if available, otherwise use SphereCast
        if (rayHit)
            newHitPoint = raycastHit.point;
        else if (sphereHit)
            newHitPoint = sphereCastHit.point;

        //Bring back prediction point
        PredictionPoint.gameObject.SetActive(wasActive);

        if (newHitPoint != Vector3.zero)
        {
            PredictionPoint.gameObject.SetActive(true);
            PredictionPoint.position = newHitPoint;
            PredictionHit = rayHit ? raycastHit : sphereCastHit;
        }
        else
        {
            PredictionPoint.gameObject.SetActive(false);
            PredictionHit = new RaycastHit();
        }
    }
}
