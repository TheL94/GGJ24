using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaBumpState : RoombaState
{
    bool turnRight = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        int rightRays = brainInstance.turningRoombaFov.CountRaysSideways(true);
        int leftRays = brainInstance.turningRoombaFov.CountRaysSideways(false);

        Debug.Log("Right rays: " + rightRays);
        Debug.Log("Left rays: " + leftRays);

        turnRight = rightRays < leftRays;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        brainInstance.RotateToDirection(turnRight ? brainInstance.transform.right : -brainInstance.transform.right, brainInstance.turningSpeed);
    }
}
