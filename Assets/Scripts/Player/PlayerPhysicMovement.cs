using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.Events;

public class PlayerPhysicMovement : MonoBehaviour
{
    Vector3 CameraForward
    {
        get => new Vector3(mainCamera.transform.forward.x, 0, mainCamera.transform.forward.z);
    }

    Vector3 CameraRight
    {
        get => new Vector3(mainCamera.transform.right.x, 0, mainCamera.transform.right.z);
    }
    
    public UnityAction<GameObject> OnBottonRacoonChange;

    private GameObject bottomRacoon;
    public GameObject BottomRacoon
    {
        get
        {
            return bottomRacoon;
        }
        private set
        {
            bottomRacoon = value;
            OnBottonRacoonChange?.Invoke(bottomRacoon);
        }
    }

    public int ActiveRacoons { get; private set; } = 3;

    public float moveSpeed = 1.0f;
    public float runMultiplier = 7.0f;
    public float jumpForce = 1f;
    public float rotationTime = 0.5f;
    public float maxSpeed;

    [SerializeField] private List<GameObject> raccoons;
    [SerializeField] private List<Transform> raccoonsPositions;

    [SerializeField] private GameObject deadRaccoonPrefab;
    [SerializeField] private Transform deadRaccoonSpawnPosition;
    [SerializeField] private float ragdollAmount;

    [Space] public float angleThreshold = .1f;

    private PlayerAnimationController animationController;
    Rigidbody m_rigidbody;
    PlayerInput playerInput;
    Camera mainCamera;
    IDamageable damageable;

    InputAction movement; //take the mf from here
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
        damageable = GetComponent<IDamageable>();
        animationController = GetComponentInChildren<PlayerAnimationController>();

        damageable.OnDamaged += OnDamaged;
        mainCamera = Camera.main;
    }

    void Start()
    {
        movement = playerInput.actions.FindAction("Movement");
        // jump = playerInput.actions.FindAction("Jump");

        // jump.performed += JumpPerformed;
        movement.performed += MovementPerformed;
        movement.canceled += MovementCanceled;

        m_rigidbody.maxLinearVelocity = maxSpeed;
    }

    void OnDisable()
    {
        // jump.performed -= JumpPerformed;
        movement.performed -= MovementPerformed;
        movement.canceled -= MovementCanceled;
        //run.performed -= RunPerformed;
        //run.canceled -= RunCancelled;
    }

    void FixedUpdate()
    {
        if (!isActiveAndEnabled)
            return;

        Move();

        if (moveInput != Vector2.zero)
        {
            Vector3 move = CameraForward * moveInput.y + CameraRight * moveInput.x;
            targetRotation = Quaternion.LookRotation(move, transform.up);
        }

        float lerpValue = currentLerpTime / rotationTime;
        transform.rotation = Quaternion.Slerp(previousRotation, targetRotation, lerpValue);

        currentLerpTime += Time.fixedDeltaTime;
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

    void OnDamaged(int damage)
    {
        GameObject spawnedRaccoon = Instantiate(deadRaccoonPrefab, deadRaccoonSpawnPosition.position,
            deadRaccoonSpawnPosition.rotation);

        spawnedRaccoon.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-ragdollAmount, ragdollAmount),
            Random.Range(-ragdollAmount, ragdollAmount)));

        StartCoroutine(RaccoonPositioning(damageable.Health));
    }

    void Move()
    {
        moveInput = movement.ReadValue<Vector2>();

        Vector3 move = CameraForward * moveInput.y + CameraRight * moveInput.x;

        // if (runIsPressed)
        //     moveSpeed *= runMultiplier;
        
        if(moveInput.magnitude > 0.1f)
            animationController.SetRun();
        else
            animationController.SetIdle();

        Vector3 moveVelocity = move * (moveSpeed * Time.deltaTime);
        m_rigidbody.MovePosition(transform.position + moveVelocity);
    }

    IEnumerator RaccoonPositioning(int currentLife)
    {
        float elapsedTime = 0f;
        float lerpTime = 1f;

        Stack<GameObject> currentRaccoons = new Stack<GameObject>(raccoons);
        Stack<Transform> currentPoints = new Stack<Transform>(raccoonsPositions);

        Dictionary<GameObject, Transform> raccoonPositionPair = new Dictionary<GameObject, Transform>();
        Dictionary<GameObject, (Vector3, Quaternion)> raccoonPreviousPositionPair =
            new Dictionary<GameObject, (Vector3, Quaternion)>();

        for (int i = damageable.MaxHealth; i < currentLife; i++)
        {
            GameObject raccoon = currentRaccoons.Pop();
            raccoon.SetActive(false);
        }

        List<GameObject> activeRaccoons = new List<GameObject>(currentRaccoons);
        BottomRacoon = activeRaccoons[activeRaccoons.Count - 1];
        ActiveRacoons = activeRaccoons.Count;

        for (int i = 0; i < currentRaccoons.Count; i++)
        {
            GameObject raccoon = currentRaccoons.Pop();
            raccoon.SetActive(true);

            raccoonPositionPair.Add(raccoon, currentPoints.Pop());
            raccoonPreviousPositionPair.Add(raccoon,
                (raccoon.transform.localPosition, raccoon.transform.localRotation));
        }

        while (elapsedTime <= lerpTime)
        {
            foreach (GameObject raccoon in activeRaccoons)
            {
                Transform target = raccoonPositionPair[raccoon];
                (Vector3, Quaternion) previousValues = raccoonPreviousPositionPair[raccoon];

                transform.localPosition =
                    Vector3.Lerp(previousValues.Item1, target.localPosition, elapsedTime / lerpTime);
                transform.localRotation =
                    Quaternion.Slerp(previousValues.Item2, target.localRotation, elapsedTime / lerpTime);
            }

            elapsedTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }
}