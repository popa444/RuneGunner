using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Параметри ворога")]
    public float maxHealth = 100f;
    public float currentHealth;
    public float damage = 10f;
    public float attackCooldown = 1.5f;
    public float moveSpeed = 3.5f;
    public float chaseRange = 1000f;
    public float attackRange = 2f;

    [Header("Гравець")]
    public Transform player;
    private PlayerHealth playerHealth;

    public Animator animator;
    private NavMeshAgent agent;
    private bool isAttacking = false;
    private float lastAttackTime = 0f;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;

        // animator = transform.GetChild(0).GetComponent<Animator>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (isDead) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            Attack();
            //Debug.Log("Attack");
        }
        else if (distance <= chaseRange)
        {
            ChasePlayer();
            //Debug.Log("ChasePlayer");
        }
        else
        {
            Idle();
            //Debug.Log("Idle");
        }
    }

    void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);

        animator.Play("Walk"); 
        animator.SetTrigger("Walk"); 
        isAttacking = false;
    }

    void Idle()
    {
        agent.isStopped = true;
        // animator.Play("Idle"); 
        // animator.SetTrigger("Idle"); 

        isAttacking = false;
    }

    void Attack()
    {
        agent.isStopped = true;

        if (!isAttacking)
        {
            animator.Play("Attack2");
        animator.SetTrigger("Attack"); 

            isAttacking = true;
        }

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;

            if (playerHealth != null)
             {
                playerHealth.TakeDamage(damage);
             }
        }
    }

    void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        animator.Play("Stun");
        animator.SetTrigger("Stun"); 

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        agent.isStopped = true;
        animator.Play("Death");
        animator.SetTrigger("Death"); 

        Destroy(gameObject, 3f); 
    }
}