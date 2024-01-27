using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerPhysicMovement : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float runMultiplier = 7.0f;
    public float jumpForce = 1f;
    public float rotationTime = 1f;
    public float maxSpeed;

    [Space] public float angleThreshold = .1f;

    Rigidbody m_rigidbody;
    PlayerInput playerInput;

    InputAction movement;
    InputAction run;
    InputAction jump;

    Coroutine m_delayedRotationRoutine;
    Tweener m_delayedRotation;
    bool runIsPressed;
    bool movementIsPressed;

    private Vector2 moveInput;
    private Vector3 moveDirection;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        m_rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        movement = playerInput.actions.FindAction("Movement");
        run = playerInput.actions.FindAction("Run");
        jump = playerInput.actions.FindAction("Jump");
        jump.performed += JumpPerformed;
        movement.performed += MovementPerformed;
        movement.canceled += MovementCanceled;
        run.performed += RunPerformed;
        run.canceled += RunCancelled;
        m_rigidbody.maxLinearVelocity = maxSpeed;
    }

    void OnDisable()
    {
        jump.performed -= JumpPerformed;
        movement.performed -= MovementPerformed;
        movement.canceled -= MovementCanceled;
        run.performed -= RunPerformed;
        run.canceled -= RunCancelled;
    }

    void FixedUpdate()
    {
        if (!isActiveAndEnabled)
            return;

        moveInput = movement.ReadValue<Vector2>();
        moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

        Move();
        CheckCurrentRotation();
    }

    void CheckCurrentRotation()
    {
        if (Vector3.Angle(transform.forward, moveDirection.normalized) > 30f)
        {
            if (m_delayedRotationRoutine != null)
            {
                StopCoroutine(m_delayedRotationRoutine);
            }

            moveInput = movement.ReadValue<Vector2>();
            moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
            m_delayedRotationRoutine = StartCoroutine(SlerpRotation(moveInput, moveDirection));
        }
    }
    
    void RunPerformed(InputAction.CallbackContext ctx)
    {
        runIsPressed = true;
    }

    void RunCancelled(InputAction.CallbackContext ctx)
    {
        runIsPressed = false;
    }

    void JumpPerformed(InputAction.CallbackContext context)
    {
        m_rigidbody.AddForce(Vector3.up * jumpForce);
    }

    void MovementPerformed(InputAction.CallbackContext context)
    {
        if (!movementIsPressed)
        {
            movementIsPressed = true;
            
            moveInput = movement.ReadValue<Vector2>();
            moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
            m_delayedRotationRoutine = StartCoroutine(SlerpRotation(moveInput, moveDirection));
        }
    }
    
    void MovementCanceled(InputAction.CallbackContext ctx)
    {
        movementIsPressed = false;
    }

    void Move()
    {
        if (runIsPressed)
            moveSpeed *= runMultiplier;

        Vector3 moveVelocity = moveDirection.normalized * (moveSpeed * Time.deltaTime);
        m_rigidbody.MovePosition(transform.position + moveVelocity);
    }

    IEnumerator SlerpRotation(Vector2 inputVector, Vector3 moveDir3)
    {
        float timeCount = 0;
        var wait = new WaitForEndOfFrame();

        do
        {
            // inputVector = ctx.ReadValue<Vector2>();
            // moveDir3 = new Vector3(inputVector.x, 0, inputVector.y);

            var lookRotation = Quaternion.LookRotation(moveDir3.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, timeCount);

            timeCount += Time.deltaTime * rotationTime;
            yield return wait;
        } while (Vector3.Angle(transform.forward, moveDir3.normalized) > angleThreshold);

        m_delayedRotationRoutine = null;
    }
}