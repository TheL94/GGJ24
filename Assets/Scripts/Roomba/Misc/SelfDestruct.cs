using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] private VisualEffect explosionVFX;

    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionForce;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<IDamageable>() is IDamageable damageable)
        {
            explosionVFX.gameObject.AddComponent<TimedDeath>();
            explosionVFX.transform.parent = null;
            explosionVFX.Play();

            damageable.Damage(1);

            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                    rb.AddExplosionForce(explosionForce, explosionPos, explosionRadius, 3.0f);
            }

            Destroy(gameObject);
        }
    }
}
