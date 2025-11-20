using System.Collections;
using UnityEngine;

public class EnemyDeathAction : MonoBehaviour
{
    private Health health;
    private Animator animator;

    private void Awake()
    {
        health = GetComponent<Health>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        health.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        health.OnDeath -= HandleDeath;
    }

    private void HandleDeath()
    {
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