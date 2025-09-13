
using System;
using UnityEngine;

public class PlayerLocomotion: MonoBehaviour
{
    [Header("REFERENCES")]
    public Transform orientation;
    public Rigidbody rb;
    public LayerMask groundLayer;


    [Header("STATUS")]
    public Vector2 inputDir;
    public Vector3 moveDirection;
    public float verticalVelocity = 0f;
    public Vector3 NewHorizontalVel;
    public Vector3 targetVel;
    public bool casthit = false;
    public bool stickToGround = false;


    [Header("GROUNDING")]
    public float groundRayDist;
    public float sphereRadius;
    public float MaxGroundStickDistance;
    public float groundOffset = .1f;
    public RaycastHit Surface;


    [Header("MOVEMENT")]
    public float MaxSpeed;
    public float acceleration;
    public float deceleration;
    public float turnSpeed;
    public float gravity;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        UpdateGroundingStatus();
        MovePlayer();
    }
    
    private void UpdateGroundingStatus()
    {
        // Get ground info
        casthit = Physics.SphereCast(transform.position + Vector3.up, sphereRadius, Vector3.down, out Surface, groundRayDist, groundLayer, QueryTriggerInteraction.Ignore);
        Surface = casthit? Surface : default;

        // Determine if we should stick to the ground
        stickToGround = false;
        if (casthit)
        {
            float dist = Mathf.Abs(transform.position.y + 1f - Surface.point.y);
            stickToGround = dist < MaxGroundStickDistance;
        }
    }

    Vector3 debugVector = Vector3.zero;
    private void MovePlayer()
    {
        // Calculate move direction based on input, surface and camera orientation
        moveDirection = (orientation.forward * inputDir.y + orientation.right * inputDir.x).normalized;
        Vector3 normal = Surface.collider != null ? Surface.normal : Vector3.up;
        Vector3 right = Vector3.Cross(Vector3.up, moveDirection).normalized;    // We only want to influence the vertical component, not the horizontal one
        Vector3 slopeForward = Vector3.Cross(right, normal).normalized;
        Vector3 SurfaceAppliedDir = slopeForward * moveDirection.magnitude;


        // Calculate Velocity
        Vector3 targetHorizontalVel = SurfaceAppliedDir * MaxSpeed;
        float appropriateAcceleration = moveDirection != Vector3.zero ? acceleration : deceleration;
        float rad = turnSpeed * Mathf.PI * Time.deltaTime;
        Vector3 currHorizontalVel = Vector3.ProjectOnPlane(rb.linearVelocity, normal);
        NewHorizontalVel = Vector3.RotateTowards(currHorizontalVel, targetHorizontalVel, rad, appropriateAcceleration * Time.deltaTime);
        targetVel = NewHorizontalVel;

        // Apply gravity, or keep the regular velocity
        if (!stickToGround) { verticalVelocity -= gravity * Time.deltaTime;}
        else
        {
            verticalVelocity = targetVel.y;

            // stick the player to the ground before applying the target velocity by moving them there
            Vector3 targetPos = new Vector3(transform.position.x, Surface.point.y + groundOffset, transform.position.z);
            transform.position = targetPos;
        }
        targetVel.y = verticalVelocity;


        // Before we move the player, check if we hit a wall, and if we do, change the velocity vector so that it slides along the wall
        Vector3 nextPos = transform.position + targetVel * Time.deltaTime;
        float dist = Vector3.Distance(transform.position, nextPos);
        if (rb.SweepTest(targetVel.normalized, out RaycastHit hit, dist))
        {
            // Only does this for walls, not slopes
            if (Vector3.Angle(Vector3.up, hit.normal) >= 90f)
            { 
                Vector3 WallSlideDir = Vector3.ProjectOnPlane(SurfaceAppliedDir.normalized, hit.normal.normalized);

                // The more parallel to the wall the velocity is, the more speed the player will have
                float dot = Vector3.Dot(SurfaceAppliedDir.normalized, hit.normal.normalized);
                float slideMagnitude = MaxSpeed * (1 - Math.Abs(dot));

                targetVel = WallSlideDir * slideMagnitude;
            }
        }

        rb.linearVelocity = targetVel;
    }

    private void OnDrawGizmos()
    {
        // Vector3 start = transform.position + Vector3.up;
        // Vector3 end = start + Vector3.down * groundRayDist;

        // Gizmos.DrawLine(start, end);

        // Gizmos.color = Color.red;
        // Gizmos.DrawWireSphere(start, sphereRadius);

        // Gizmos.color = Color.blue;
        // Gizmos.DrawWireSphere(end, sphereRadius);

        // Gizmos.color = Color.red;
        // Gizmos.DrawRay(transform.position + Vector3.up, debugVector.normalized * 3f);
        
    }
}