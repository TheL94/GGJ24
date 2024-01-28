using System;
using UnityEngine;

public class Damage : MonoBehaviour, IDamageable
{
    public float vulnerability = 2f;
    float m_CurrentVulnerability;

    public Action<int> OnDamaged { get; set; }
    public Action<int> OnHealed { get; set; }

    public float Vulnerability { get => m_CurrentVulnerability; }

    public int Health { get => m_CurrentHealth; }

    public int MaxHealth { get => startingHealth; }

    public int startingHealth = 3;
    int m_CurrentHealth;
    AudioSource m_AudioSource;

    private void Awake()
    {
        m_CurrentHealth = startingHealth;
        m_AudioSource = GetComponent<AudioSource>();
    }

    void IDamageable.Damage(int damage)
    {
        if (m_CurrentVulnerability > 0)
            return;

        m_CurrentHealth -= damage;
        m_CurrentVulnerability = vulnerability;
        m_AudioSource.Play();

        OnDamaged?.Invoke(damage);

        if (m_CurrentHealth <= 0)
        {
            GameManager.Instance.TriggerGameOver();
        }
    }

    public void Heal(int heal)
    {
        if (m_CurrentHealth < MaxHealth)
            m_CurrentHealth += heal;
    }

    private void Update()
    {
        if (m_CurrentVulnerability > 0)
            m_CurrentVulnerability -= Time.deltaTime;

        if (m_CurrentVulnerability == 0)
            m_CurrentVulnerability = 0;
    }
}
