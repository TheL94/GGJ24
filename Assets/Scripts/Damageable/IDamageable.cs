using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IDamageable
{
    Action<int> OnDamaged { get; set; }
    Action<int> OnHealed { get; set; }
    int Health { get; }
    int MaxHealth { get; }
    float Vulnerability { get; }
    void Damage(int damage);
    void Heal(int damage);
}
