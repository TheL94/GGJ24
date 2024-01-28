using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumpDamage : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<IDamageable>() is IDamageable damageable)
        {
            damageable.Damage(1);
        }
    }
}
