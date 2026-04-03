using UnityEngine;
using UnityEngine.AI;

public class EnemyShooter : MonoBehaviour, IDamageScaler
{
    private Transform playerTransform;
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Movement Settings")]
    public float speed = 1.5f;

    [Header("Shooting Settings")]
    public float shootingRange = 8f;
    public float fireRate = 2.5f;
    private float nextFireTime;
    public float baseDamage = 25f;
    public float currentDamage = 25f;

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

    private void OnEnable()
    {
        if (agent != null)
        {
            agent.speed = speed * Random.Range(0.8f, 1.2f);
        }
    }

    void Update()
    {
        if (!agent.enabled || playerTransform == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        bool canSeePlayer = false;

        if (distanceToPlayer <= shootingRange)
        {
            Vector3 targetPos = playerTransform.position + Vector3.up * 1f;
            Vector3 rayStart = transform.position + Vector3.up * 1f;

            Vector3 directionToPlayer = (targetPos - rayStart).normalized;
            rayStart += directionToPlayer * 0.5f;

            RaycastHit[] hits = Physics.RaycastAll(rayStart, directionToPlayer, shootingRange);

            float playerDistance = Mathf.Infinity;
            float closestObstacleDistance = Mathf.Infinity;

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.isTrigger) continue;
                if (hit.collider.CompareTag("Player"))
                {
                    playerDistance = hit.distance;
                }
                else if (!hit.collider.CompareTag("Enemy"))
                {
                    if (hit.distance < closestObstacleDistance)
                    {
                        closestObstacleDistance = hit.distance;
                    }
                }
            }

            if (playerDistance != Mathf.Infinity && playerDistance < closestObstacleDistance)
            {
                canSeePlayer = true;
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
        EnemyProjectile proj = bullet.GetComponent<EnemyProjectile>();
        proj.prefab = projectilePrefab;
        proj.damage = currentDamage;

        bullet.transform.LookAt(playerTransform.position + Vector3.up * 1f);
    }

    public void SetDamageMultiplier(float multiplier)
    {
        currentDamage = baseDamage * multiplier;
    }
}