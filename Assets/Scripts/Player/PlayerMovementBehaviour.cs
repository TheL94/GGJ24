using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerMovementBehaviour : MonoBehaviour
{
    public bool abilityPermitted = true;
    public Camera MainCamera;

    public Transform CameraPosition;

    [Header("Control Settings")]
    public float mouseSensitivity = 100.0f;
    public float controllerRotationMultiplier = 2.0f;
    public float playerSpeed = 5.0f;
    public float RunningSpeed = 7.0f;
    public float jumpSpeed = 5.0f;
    public float pushPower = 2.0f;
    public float gravityValue = 2.0f;

#if UNITY_ANDROID || UNITY_WEBGL
        public float androidMultiplier = 2;

        [HideInInspector]
        public Vector2 touchMovement = Vector2.zero;
        public Vector2 touchRotation = Vector2.zero;
#endif

    float m_VerticalSpeed = 0.0f;

    float m_VerticalAngle, m_HorizontalAngle;
    public float Speed { get; private set; } = 0.0f;

    public bool Grounded => m_Grounded;

    CharacterController m_CharacterController;

    bool m_Grounded;
    float m_GroundedTimer;
    float m_SpeedAtJump = 0.0f;
    Vector3 moveVelocity = new Vector3();

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
        rotation = playerInput.actions.FindAction("Rotation");
        jump = playerInput.actions.FindAction("Jump");

        jump.performed += Jump;

        m_Grounded = true;

        MainCamera.transform.SetParent(CameraPosition, false);
        MainCamera.transform.localPosition = Vector3.zero;
        MainCamera.transform.localRotation = Quaternion.identity;

        m_VerticalAngle = 0.0f;
        m_HorizontalAngle = transform.localEulerAngles.y;
    }

    void Update()
    {
        if (!abilityPermitted)
            return;

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

        //Vector3 move = Vector3.zero;
        float actualSpeed = playerSpeed;

        // Move around with WASD
#if UNITY_ANDROID
            move = new Vector3(touchMovement.x, 0, touchMovement.y);
#elif !UNITY_WEBGL || UNITY_EDITOR
        moveVelocity = new Vector3(movement.ReadValue<Vector2>().x, moveVelocity.y, movement.ReadValue<Vector2>().y);
#else
            if(WebGL_Lib.IsWebMobile())
                move = new Vector3(touchMovement.x, 0, touchMovement.y);
            else
                move = new Vector3(movement.ReadValue<Vector2>().x, 0, movement.ReadValue<Vector2>().y);
#endif
        if (moveVelocity.sqrMagnitude > 1.0f)
            moveVelocity.Normalize();

        float usedSpeed = m_Grounded ? actualSpeed : m_SpeedAtJump;

        moveVelocity = moveVelocity * usedSpeed * Time.deltaTime;

        moveVelocity = transform.TransformDirection(moveVelocity);
        m_CharacterController.Move(moveVelocity);

        Speed = moveVelocity.magnitude / (playerSpeed * Time.deltaTime) * playerSpeed;

        // Turn player
#if UNITY_ANDROID
            float turnPlayer = (touchRotation.x / 20) * mouseSensitivity * androidMultiplier;
#elif !UNITY_WEBGL || UNITY_EDITOR
        float turnPlayer = (rotation.ReadValue<Vector2>().x / 20) * mouseSensitivity * ((Gamepad.all.Count > 0) ? controllerRotationMultiplier : 1);
#else
            float turnPlayer = 0;
            if(WebGL_Lib.IsWebMobile())
                turnPlayer = (touchRotation.x / 20) * mouseSensitivity * androidMultiplier;
            else
                turnPlayer = (rotation.ReadValue<Vector2>().x / 20) * mouseSensitivity;
#endif
        m_HorizontalAngle = m_HorizontalAngle + turnPlayer;

        if (m_HorizontalAngle > 360) m_HorizontalAngle -= 360.0f;
        if (m_HorizontalAngle < 0) m_HorizontalAngle += 360.0f;

        Vector3 currentAngles = transform.localEulerAngles;
        currentAngles.y = m_HorizontalAngle;
        transform.localEulerAngles = currentAngles;

        // Camera look up/down
#if UNITY_ANDROID
            var turnCam = -touchRotation.y / 20;
            turnCam = turnCam * mouseSensitivity * androidMultiplier;
#elif !UNITY_WEBGL || UNITY_EDITOR
        var turnCam = -rotation.ReadValue<Vector2>().y / 20;
        turnCam = turnCam * mouseSensitivity;
#else
            float turnCam = 0;
            if (WebGL_Lib.IsWebMobile())
            {
                turnCam = -touchRotation.y / 20;
                turnCam = turnCam * mouseSensitivity * androidMultiplier;
            }
            else
            {
                turnCam = -rotation.ReadValue<Vector2>().y / 20;
                turnCam = turnCam * mouseSensitivity;
            }
#endif
        m_VerticalAngle = Mathf.Clamp(turnCam + m_VerticalAngle, -89.0f, 89.0f);
        currentAngles = CameraPosition.transform.localEulerAngles;
        currentAngles.x = m_VerticalAngle;
        CameraPosition.transform.localEulerAngles = currentAngles;

        // Fall down / gravity
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

    private void OnControllerColliderHit(ControllerColliderHit hit)
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
    protected virtual void Jump(InputAction.CallbackContext context)
    {
        moveVelocity.y = jumpSpeed;
    }
}
