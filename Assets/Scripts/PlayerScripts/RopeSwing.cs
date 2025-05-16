using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSwing : MonoBehaviour
{
    public KeyCode SwingKey = KeyCode.Mouse0;

    public LineRenderer lr;
    public Transform GunTip, cam, player;
    public LayerMask WhatIsGrappleable;

    private float MaxSwingDistance = 50f;
    private Vector3 SwingPoint;
    private SpringJoint joint;
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
        // Ensure we have a valid PredictionHit from either Raycast or SphereCast
        if (PredictionHit.point == Vector3.zero) return;  // Prevent starting without a valid point

        RaycastHit hit;

        // Try to use Raycast first
        if (Physics.Raycast(cam.position, cam.forward, out hit, MaxSwingDistance, WhatIsGrappleable))
        {
            SwingPoint = hit.point;
        }
        // If Raycast didn't hit anything, use SphereCast
        else if (Physics.SphereCast(cam.position, PredictionSphereCastRadius, cam.forward, out hit, MaxSwingDistance, WhatIsGrappleable))
        {
            SwingPoint = hit.point; // Use the SphereCast hit point
        }
        else
        {
            return; // If neither hits, do not start the swing
        }

        // Create the spring joint for swinging
        joint = player.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = SwingPoint;

        float DistanceFromPoint = Vector3.Distance(player.position, SwingPoint);
        joint.maxDistance = DistanceFromPoint * 0.8f;
        joint.minDistance = DistanceFromPoint * 0.25f;

        joint.spring = 4.5f;
        joint.damper = 7f;
        joint.massScale = 4.5f;

        // Set up the LineRenderer to draw the rope
        lr.positionCount = 2;
        CurrentGrapplePosition = GunTip.position;
    }

    void StopSwing()
    {
        lr.positionCount = 0;
        if (joint != null)
        {
            // Capture current velocity before removing the joint
            Vector3 releaseVelocity = player.GetComponent<Rigidbody>().velocity;

            // Limit the release speed to prevent excessive launching
            float maxReleaseSpeed = 20f; // Adjust as needed
            if (releaseVelocity.magnitude > maxReleaseSpeed)
            {
                releaseVelocity = releaseVelocity.normalized * maxReleaseSpeed;
            }

            // Apply the limited velocity
            player.GetComponent<Rigidbody>().velocity = releaseVelocity;

            Destroy(joint);
        }
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    void DrawRope()
    {
        if (joint == null) return;

        CurrentGrapplePosition = Vector3.Lerp(CurrentGrapplePosition, SwingPoint, Time.deltaTime * 8f);

        lr.SetPosition(0, GunTip.position);
        lr.SetPosition(1, SwingPoint);
    }

    void CheckForSwingPoints()
    {
        if (joint != null || PredictionPoint == null) return; // Prevent errors

        // Temporarily disable PredictionPoint to prevent self-detection
        bool wasActive = PredictionPoint.gameObject.activeSelf;
        PredictionPoint.gameObject.SetActive(false);

        RaycastHit sphereCastHit;
        bool sphereHit = Physics.SphereCast(cam.position, PredictionSphereCastRadius, cam.forward, out sphereCastHit, MaxSwingDistance, WhatIsGrappleable);

        RaycastHit raycastHit;
        bool rayHit = Physics.Raycast(cam.position, cam.forward, out raycastHit, MaxSwingDistance, WhatIsGrappleable);

        Vector3 newHitPoint = Vector3.zero;

        // Choose the best valid hit point (raycast first, spherecast second)
        if (rayHit)
            newHitPoint = raycastHit.point;
        else if (sphereHit)
            newHitPoint = sphereCastHit.point;

        // Restore PredictionPoint visibility
        PredictionPoint.gameObject.SetActive(wasActive);

        if (newHitPoint != Vector3.zero)
        {
            PredictionPoint.gameObject.SetActive(true);
            PredictionPoint.position = newHitPoint;
            PredictionHit = rayHit ? raycastHit : sphereCastHit; // Store correct hit data
        }
        else
        {
            PredictionPoint.gameObject.SetActive(false);
            PredictionHit = new RaycastHit(); // Reset PredictionHit when nothing is found
        }
    }
}