using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class RoombaTaserBrain : RoombaBrain
{
    [SerializeField] VisualEffect taserParticle;
    [SerializeField] Transform taserMuzzle;

    [SerializeField] float taserStunRadius;

    protected override void Init()
    {
        base.Init();
    }

    public void ShootTaser()
    {
        Debug.Log("Pew pew get tased");

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, Mathf.Infinity, ~0))
        {
            Collider[] colliders = Physics.OverlapSphere(hit.transform.position, taserStunRadius);

            foreach (Collider hitCollider in colliders)
            {
                if (hitCollider.GetComponent<IDamageable>() is IDamageable damageable)
                {
                    damageable.Damage(1);
                }
            }
        }
    }
}
