using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaWanderingState : RoombaState
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        brainInstance.m_audioSource.Stop();
        brainInstance.m_audioSource.clip = brainInstance.audioMove;
        brainInstance.m_audioSource.Play();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        brainInstance.MoveToRelative(brainInstance.transform.forward, brainInstance.speed);
        brainInstance.SearchForTarget();
    }
}
