using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaChainsawCharge : RoombaState
{
    RoombaChainsawBrain chainsawRoombaBrain;

    Vector3 targetDirection;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        chainsawRoombaBrain = (RoombaChainsawBrain)brainInstance;

        Vector3 targetPosition = Vector3.ProjectOnPlane(chainsawRoombaBrain.Target.transform.position, chainsawRoombaBrain.transform.up);

        targetDirection = (targetPosition - brainInstance.transform.position).normalized;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        chainsawRoombaBrain.ChainsawRush(targetDirection);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        chainsawRoombaBrain.ResetSuperValues();
    }
}
