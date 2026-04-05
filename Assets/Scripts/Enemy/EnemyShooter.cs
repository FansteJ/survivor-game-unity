using UnityEngine;

public class EnemyShooter : MonoBehaviour, IDamageScaler
{
    private Transform playerTransform;
    private Rigidbody rb;
    private Animator animator;

    [Header("Movement Settings")]
    public float speed = 1.5f;
    public float groundOffsetY = 0f;

    [Header("Shooting Settings")]
    public float shootingRange = 8f;
    public float fireRate = 2.5f;
    private float nextFireTime;
    public float baseDamage = 25f;
    public float currentDamage = 25f;

    public GameObject projectilePrefab;
    public Transform firePoint;

    private bool canSeePlayer = false;

    private float aiTickRate = 0.3f;
    private float aiTimer = 0f;
    private float shootingRangeSqr;
    private float currentDistanceSqr;
    private Vector3 currentDirection;
    private float currentRandomizedSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        shootingRangeSqr = shootingRange * shootingRange;
    }

    private void OnEnable()
    {
        currentRandomizedSpeed = speed * Random.Range(0.8f, 1.2f);
        aiTimer = Random.Range(0f, aiTickRate);
        canSeePlayer = false;
    }

    void Update()
    {
        if (playerTransform == null) return;

        aiTimer += Time.deltaTime;

        if (aiTimer >= aiTickRate)
        {
            Vector3 offset = playerTransform.position - transform.position;
            currentDistanceSqr = offset.sqrMagnitude;

            offset.y = 0;
            currentDirection = offset.normalized;

            canSeePlayer = false;

            if (currentDistanceSqr <= shootingRangeSqr)
            {
                CheckLineOfSight();
            }

            aiTimer = 0f;
        }

        if (currentDistanceSqr <= shootingRangeSqr && canSeePlayer && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            if (animator != null) animator.SetTrigger("Attack");
            Shoot();
        }

        if (animator != null)
        {
            float animSpeed = (currentDistanceSqr > shootingRangeSqr || !canSeePlayer) ? currentRandomizedSpeed : 0f;
            animator.SetFloat("Speed", animSpeed);
        }
    }

    private void FixedUpdate()
    {
        if (playerTransform == null) return;

        if (currentDistanceSqr > shootingRangeSqr || !canSeePlayer)
        {
            Vector3 newPosition = transform.position + currentDirection * currentRandomizedSpeed * Time.fixedDeltaTime;

            if (Terrain.activeTerrain != null)
            {
                float terrainHeight = Terrain.activeTerrain.SampleHeight(newPosition);
                newPosition.y = terrainHeight + Terrain.activeTerrain.transform.position.y + groundOffsetY;
            }

            transform.position = newPosition;
            FaceTarget(currentDirection);
        }
        else
        {
            FaceTarget(currentDirection);
        }
    }

    private void FaceTarget(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.fixedDeltaTime * 10f);
        }
    }

    private void CheckLineOfSight()
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

    private void Shoot()
    {
        if (projectilePrefab == null) return;
        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position + Vector3.up * 0.7f;
        GameObject bullet = PoolManager.Instance.Get(projectilePrefab, spawnPos);
        EnemyProjectile proj = bullet.GetComponent<EnemyProjectile>();

        if (proj != null)
        {
            proj.prefab = projectilePrefab;
            proj.damage = currentDamage;
        }

        bullet.transform.LookAt(playerTransform.position + Vector3.up * 1f);
    }

    public void SetDamageMultiplier(float multiplier)
    {
        currentDamage = baseDamage * multiplier;
    }
}