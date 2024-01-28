using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementBehaviour : MonoBehaviour
{
    public float Speed { get; private set; } = 0.0f;
    public bool Grounded => m_Grounded;

    public bool abilityPermitted = true;

    [Header("Control Settings")] 
    public float playerSpeed = 5.0f;
    public float runningSpeed = 7.0f;
    public float jumpSpeed = 5.0f;
    public float pushPower = 2.0f;
    public float gravityValue = 2.0f;
    public float rotationTime = 1f;
    
    CharacterController m_CharacterController;
    PlayerInput playerInput;
    
    InputAction movement;
    InputAction rotation;
    InputAction jump;
    
    bool m_Grounded;
    float m_GroundedTimer;
    float m_SpeedAtJump;
    Vector3 moveVelocity;

    Tween m_delayedRotation;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        m_CharacterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        movement = playerInput.actions.FindAction("Movement");
        jump = playerInput.actions.FindAction("Jump");
        jump.performed += Jump;
        movement.performed += RotateTowardsMoveDirection;
        m_Grounded = true;
    }

    private void OnDisable()
    {
        jump.performed -= Jump;
        movement.performed -= RotateTowardsMoveDirection;
    }

    void Update()
    {
        if (!abilityPermitted)
            return;
        
        CheckGrounded();
        Move();
        Fall();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var destroyable = hit.gameObject.GetComponent<ExplodingInteraction>();
        if (destroyable != null)
        {
            //if (Speed >= destroyable.breakForce)
            //    destroyable.Break();
            //return;
        }

        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
            return;
        if (hit.moveDirection.y < -0.3)
            return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.velocity = pushDir * pushPower;
    }

    void Jump(InputAction.CallbackContext context)
    {
        moveVelocity.y = jumpSpeed;
    }

    void CheckGrounded()
    {
        //we define our own grounded and not use the Character controller one as the character controller can flicker
        //between grounded/not grounded on small step and the like. So we actually make the controller "not grounded" only
        //if the character controller reported not being grounded for at least .5 second;
        if (!m_CharacterController.isGrounded)
        {
            if (m_Grounded)
            {
                m_GroundedTimer += Time.deltaTime;
                if (m_GroundedTimer >= 0.5f)
                    m_Grounded = false;
            }
        }
        else
        {
            m_GroundedTimer = 0.0f;
            m_Grounded = true;
        }
    }

    void Move()
    {
        Vector2 moveInput = movement.ReadValue<Vector2>();
        moveVelocity = new Vector3(moveInput.x, moveVelocity.y, moveInput.y);

        if (moveVelocity.sqrMagnitude > 1.0f)
            moveVelocity.Normalize();

        float usedSpeed = m_Grounded ? playerSpeed : m_SpeedAtJump;
        moveVelocity *= usedSpeed * Time.deltaTime;
        m_CharacterController.Move(moveVelocity);
        Speed = moveVelocity.magnitude / (playerSpeed * Time.deltaTime) * playerSpeed;
    }

    void RotateTowardsMoveDirection(InputAction.CallbackContext ctx)
    {
        if (m_delayedRotation != null && m_delayedRotation.IsPlaying())
            m_delayedRotation.Kill();
        Vector2 inputVector = ctx.ReadValue<Vector2>();
        Vector3 moveDir3 = new Vector3(inputVector.x, transform.forward.y, inputVector.y);
        m_delayedRotation = transform.DOLookAt(moveDir3.normalized, rotationTime, AxisConstraint.Y, Vector3.up);
    }

    void Fall()
    {
        // m_VerticalSpeed = m_VerticalSpeed - gravityValue * Time.deltaTime;
        // if (m_VerticalSpeed < -10.0f)
        //     m_VerticalSpeed = -10.0f; // max fall speed
        // var verticalMove = new Vector3(0, m_VerticalSpeed * Time.deltaTime, 0);
        // var flag = m_CharacterController.Move(verticalMove);
        // if ((flag & CollisionFlags.Below) != 0)
        //     m_VerticalSpeed = 0;

        moveVelocity.y -= Mathf.Abs(gravityValue) * Time.deltaTime;
        m_CharacterController.Move(moveVelocity);
        var flag = m_CharacterController.Move(moveVelocity);
        if ((flag & CollisionFlags.Below) != 0)
            moveVelocity.y = 0;
    }
}