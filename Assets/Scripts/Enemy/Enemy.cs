using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform tower;
    private Health towerHealth;
    private Animator animator;
    [SerializeField] private AnimationClip spawnAnimation;
    private float spawnAnimationDuration => spawnAnimation.length;
    [SerializeField] private AnimationClip attackAnimation;

    private float speed = 6f;
    private float minimumDistanceToTower = 4f;
    private float attackRate => attackAnimation.length;
    private float nextAttackTime = 0f;
    private int attackDamage = 2;

    private float yPos;

    private int coinsOnDeath = 5;
    public int CoinsOnDeath => coinsOnDeath;
    private int soulsOnDeath = 1;
    public int SoulsOnDeath => soulsOnDeath;
    private float experienceOnDeath = 15f;
    public float ExperienceOnDeath => experienceOnDeath;

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

    public void Attack()
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