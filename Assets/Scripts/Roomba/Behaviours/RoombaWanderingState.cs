using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaWanderingState : RoombaState
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        brainInstance.MoveToRelative(brainInstance.transform.forward, brainInstance.speed);
        brainInstance.SearchForPlayer();
    }
}
