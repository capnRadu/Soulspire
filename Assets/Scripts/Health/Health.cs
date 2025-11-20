using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    private HealthBar healthBar;

    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    private bool isDead = false;
    public event Action OnDeath;

    private void Awake()
    {
        currentHealth = maxHealth;

        healthBar = GetComponentInChildren<HealthBar>();
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Current health: {currentHealth}");

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        OnDeath?.Invoke();
    }
}