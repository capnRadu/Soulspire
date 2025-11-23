using UnityEngine;

public class FireballProjectile : MonoBehaviour
{
    [SerializeField] private float duration = 1.0f;
    [SerializeField] private float arcHeight = 5.0f;
    [SerializeField] private AnimationCurve speedCurve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] private GameObject explosionPrefab;

    private Transform target;
    private Vector3 startPos;
    private Vector3 controlPoint;
    private float journeyTimer = 0f;

    private Health towerHealth;
    private float baseDamage;
    private float critChance;
    private float damagePerMeterPercent;
    private float lifeStealPercent;
    private float healOnKillAmount;

    public void Initialize(Transform enemyTarget, Health towerHealth, float dmg, float crit, float dmgPerMeter, float lifesteal, float healOnKill)
    {
        target = enemyTarget;
        startPos = transform.position;
        this.towerHealth = towerHealth;

        baseDamage = dmg;
        critChance = crit;
        damagePerMeterPercent = dmgPerMeter;
        lifeStealPercent = lifesteal;
        healOnKillAmount = healOnKill;
    }

    private void Update()
    {
        if (target == null)
        {
            Arrive(false);
            return;
        }

        ProjectileTrajectoryAndMovement();
    }

    private void ProjectileTrajectoryAndMovement()
    {
        Vector3 currentTargetPos = target.position;
        Vector3 midPoint = (startPos + currentTargetPos) / 2f;
        controlPoint = midPoint + 2f * arcHeight * Vector3.up;

        if (journeyTimer < duration)
        {
            journeyTimer += Time.deltaTime;

            float linearT = journeyTimer / duration;
            float t = speedCurve.Evaluate(linearT);

            transform.position = CalculateQuadraticBezier(t, startPos, controlPoint, currentTargetPos);
        }
        else
        {
            transform.position = currentTargetPos;
            Arrive(true);
        }
    }

    private Vector3 CalculateQuadraticBezier(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 position = (uu * p0) + (2 * u * t * p1) + (tt * p2);

        return position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Health>(out Health enemyHealth))
        {
            // Damage Per Meter Stat
            float distanceTraveled = Vector3.Distance(startPos, transform.position);
            float distanceMultiplier = 1f + (distanceTraveled * (damagePerMeterPercent / 100f));
            float finalDamage = baseDamage * distanceMultiplier;

            // Critical Chance Stat
            bool isCrit = Random.Range(0f, 100f) < critChance;

            if (isCrit)
            {
                finalDamage *= 2f; // Double damage on crit
                Debug.Log("CRITICAL HIT!");
            }

            bool wasAlive = !enemyHealth.IsDead;
            enemyHealth.TakeDamage(finalDamage);

            // Life Steal Stat
            if (lifeStealPercent > 0 && towerHealth != null)
            {
                float healAmount = finalDamage * (lifeStealPercent / 100f);
                towerHealth.Heal(healAmount);
            }

            // Heal on Kill Stat
            if (wasAlive && enemyHealth.IsDead && healOnKillAmount > 0 && towerHealth != null)
            {
                towerHealth.Heal(healOnKillAmount);
            }

            Arrive(true);
        }
    }

    private void Arrive(bool explosion)
    {
        if (explosion && explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}