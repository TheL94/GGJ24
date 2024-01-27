using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaState : StateMachineBehaviour
{
    protected IRoombaBrain brainInstance { get; private set; }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        brainInstance = animator.GetComponent<IRoombaBrain>();
    }
}
