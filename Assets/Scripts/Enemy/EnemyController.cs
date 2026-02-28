using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private Transform playerTransform;
    private PlayerHealth playerHealth;
    
    public float speed;
    public float stopDistance = 1.5f;
    NavMeshAgent agent;

    private Animator animator;

    public float damage = 10f;
    public float damageCooldown = 1f;
    private float lastDamageTime;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.speed = speed;
        agent.stoppingDistance = stopDistance;
        GameObject player = GameObject.FindWithTag("Player");
        playerTransform = player.transform;
        playerHealth = playerTransform.GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
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
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if(distanceToPlayer <= stopDistance + 0.5f)
        {
            playerHealth.TakeDamage(damage);
        }
    }
}
