using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovementBehaviour : MonoBehaviour
{
    public float Speed { get; private set; } = 0.0f;
    public bool Grounded => m_Grounded;
    
    public bool abilityPermitted = true;

    [Header("Control Settings")]
    public float controllerRotationMultiplier = 2.0f;
    public float playerSpeed = 5.0f;
    public float runningSpeed = 7.0f;
    public float jumpSpeed = 5.0f;
    public float pushPower = 2.0f;
    public float gravityValue = 2.0f;
    public float rotateAngleThreshold;

    float m_VerticalSpeed;
    float m_VerticalAngle, m_HorizontalAngle;
    CharacterController m_CharacterController;
    bool m_Grounded;
    float m_GroundedTimer;
    float m_SpeedAtJump;
    Vector3 moveVelocity;
    PlayerInput playerInput;
    InputAction movement;
    InputAction rotation;
    InputAction jump;

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
        m_Grounded = true;
        m_VerticalAngle = 0.0f;
        m_HorizontalAngle = transform.localEulerAngles.y;
    }

    void Update()
    {
        if (!abilityPermitted)
            return;

        CheckGrounded();
        var moveVelocity = Move();
        RotateTowardsMoveDir(moveVelocity);
        Fall();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var destroyable = hit.gameObject.GetComponent<ExplodingInteraction>();
        if (destroyable != null)
        {
            if (Speed >= destroyable.breakForce)
                destroyable.Break();
            return;
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

    Vector3 Move()
    {
        moveVelocity = new Vector3(movement.ReadValue<Vector2>().x, moveVelocity.y, movement.ReadValue<Vector2>().y);

        if (moveVelocity.sqrMagnitude > 1.0f)
            moveVelocity.Normalize();

        float usedSpeed = m_Grounded ? playerSpeed : m_SpeedAtJump;

        moveVelocity *= usedSpeed * Time.deltaTime;

        moveVelocity = transform.TransformDirection(moveVelocity);
        m_CharacterController.Move(moveVelocity);

        Speed = moveVelocity.magnitude / (playerSpeed * Time.deltaTime) * playerSpeed;
        return moveVelocity;
    }

    void RotateTowardsMoveDir(Vector3 moveDir)
    {
        Debug.DrawRay(transform.position, moveDir);

        float error = Vector3.SignedAngle(transform.forward, moveDir, transform.up) / rotateAngleThreshold;

        error = Mathf.Clamp(error, -1f, 1f);

        currentTargetSpeed = speed * (1 - Mathf.Abs(error));
        currentTargetRotationSpeed = turningSpeed * error;
    }

    void Fall()
    {
        //m_VerticalSpeed = m_VerticalSpeed - gravity * Time.deltaTime;
        //if (m_VerticalSpeed < -10.0f)
        //    m_VerticalSpeed = -10.0f; // max fall speed
        //var verticalMove = new Vector3(0, m_VerticalSpeed * Time.deltaTime, 0);
        //var flag = m_CharacterController.Move(verticalMove);
        //if ((flag & CollisionFlags.Below) != 0)
        //    m_VerticalSpeed = 0;

        Debug.Log(moveVelocity.y);
        moveVelocity.y += gravityValue * Time.deltaTime;
        m_CharacterController.Move(moveVelocity);
        var flag = m_CharacterController.Move(moveVelocity);
        if ((flag & CollisionFlags.Below) != 0)
            moveVelocity.y = 0;
    }
}