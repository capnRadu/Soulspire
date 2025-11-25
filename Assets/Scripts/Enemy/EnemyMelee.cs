using UnityEngine;

public class EnemyMelee : Enemy
{
    public override void Attack()
    {
        if (towerHealth != null)
        {
            towerHealth.TakeDamage(attackDamage);
        }
        else
        {
            Debug.LogWarning("Tower health script not found!");
        }
    }
}