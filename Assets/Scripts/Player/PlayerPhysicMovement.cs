using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerPhysicMovement : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float runMultiplier = 7.0f;
    public float jumpForce = 1f;
    public float rotationTime = 0.5f;
    public float maxSpeed;

    [Space] public float angleThreshold = .1f;

    Rigidbody m_rigidbody;
    PlayerInput playerInput;

    InputAction movement; //take the mf from here
    InputAction run;
    InputAction jump;

    bool runIsPressed;

    private float currentLerpTime = 0;

    private Vector2 moveInput;
    private Quaternion targetRotation;
    private Quaternion previousRotation;

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

        Move();

        if (moveInput != Vector2.zero)
        {
            targetRotation = Quaternion.LookRotation(new Vector3(moveInput.x, 0, moveInput.y), transform.up);
        }
        float lerpValue = currentLerpTime / rotationTime;
        transform.rotation = Quaternion.Slerp(previousRotation, targetRotation, lerpValue);

        currentLerpTime += Time.fixedDeltaTime;
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
        previousRotation = transform.rotation;
        currentLerpTime = 0;
    }

    void MovementCanceled(InputAction.CallbackContext ctx)
    {

    }

    void Move()
    {
        moveInput = movement.ReadValue<Vector2>();

        if (runIsPressed)
            moveSpeed *= runMultiplier;

        Vector3 moveVelocity = new Vector3(moveInput.x, 0, moveInput.y) * (moveSpeed * Time.deltaTime);
        m_rigidbody.MovePosition(transform.position + moveVelocity);

    }
}