using UnityEngine;

public class EnemyRanged : Enemy
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    public override void Attack()
    {
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        RangedProjectile projectile = proj.GetComponent<RangedProjectile>();
        projectile.Initialize(tower, attackDamage);
    }
}