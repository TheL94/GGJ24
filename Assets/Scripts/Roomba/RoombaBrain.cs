using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaBrain : MonoBehaviour
{
    public bool TargetDetected { get => targetDetected; set { Animator.SetBool("TargetDetected", value); targetDetected = value; } }
    public bool Bumped { get => bumped; set { Animator.SetBool("Bumped", value); bumped = value; } }
    public Transform Target { get; private set; }
    public StateMachineBehaviour CurrentState { get; set; }
    public Animator Animator { get { if (animator == null) animator = GetComponent<Animator>(); return animator; } }

    public RoombaFOV playerRoombaFov;
    public RoombaFOV turningRoombaFov;

    [SerializeField] LayerMask playerLayerMask;
    [SerializeField] LayerMask floorLayerMask;

    public float speed;
    public float turningSpeed;
    public float angleThreshold;

    public float acceleration;
    public float turningAcceleration;

    protected Animator animator;
    protected Rigidbody rigidbody;

    protected float currentSpeed;
    protected float currentRotationSpeed;

    protected float currentTargetSpeed;
    protected float currentTargetRotationSpeed;

    protected bool targetDetected;
    protected bool bumped;

    protected int numberOfCollisions = 0;

    public AudioSource m_audioSource;
    public AudioClip audioMove;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.red);

        transform.position += (transform.forward * currentSpeed * Time.deltaTime);
        transform.Rotate(transform.up * currentRotationSpeed * Time.deltaTime);

        currentSpeed += currentTargetSpeed * acceleration * Time.deltaTime;
        currentRotationSpeed += currentTargetRotationSpeed * turningAcceleration * Time.deltaTime;

        float absoluteTargetSpeed = Mathf.Abs(currentTargetSpeed);
        currentSpeed = Mathf.Clamp(currentSpeed, -absoluteTargetSpeed, absoluteTargetSpeed);

        float absoluteRotationSpeed = Mathf.Abs(currentTargetRotationSpeed);
        currentRotationSpeed = Mathf.Clamp(currentRotationSpeed, -absoluteRotationSpeed, absoluteRotationSpeed);
    }

    protected virtual void Init()
    {
        TargetDetected = false;
        Bumped = false;

        rigidbody = GetComponent<Rigidbody>();
    }

    public void SearchForTarget()
    {
        TargetDetected = playerRoombaFov.FindObject(playerLayerMask, out RaycastHit hit);

        if (hit.transform != null)
            Target = hit.transform;
    }

    public void StopMoving()
    {
        currentSpeed = 0;
        currentRotationSpeed = 0;

        currentTargetSpeed = 0;
        currentTargetRotationSpeed = 0;
    }

    public void MoveToRelative(Vector3 targetDirection, float targetSpeed)
    {
        Debug.DrawRay(transform.position, targetDirection);

        float error = Vector3.SignedAngle(transform.forward, targetDirection, transform.up) / angleThreshold;

        error = Mathf.Clamp(error, -1f, 1f);

        currentTargetSpeed = targetSpeed * (1 - Mathf.Abs(error));
        currentTargetRotationSpeed = turningSpeed * error;
    }

    public void RotateToDirection(Vector3 targetDirection, float targetSpeed)
    {
        Debug.DrawRay(transform.position, targetDirection);

        float signedAngle = Vector3.SignedAngle(transform.forward, targetDirection, transform.up) / angleThreshold;

        currentTargetSpeed = 0;
        currentTargetRotationSpeed = Mathf.Sign(signedAngle) * targetSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (floorLayerMask == (floorLayerMask | (1 << collision.gameObject.layer)))
            return;

        numberOfCollisions++;
        Bumped = numberOfCollisions > 0;

        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (floorLayerMask == (floorLayerMask | (1 << collision.gameObject.layer)))
            return;

        numberOfCollisions--;
        Bumped = numberOfCollisions > 0;

        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }
}
