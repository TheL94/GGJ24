using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaShootTaserState : RoombaState
{
    RoombaTaserBrain taserBrain;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        taserBrain = (RoombaTaserBrain)brainInstance;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        if (brainInstance.Target != null)
        {
            Vector3 targetPosition = Vector3.ProjectOnPlane(brainInstance.Target.transform.position, brainInstance.transform.up);

            brainInstance.RotateToDirection((targetPosition - brainInstance.transform.position).normalized, brainInstance.turningSpeed);
        }

        brainInstance.SearchForTarget();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        taserBrain.ShootTaser();
    }
}
