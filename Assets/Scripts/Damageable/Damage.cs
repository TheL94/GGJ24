using System;
using UnityEngine;

public class Damage : MonoBehaviour, IDamageable
{
    public float vulnerability = 2f;
    float m_CurrentVulnerability;

    public Action<int> OnDamaged { get; set; }

    public float Vulnerability
    {
        get
        {
            return m_CurrentVulnerability;
        }
    }

    public int startingHealth = 3;
    float m_CurrentHealth;
    public float Health
    {
        get
        {
            return m_CurrentHealth;
        }
    }

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
