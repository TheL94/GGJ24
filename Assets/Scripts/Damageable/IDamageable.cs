using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IDamageable
{
    Action<int> OnDamaged { get; set; }
    float Health { get; }
    float Vulnerability { get; }
    void Damage(int damage);
}
