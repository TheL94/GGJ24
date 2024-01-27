using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementBehaviour : MonoBehaviour
{
    public bool abilityPermitted = true;
    public Camera MainCamera;

    public Transform CameraPosition;

    [Header("Control Settings")]
    public float MouseSensitivity = 100.0f;
    public float PlayerSpeed = 5.0f;
    public float RunningSpeed = 7.0f;
    public float JumpSpeed = 5.0f;

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

    PlayerInput playerInput;
    InputAction movement;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        m_CharacterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        movement = playerInput.actions.FindAction("Movement");

        m_Grounded = true;

        MainCamera.transform.SetParent(CameraPosition, false);
        MainCamera.transform.localPosition = Vector3.zero;
        MainCamera.transform.localRotation = Quaternion.identity;

        m_VerticalAngle = 0.0f;
        m_HorizontalAngle = transform.localEulerAngles.y;
    }

    void Update()
    {
        if(!abilityPermitted) 
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

        Speed = 0;
        Vector3 move = Vector3.zero;
		float actualSpeed = PlayerSpeed;

        // Move around with WASD
#if UNITY_ANDROID
        move = new Vector3(touchMovement.x, 0, touchMovement.y);
#elif !UNITY_WEBGL || UNITY_EDITOR
        move = new Vector3(movement.ReadValue<Vector2>().x, 0, movement.ReadValue<Vector2>().y);
#else
        if(WebGL_Lib.IsWebMobile())
            move = new Vector3(touchMovement.x, 0, touchMovement.y);
        else
            move = new Vector3(movement.ReadValue<Vector2>().x, 0, movement.ReadValue<Vector2>().y);
#endif
        if (move.sqrMagnitude > 1.0f)
			move.Normalize();

		float usedSpeed = m_Grounded ? actualSpeed : m_SpeedAtJump;

		move = move * usedSpeed * Time.deltaTime;

		move = transform.TransformDirection(move);
		m_CharacterController.Move(move);

        // Turn player
#if UNITY_ANDROID
        float turnPlayer = (touchRotation.x / 20) * MouseSensitivity * androidMultiplier;
#elif !UNITY_WEBGL || UNITY_EDITOR
#else
        float turnPlayer = 0;
        if(WebGL_Lib.IsWebMobile())
            turnPlayer = (touchRotation.x / 20) * MouseSensitivity * androidMultiplier;
        else
            turnPlayer = (rotation.ReadValue<Vector2>().x / 20) * MouseSensitivity;
#endif

		if (m_HorizontalAngle > 360) m_HorizontalAngle -= 360.0f;
		if (m_HorizontalAngle < 0) m_HorizontalAngle += 360.0f;

		Vector3 currentAngles = transform.localEulerAngles;
		currentAngles.y = m_HorizontalAngle;
		transform.localEulerAngles = currentAngles;

        // Camera look up/down
#if UNITY_ANDROID
        var turnCam = -touchRotation.y / 20;
        turnCam = turnCam * MouseSensitivity * androidMultiplier;
#elif !UNITY_WEBGL || UNITY_EDITOR

#else
        float turnCam = 0;
        if (WebGL_Lib.IsWebMobile())
        {
            turnCam = -touchRotation.y / 20;
            turnCam = turnCam * MouseSensitivity * androidMultiplier;
        }
        else
        {
            turnCam = -rotation.ReadValue<Vector2>().y / 20;
            turnCam = turnCam * MouseSensitivity;
        }
#endif
		currentAngles = CameraPosition.transform.localEulerAngles;
		currentAngles.x = m_VerticalAngle;
		CameraPosition.transform.localEulerAngles = currentAngles;

		Speed = move.magnitude / (PlayerSpeed * Time.deltaTime);

		// Fall down / gravity
		m_VerticalSpeed = m_VerticalSpeed - 10.0f * Time.deltaTime;
        if (m_VerticalSpeed < -10.0f)
            m_VerticalSpeed = -10.0f; // max fall speed
        var verticalMove = new Vector3(0, m_VerticalSpeed * Time.deltaTime, 0);
        var flag = m_CharacterController.Move(verticalMove);
        if ((flag & CollisionFlags.Below) != 0)
            m_VerticalSpeed = 0;
    }
}
