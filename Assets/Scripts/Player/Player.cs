using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Health towerHealth;
    private Animator animator;

    [SerializeField] private AnimationClip castAnimation;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    private Enemy currentTarget;
    public float DetectionRadius => StatsManager.Instance.GetStatValue(StatType.Range);

    private float rotationSpeed = 720f;
    private bool isOnCooldown = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StatsManager.Instance.OnStatUpgraded += HandleStatUpgrade;
        HandleStatUpgrade(StatType.AttackSpeed);
    }

    private void OnDisable()
    {
        StatsManager.Instance.OnStatUpgraded -= HandleStatUpgrade;
    }

    private void HandleStatUpgrade(StatType statType)
    {
        if (statType == StatType.AttackSpeed)
        {
            // Attack Speed Stat
            float baseCooldown = castAnimation.length;
            float attackSpeedStat = StatsManager.Instance.GetStatValue(StatType.AttackSpeed);
            float calculatedCooldown = baseCooldown / attackSpeedStat;
            animator.SetFloat("castSpeedMultiplier", baseCooldown / calculatedCooldown);
        }
    }

    private void Update()
    {
        currentTarget = GetClosestEnemy();

        if (currentTarget == null)
        {
            animator.ResetTrigger("cast");
            return;
        }

        RotateTowardsTarget(currentTarget);

        if (!isOnCooldown)
        {
            StartCoroutine(ShootCooldown());
        }
    }

    private Enemy GetClosestEnemy()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        if (enemies.Length == 0) return null;

        Enemy closest = null;
        float minDist = Mathf.Infinity;

        float currentRange = DetectionRadius;

        foreach (var e in enemies)
        {
            if (e.enabled == false || e.GetComponent<Health>().IsDead) continue;

            float dist = Vector3.Distance(transform.position, e.transform.position);

            if (dist < minDist && dist <= currentRange)
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

    private IEnumerator ShootCooldown()
    {
        isOnCooldown = true;
        animator.SetTrigger("cast");
        float cooldown = castAnimation.length / animator.GetFloat("castSpeedMultiplier");

        yield return new WaitForSeconds(Mathf.Max(cooldown, 0.1f));

        isOnCooldown = false;
    }

    public void Shoot()
    {
        if (currentTarget == null || currentTarget.GetComponent<Health>().IsDead)
        {
            return;
        }

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        FireballProjectile projectile = proj.GetComponent<FireballProjectile>();

        float damage = StatsManager.Instance.GetStatValue(StatType.Damage);
        float critChance = StatsManager.Instance.GetStatValue(StatType.CriticalChance);
        float dmgPerMeter = StatsManager.Instance.GetStatValue(StatType.DamagePerMeter);
        float lifeSteal = StatsManager.Instance.GetStatValue(StatType.LifeSteal);
        float healOnKill = StatsManager.Instance.GetStatValue(StatType.HealOnKill);

        projectile.Initialize(currentTarget.transform, towerHealth, damage, critChance, dmgPerMeter, lifeSteal, healOnKill);
    }
}