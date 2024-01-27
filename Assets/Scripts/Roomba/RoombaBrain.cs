using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaBrain : MonoBehaviour
{
    public bool PlayerDetected { get => playerDetected; set { Animator.SetBool("PlayerDetected", value); playerDetected = value; } }
    public PlayerMovementBehaviour Player { get; private set; }
    public StateMachineBehaviour CurrentState { get; set; }
    public Animator Animator { get { if (animator == null) animator = GetComponent<Animator>(); return animator; } }

    public float speed;
    public float turningSpeed;
    public float angleThreshold;

    public float acceleration;
    public float turningAcceleration;

    private Animator animator;

    private float currentSpeed;
    private float currentRotationSpeed;

    private float currentTargetSpeed;
    private float currentTargetRotationSpeed;

    private bool playerDetected;

    private void Start()
    {
        ///TEST
        Player = FindAnyObjectByType<PlayerMovementBehaviour>();
        ///
        Init();
    }

    private void Update()
    {
        transform.Translate(transform.forward * currentSpeed * Time.deltaTime);
        transform.Rotate(transform.up * currentRotationSpeed * Time.deltaTime);

        currentSpeed += currentTargetSpeed * acceleration * Mathf.Sign(currentTargetSpeed) * Time.deltaTime;
        currentRotationSpeed += currentTargetRotationSpeed * turningAcceleration * Mathf.Sign(currentTargetRotationSpeed) * Time.deltaTime;

        float absoluteTargetSpeed = Mathf.Abs(currentTargetSpeed);
        currentSpeed = Mathf.Clamp(currentSpeed, -absoluteTargetSpeed, absoluteTargetSpeed);

        float absoluteRotationSpeed = Mathf.Abs(currentTargetRotationSpeed);
        currentRotationSpeed = Mathf.Clamp(currentRotationSpeed, -absoluteRotationSpeed, absoluteRotationSpeed);
    }

    private void Init()
    {
        PlayerDetected = false;
    }

    public void StopMoving()
    {
        currentTargetSpeed = 0;
        currentRotationSpeed = 0;
    }

    public void MoveToRelative(Vector3 targetDirection)
    {
        Debug.DrawRay(transform.position, targetDirection);

        float error = Vector3.SignedAngle(transform.forward, targetDirection, transform.up) / angleThreshold;

        error = Mathf.Clamp(error, -1f, 1f);

        currentTargetSpeed = speed * (1 - Mathf.Abs(error));
        currentTargetRotationSpeed = turningSpeed * error;
    }
}
