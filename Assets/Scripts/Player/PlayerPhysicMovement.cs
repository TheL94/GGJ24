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

    Rigidbody m_rigidbody;
    PlayerInput playerInput;

    InputAction movement;
    InputAction run;
    InputAction jump;

    Tweener m_delayedRotation;
    bool runIsPressed;

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
        jump.performed += Jump;
        movement.performed += RotateTowardsMoveDirection;
        run.performed += RunPerformed;
        run.canceled += RunCancelled;
        m_rigidbody.maxLinearVelocity = maxSpeed;
    }

    void RunPerformed(InputAction.CallbackContext ctx)
    {
        runIsPressed = true;
    }
    
    void RunCancelled(InputAction.CallbackContext obj)
    {
        runIsPressed = false;
    }

    void OnDisable()
    {
        jump.performed -= Jump;
        movement.performed -= RotateTowardsMoveDirection;
        run.performed -= RunPerformed;
    }

    void FixedUpdate()
    {
        if (!isActiveAndEnabled)
            return;
        Move();
    }

    void Jump(InputAction.CallbackContext context)
    {
        m_rigidbody.AddForce(Vector3.up * jumpForce);
    }

    void Move()
    {
        Vector2 moveInput = movement.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

        if (runIsPressed)
            moveSpeed *= runMultiplier;
        
        Vector3 moveVelocity = moveDirection.normalized * (moveSpeed * Time.deltaTime);
        m_rigidbody.MovePosition(transform.position + moveVelocity);
    }

    void RotateTowardsMoveDirection(InputAction.CallbackContext ctx)
    {
        Vector2 inputVector = ctx.ReadValue<Vector2>();
        Vector3 moveDir3 = new Vector3(inputVector.x, transform.forward.y, inputVector.y);
        if (m_delayedRotation != null && m_delayedRotation.IsPlaying())
        {
            m_delayedRotation.Complete();
        }
        else
        {
            m_delayedRotation = transform.DOLookAt(moveDir3.normalized, rotationTime, AxisConstraint.Y, Vector3.up);
        }
    }
}
