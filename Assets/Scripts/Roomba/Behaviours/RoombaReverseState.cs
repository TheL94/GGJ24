using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaReverseState : RoombaState
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        brainInstance.StopMoving();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        brainInstance.MoveToRelative(brainInstance.transform.forward, -brainInstance.speed / 2f);
    }
}
