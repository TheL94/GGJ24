using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerPhysicMovement : MonoBehaviour
{
    Vector3 CameraForward { get => new Vector3(mainCamera.transform.forward.x, 0, mainCamera.transform.forward.z); }
    Vector3 CameraRight { get => new Vector3(mainCamera.transform.right.x, 0, mainCamera.transform.right.z); }

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

    Rigidbody m_rigidbody;
    PlayerInput playerInput;
    Camera mainCamera;
    IDamageable damageable;

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
        damageable = GetComponent<IDamageable>();

        damageable.OnDamaged += OnDamaged;
        mainCamera = Camera.main;
    }

    void Start()
    {
        movement = playerInput.actions.FindAction("Movement");
        run = playerInput.actions.FindAction("Run");
        jump = playerInput.actions.FindAction("Jump");

        jump.performed += JumpPerformed;
        movement.performed += MovementPerformed;
        movement.canceled += MovementCanceled;

        //run.performed += RunPerformed;
        //run.canceled += RunCancelled;

        m_rigidbody.maxLinearVelocity = maxSpeed;
    }

    void OnDisable()
    {
        jump.performed -= JumpPerformed;
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

    void OnDamaged(int damage)
    {
        GameObject spawnedRaccoon = Instantiate(deadRaccoonPrefab, deadRaccoonSpawnPosition.position, deadRaccoonSpawnPosition.rotation);

        spawnedRaccoon.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-ragdollAmount, ragdollAmount), Random.Range(-ragdollAmount, ragdollAmount)));

        StartCoroutine(RaccoonPositioning(damageable.Health));
    }

    void Move()
    {
        moveInput = movement.ReadValue<Vector2>();

        Vector3 move = CameraForward * moveInput.y + CameraRight * moveInput.x;

        if (runIsPressed)
            moveSpeed *= runMultiplier;

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
        Dictionary<GameObject, (Vector3, Quaternion)> raccoonPreviousPositionPair = new Dictionary<GameObject, (Vector3, Quaternion)>();

        for (int i = damageable.MaxHealth; i < currentLife; i++)
        {
            GameObject raccoon = currentRaccoons.Pop();
            raccoon.SetActive(false);
        }

        List<GameObject> activeRaccoons = new List<GameObject>(currentRaccoons);

        for (int i = 0; i < currentRaccoons.Count; i++)
        {
            GameObject raccoon = currentRaccoons.Pop();
            raccoon.SetActive(true);

            raccoonPositionPair.Add(raccoon, currentPoints.Pop());
            raccoonPreviousPositionPair.Add(raccoon, (raccoon.transform.localPosition, raccoon.transform.localRotation));
        }

        while (elapsedTime <= lerpTime)
        {
            foreach (GameObject raccoon in activeRaccoons)
            {
                Transform target = raccoonPositionPair[raccoon];
                (Vector3, Quaternion) previousValues = raccoonPreviousPositionPair[raccoon];

                transform.localPosition = Vector3.Lerp(previousValues.Item1, target.localPosition, elapsedTime / lerpTime);
                transform.localRotation = Quaternion.Slerp(previousValues.Item2, target.localRotation, elapsedTime / lerpTime);
            }

            elapsedTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }
}