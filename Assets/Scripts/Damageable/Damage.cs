using System;
using UnityEngine;

public class Damage : MonoBehaviour, IDamageable
{
    public float vulnerability = 2f;
    float m_CurrentVulnerability;

    public Action<int> OnDamaged { get; set; }

    public float Vulnerability { get => m_CurrentVulnerability; }

    public int Health { get => m_CurrentHealth; }

    public int MaxHealth { get => startingHealth; }

    public int startingHealth = 3;
    int m_CurrentHealth;

    private void Awake()
    {
        m_CurrentHealth = startingHealth;
    }

    void IDamageable.Damage(int damage)
    {
        m_CurrentHealth -= damage;
        m_CurrentVulnerability = vulnerability;

        if (m_CurrentHealth <= 0)
        {
            //Fucking die
        }
        //TODO something
    }

    private void Update()
    {
        if (m_CurrentVulnerability > 0)
            m_CurrentVulnerability -= Time.deltaTime;

        if (m_CurrentVulnerability == 0)
            m_CurrentVulnerability = 0;
    }
}
