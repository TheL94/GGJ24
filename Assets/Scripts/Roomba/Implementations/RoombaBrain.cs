using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaBrain : MonoBehaviour, IRoombaBrain
{
    public bool PlayerDetected { get => playerDetected; set { animator.SetBool("PlayerDetected", value); playerDetected = value; } }

    public StateMachineBehaviour CurrentState { get; set; }
    public Animator Animator { get { if (animator == null) animator = GetComponent<Animator>(); return animator; } }

    private Animator animator;
    private bool playerDetected;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        PlayerDetected = false;
    }
}
