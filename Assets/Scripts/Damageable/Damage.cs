using UnityEngine;

public class Damage : MonoBehaviour, IDamageable
{
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
        m_CurrentHealth-= damage;

        if(m_CurrentHealth <= 0) { }//TODO something
    }
}
