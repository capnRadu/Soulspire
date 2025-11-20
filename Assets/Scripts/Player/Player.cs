using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private AnimationClip castAnimation;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float detectionRadius = 10f;
    public float DetectionRadius => detectionRadius;

    private float rotationSpeed = 720f;

    private bool isOnCooldown = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Enemy target = GetClosestEnemy();
        if (target == null) return;

        RotateTowardsTarget(target);

        float cooldownDuration = 1f / castAnimation.length;
        if (!isOnCooldown)
        {
            StartCoroutine(ShootCooldown(target));
        }
    }

    private Enemy GetClosestEnemy()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        if (enemies.Length == 0) return null;

        Enemy closest = null;
        float minDist = Mathf.Infinity;

        foreach (var e in enemies)
        {
            if (e.enabled == false) continue;

            float dist = Vector3.Distance(transform.position, e.transform.position);

            if (dist < minDist && dist <= detectionRadius)
            {
                minDist = dist;
                closest = e;
            }
        }

        return closest;
    }

    private void RotateTowardsTarget(Enemy target)
    {
        Vector3 direction = target.transform.position - transform.position;
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        float step = rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);
    }

    private IEnumerator ShootCooldown(Enemy target)
    {
        isOnCooldown = true;
        animator.SetTrigger("cast");

        yield return new WaitForSeconds(0.7f);

        Shoot(target.gameObject);

        float cooldown = 1f / castAnimation.length;
        yield return new WaitForSeconds(cooldown);

        isOnCooldown = false;
    }

    private void Shoot(GameObject target)
    {
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        FireballProjectile projectile = proj.GetComponent<FireballProjectile>();
        projectile.Initialize(target.transform);
    }
}