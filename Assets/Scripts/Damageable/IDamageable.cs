using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    float lifes { get; }
    void Damage(int damage);
}
