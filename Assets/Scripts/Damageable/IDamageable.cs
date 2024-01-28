using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    float Health { get; }
    float Vulnerability { get; }
    void Damage(int damage);
}
