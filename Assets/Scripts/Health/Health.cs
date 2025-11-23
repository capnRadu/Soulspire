using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    private HealthBar healthBar;

    [SerializeField] private bool isPlayer;
    [SerializeField] private float maxHealth;
    private float currentHealth;

    private bool isDead = false;
    public bool IsDead => isDead;
    public event Action OnDeath;

    private void Start()
    {
        healthBar = GetComponentInChildren<HealthBar>();

        if (isPlayer)
        {
            // Health Stat
            maxHealth = StatsManager.Instance.GetStatValue(StatType.Health);
        }

        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
    }

    private void Update()
    {
        if (isPlayer && !isDead && currentHealth < maxHealth)
        {
            // Health Regeneration Stat
            float regenAmount = StatsManager.Instance.GetStatValue(StatType.HealthRegeneration);

            if (regenAmount > 0)
            {
                Heal(regenAmount * Time.deltaTime);
            }
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        if (isPlayer)
        {
            // Dodge Chance Stat
            float dodgeChance = StatsManager.Instance.GetStatValue(StatType.DodgeChance);

            if (UnityEngine.Random.Range(0f, 100f) < Mathf.Min(dodgeChance, 60f))
            {
                Debug.Log("Player Dodged!");
                return;
            }

            // Damage Reduction Stat
            float damageReduction = StatsManager.Instance.GetStatValue(StatType.DamageReduction);
            float multiplier = 1f - (Mathf.Min(damageReduction, 75f) / 100f);
            damage *= multiplier;
        }

        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage:F1} damage. Health: {currentHealth:F1}");

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

        if (healthBar != null)
        {
            healthBar.SetHealth(0);
        }
    }
}