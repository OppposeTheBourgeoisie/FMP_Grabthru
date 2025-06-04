using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RopeSwing : MonoBehaviour
{
    // Script referenced from https://www.youtube.com/watch?v=HPjuTK91MA8
    [Header("References")]
    public LineRenderer lr;
    public Transform GunTip, cam, player;
    public LayerMask WhatIsGrappleable;
    public AudioSource swingAudio;
    public ParticleSystem speedEffect;
    public Transform PredictionPoint;

    [Header("Swing Settings")]
    public float PredictionSphereCastRadius;
    public float minVolume = 0.1f;
    public float maxVolume = 1.0f;
    public float maxSpeedForVolume = 30f;
    public float speedEffectThreshold = 200f;

    private float MaxSwingDistance = 50f;
    private Vector3 SwingPoint;
    private SpringJoint joint;
    private Vector3 CurrentGrapplePosition;
    public RaycastHit PredictionHit;
    public bool AbleToGrapple = false;

    private PlayerInputActions inputActions;
    private PlayerShmove playerShmove;

    // Setup input actions and references
    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Swing.started += ctx => StartSwing();
        inputActions.Player.Swing.canceled += ctx => StopSwing();

        if (player != null)
            playerShmove = player.GetComponent<PlayerShmove>();

        if (speedEffect != null)
            speedEffect.Stop();
    }

    private void OnEnable() => inputActions.Enable();

    private void OnDisable() => inputActions.Disable();

    void Update()
    {
        CheckForSwingPoints();

        // Adjust swing sound volume based on player speed
        if (joint != null && swingAudio != null && playerShmove != null)
        {
            if (!swingAudio.isPlaying)
                swingAudio.Play();

            float speed01 = Mathf.Clamp01(playerShmove.speed / maxSpeedForVolume);
            swingAudio.volume = Mathf.Lerp(minVolume, maxVolume, speed01);
        }
        else if (swingAudio != null && swingAudio.isPlaying)
        {
            swingAudio.Stop();
        }

        // Play speed effect if player is fast enough
        if (playerShmove != null && speedEffect != null)
        {
            if (playerShmove.speed >= speedEffectThreshold)
            {
                if (!speedEffect.isPlaying)
                    speedEffect.Play();
            }
            else
            {
                if (speedEffect.isPlaying)
                    speedEffect.Stop();
            }
        }
    }

    void StartSwing()
    {
        // If the player can grapple, start the swing
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

        AudioSystem.Instance.PlaySound("RopeSwing");

        // Create a spring joint to simulate the swing
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
        // If the player is swinging, stop the swing and reset the line renderer
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
        // Use a line renderer to draw a rope
        if (joint == null) return;
        CurrentGrapplePosition = Vector3.Lerp(CurrentGrapplePosition, SwingPoint, Time.deltaTime * 8f);
        lr.SetPosition(0, GunTip.position);
        lr.SetPosition(1, SwingPoint);
    }

    void CheckForSwingPoints()
    {
        // Check if the player is able to grapple and predict the swing point
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
