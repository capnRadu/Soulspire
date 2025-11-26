using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Transform tower;
    protected Health towerHealth;
    protected Animator animator;
    [SerializeField] protected Health health;
    [SerializeField] protected AnimationClip spawnAnimation;
    protected float spawnAnimationDuration => spawnAnimation.length;
    [SerializeField] protected AnimationClip attackAnimation;

    [SerializeField] protected float speed = 6f;
    [SerializeField] protected float minimumDistanceToTower = 4f;
    [SerializeField] protected float attackCooldown = 0.5f;
    protected float attackRate => attackAnimation.length + attackCooldown;
    protected float nextAttackTime = 0f;
    [SerializeField] protected int attackDamage = 2;

    protected float yPos;

    [SerializeField] protected int coinsOnDeath = 5;
    public int CoinsOnDeath => coinsOnDeath;
    [SerializeField] protected int soulsOnDeath = 1;
    public int SoulsOnDeath => soulsOnDeath;
    [SerializeField] protected float experienceOnDeath = 15f;
    public float ExperienceOnDeath => experienceOnDeath;

    public void ApplyDifficultyBuffs(float hpMultiplier, float dmgMultiplier)
    {
        attackDamage = Mathf.RoundToInt(attackDamage * dmgMultiplier);
        health.Buff(hpMultiplier);
    }

    private void Awake()
    {
        tower = GameObject.FindGameObjectWithTag("Tower").transform;
        animator = GetComponent<Animator>();
        yPos = transform.position.y;

        if (tower != null)
        {
            towerHealth = tower.GetComponent<Health>();
        }

        enabled = false;
        StartCoroutine(EnableAfterSpawn());
    }

    private void Update()
    {
        if (tower != null)
        {
            if (Vector3.Distance(transform.position, tower.position) > minimumDistanceToTower)
            {
                MoveTowardsTower();
            }
            else
            {
                CheckAttack();
            }
        }
    }

    private IEnumerator EnableAfterSpawn()
    {
        yield return new WaitForSeconds(spawnAnimationDuration);
        enabled = true;
    }

    private void MoveTowardsTower()
    {
        Vector3 direction = (tower.position - transform.position).normalized;
        transform.position += speed * Time.deltaTime * direction;
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);

        if (!animator.GetBool("isWalking"))
        {
            animator.SetBool("isWalking", true);
        }
    }

    private void CheckAttack()
    {
        if (animator.GetBool("isWalking"))
        {
            animator.SetBool("isWalking", false);
        }

        if (Time.time >= nextAttackTime)
        {
            animator.SetTrigger("attack");
            nextAttackTime = Time.time + attackRate;
        }
    }

    public virtual void Attack()
    {
    }
}