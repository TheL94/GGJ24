using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaBumpState : RoombaState
{
    bool turnRight = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        turnRight = brainInstance.turningRoombaFov.CountRaysSideways(true) < brainInstance.turningRoombaFov.CountRaysSideways(false);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        brainInstance.RotateToDirection(turnRight ? brainInstance.transform.right : -brainInstance.transform.right, brainInstance.turningSpeed);
    }
}
