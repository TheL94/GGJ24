using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaChainsawWindupState : RoombaState
{
    RoombaChainsawBrain chainsawRoombaBrain;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        chainsawRoombaBrain = (RoombaChainsawBrain)brainInstance;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        if (chainsawRoombaBrain.Target != null)
        {
            Vector3 targetPosition = Vector3.ProjectOnPlane(chainsawRoombaBrain.Target.transform.position, chainsawRoombaBrain.transform.up);

            chainsawRoombaBrain.SuperRotateToDirection((targetPosition - brainInstance.transform.position).normalized);
        }

        brainInstance.SearchForTarget();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        chainsawRoombaBrain.ResetSuperValues();
    }
}
