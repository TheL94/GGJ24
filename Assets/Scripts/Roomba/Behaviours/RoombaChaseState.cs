using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaChaseState : RoombaState
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        if (brainInstance.Target != null)
        {
            Vector3 targetPosition = Vector3.ProjectOnPlane(brainInstance.Target.transform.position, brainInstance.transform.up);

            brainInstance.MoveToRelative((targetPosition - brainInstance.transform.position).normalized, brainInstance.speed);
        }

        brainInstance.SearchForTarget();
    }
}
