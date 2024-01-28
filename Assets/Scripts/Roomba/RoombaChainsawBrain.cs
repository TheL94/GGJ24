using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaChainsawBrain : RoombaBrain
{
    public float chainsawTurnSpeed;
    public float chainsawChargeSpeed;

    protected float baseTurningAcceleration;
    protected float baseAcceleration;

    [SerializeField] AudioClip chainsawAudio;

    protected override void Init()
    {
        base.Init();

        baseTurningAcceleration = turningAcceleration;
        baseAcceleration = acceleration;
    }

    public void SuperRotateToDirection(Vector3 targetDirection)
    {
        turningAcceleration = chainsawTurnSpeed / 2f;

        RotateToDirection(targetDirection, chainsawTurnSpeed);
    }

    public void ResetSuperValues()
    {
        turningAcceleration = baseTurningAcceleration;
        acceleration = baseAcceleration;
    }

    public void ChainsawRush(Vector3 direction)
    {
        m_audioSource.Stop();
        m_audioSource.clip = chainsawAudio;

        m_audioSource.Play();
        acceleration = chainsawChargeSpeed / 2f;

        MoveToRelative(direction, chainsawChargeSpeed);
    }
}
