using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float detectionRadius = 10f;
    public float DetectionRadius => detectionRadius;
    [SerializeField] private float fireRate = 1f;

    private float _fireCooldown;

    private void Update()
    {
        _fireCooldown -= Time.deltaTime;

        Enemy target = GetClosestEnemy();
        if (target == null) return;

        if (_fireCooldown <= 0f)
        {
            Shoot(target.gameObject);
            _fireCooldown = 1f / fireRate;
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
            float dist = Vector3.Distance(transform.position, e.transform.position);

            if (dist < minDist && dist <= detectionRadius)
            {
                minDist = dist;
                closest = e;
            }
        }

        return closest;
    }

    private void Shoot(GameObject target)
    {
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        FireballProjectile projectile = proj.GetComponent<FireballProjectile>();
        projectile.Initialize(target.transform);
    }
}