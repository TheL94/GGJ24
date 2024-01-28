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

        if (Physics.Raycast(taserMuzzle.position, transform.forward, out RaycastHit hit, Mathf.Infinity, ~0))
        {
            taserParticle.SetVector3("ThunderTarget", transform.InverseTransformPoint(hit.transform.position));
            taserParticle.SetVector3("ThunderStart", transform.InverseTransformPoint(taserMuzzle.position));
            //taserParticle.SetVector2("Size", new Vector2(taserStunRadius, taserStunRadius));

            Collider[] colliders = Physics.OverlapSphere(hit.transform.position, taserStunRadius);

            taserParticle.Play();

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
