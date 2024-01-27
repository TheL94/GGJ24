using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaChaseState : RoombaState
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);


        brainInstance.MoveToRelative(new Vector3(-1f, 0, 0f));
    }
}
