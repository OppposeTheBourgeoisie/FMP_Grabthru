using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSwing : MonoBehaviour
{
    public KeyCode SwingKey = KeyCode.Mouse0;

    public LineRenderer lr;
    public Transform GunTip, cam, player;
    public LayerMask WhatIsGrappleable;

    private float MaxSwingDistance = 25f;
    private Vector3 SwingPoint;
    private SpringJoint joint;
    private Vector3 CurrentGrapplePosition;

    public RaycastHit PredictionHit;
    public float PredictionSphereCastRadius;
    public Transform PredictionPoint;

    public bool AbleToGrapple = false;

    //blah

    void Update()
    {
        if (Input.GetKeyDown(SwingKey)) StartSwing();
        if (Input.GetKeyUp(SwingKey)) StopSwing();

        CheckForSwingPoints();
    }

    void StartSwing()
    {

        if (PredictionHit.point == Vector3.zero) return;

        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, MaxSwingDistance, WhatIsGrappleable))
        {
            SwingPoint = hit.point;
        }

        else if (Physics.SphereCast(cam.position, PredictionSphereCastRadius, cam.forward, out hit, MaxSwingDistance, WhatIsGrappleable))
        {
            SwingPoint = hit.point;
        }
        else
        {
            return;
        }

        joint = player.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = SwingPoint;

        float DistanceFromPoint = Vector3.Distance(player.position, SwingPoint);
        joint.maxDistance = DistanceFromPoint * 0.8f;
        joint.minDistance = DistanceFromPoint * 0.25f;

        joint.spring = 4.5f;
        joint.damper = 7f;
        joint.massScale = 4.5f;

        lr.positionCount = 2;
        CurrentGrapplePosition = GunTip.position;
    }

    void StopSwing()
    {
        lr.positionCount = 0;
        if (joint != null)
        {
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
        if (joint != null || PredictionPoint == null) return;

        bool wasActive = PredictionPoint.gameObject.activeSelf;
        PredictionPoint.gameObject.SetActive(false);

        RaycastHit sphereCastHit;
        bool sphereHit = Physics.SphereCast(cam.position, PredictionSphereCastRadius, cam.forward, out sphereCastHit, MaxSwingDistance, WhatIsGrappleable);

        RaycastHit raycastHit;
        bool rayHit = Physics.Raycast(cam.position, cam.forward, out raycastHit, MaxSwingDistance, WhatIsGrappleable);

        Vector3 newHitPoint = Vector3.zero;

        if (rayHit)
            newHitPoint = raycastHit.point;
        else if (sphereHit)
            newHitPoint = sphereCastHit.point;

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
