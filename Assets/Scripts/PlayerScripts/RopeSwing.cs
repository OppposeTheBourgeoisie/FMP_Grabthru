using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RopeSwing : MonoBehaviour
{
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

    private PlayerInputActions inputActions;
    private bool swingPressed;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Swing.performed += ctx => swingPressed = true;
        inputActions.Player.Swing.canceled += ctx => swingPressed = false;
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    void Update()
    {
        if (swingPressed && joint == null) StartSwing();
        if (!swingPressed && joint != null) StopSwing();

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
            Vector3 releaseVelocity = player.GetComponent<Rigidbody>().velocity;
            float maxReleaseSpeed = 20f;
            if (releaseVelocity.magnitude > maxReleaseSpeed)
            {
                releaseVelocity = releaseVelocity.normalized * maxReleaseSpeed;
            }
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
