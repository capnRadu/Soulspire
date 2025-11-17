using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        if (animator != null)
        {
            animator.SetTrigger("hasDied");
        }

        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            if (script != this)
            {
                script.enabled = false;
            }
        }

        enabled = false;
    }

    public void OnDeathAnimationComplete()
    {
        StartCoroutine(ScaleDownAndDestroy());
    }

    private IEnumerator ScaleDownAndDestroy()
    {
        float timer = 0f;
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = Vector3.zero;
        float destroyDuration = 0.1f;

        while (timer < destroyDuration)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, timer / destroyDuration);

            timer += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;

        Destroy(gameObject);
    }
}