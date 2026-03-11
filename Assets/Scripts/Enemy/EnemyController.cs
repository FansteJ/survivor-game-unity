using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private Transform playerTransform;
    private PlayerHealth playerHealth;

    public float speed;
    public float stopDistance = 1.5f;
    private NavMeshAgent agent;
    private Animator animator;

    public float damage = 10f;
    public float damageCooldown = 1f;
    private float lastDamageTime;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerHealth = playerTransform.GetComponent<PlayerHealth>();
        }
    }

    private void OnEnable()
    {
        lastDamageTime = Time.time;

        if (agent != null)
        {
            agent.speed = speed;
            agent.stoppingDistance = stopDistance;
            agent.enabled = true;
            agent.ResetPath();
        }
    }

    void Update()
    {
        if (!agent.enabled || playerTransform == null) return;

        agent.SetDestination(playerTransform.position);

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }
        else
        {
            agent.isStopped = false;
        }

        animator.SetFloat("Speed", agent.velocity.magnitude);

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= stopDistance && Time.time >= lastDamageTime + damageCooldown)
        {
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {
        lastDamageTime = Time.time;
        animator.SetTrigger("Attack");
    }

    public void Hit()
    {
        if (playerHealth == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= stopDistance + 0.5f)
        {
            playerHealth.TakeDamage(damage);
        }
    }

    private void OnDisable()
    {
        if (agent != null)
        {
            agent.enabled = false;
        }
    }
}