using UnityEngine;
using UnityEngine.AI;

public class EnemyShooter : MonoBehaviour
{
    private Transform playerTransform;
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Shooting Settings")]
    public float shootingRange = 8f;
    public float fireRate = 2.5f;
    private float nextFireTime;

    public GameObject projectilePrefab;
    public Transform firePoint;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        if (!agent.enabled || playerTransform == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        bool canSeePlayer = false;

        if (distanceToPlayer <= shootingRange)
        {
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;

            Vector3 rayStart = transform.position + Vector3.up * 0.7f;

            if (Physics.Raycast(rayStart, directionToPlayer, out RaycastHit hit, shootingRange))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    canSeePlayer = true;
                }
            }
        }

        if (distanceToPlayer <= shootingRange && canSeePlayer)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;

            Vector3 direction = (playerTransform.position - transform.position).normalized;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);

            if (animator != null) animator.SetFloat("Speed", 0f);

            if (Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + fireRate;
                if (animator != null) animator.SetTrigger("Attack");
                Shoot();
            }
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(playerTransform.position);

            if (animator != null) animator.SetFloat("Speed", agent.velocity.magnitude);
        }
    }

    private void Shoot()
    {
        if (projectilePrefab == null) return;

        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position + Vector3.up * 0.7f;

        GameObject bullet = PoolManager.Instance.Get(projectilePrefab, spawnPos);
        bullet.GetComponent<EnemyProjectile>().prefab = projectilePrefab;

        bullet.transform.LookAt(playerTransform.position + Vector3.up * 1f);
    }
}