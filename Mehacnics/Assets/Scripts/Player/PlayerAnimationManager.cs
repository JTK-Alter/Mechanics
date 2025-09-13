using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public enum IdleMood { normal = 0, battle = 1, happy = 2, relaxed = 3, scared = 4 }
    PlayerLocomotion p_movement => PlayerInputScript.Instance._playerLocomotion;

    public IdleMood idle;
    public float modelRotationSpeed = 10f;


    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        idle = IdleMood.normal;
    }

    void Update()
    {
        animator.SetFloat("Idle", (float)idle);
        animator.SetFloat("Speed", p_movement.rb.linearVelocity.magnitude / p_movement.MaxSpeed);
        animator.SetFloat("MovementMagnitude", p_movement.inputDir.magnitude);
    }

    void LateUpdate()
    {
        if (p_movement.inputDir.sqrMagnitude > .01 && Vector3.Angle(p_movement.moveDirection.normalized, transform.forward.normalized) > 1f) {RotateTowardsVelocity();}
    }

    public void RotateTowardsVelocity()
    {
        Vector3 dir = p_movement.moveDirection.normalized;
        dir.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, modelRotationSpeed * Time.deltaTime);
    }

}
