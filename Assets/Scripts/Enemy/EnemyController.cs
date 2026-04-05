using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageScaler
{
    private Transform playerTransform;
    private PlayerHealth playerHealth;

    [Header("Movement Settings")]
    public float speed = 3f;
    public float stopDistance = 1.5f;
    public float groundOffsetY = 0f;

    private Rigidbody rb;
    private Animator animator;

    [Header("Combat Settings")]
    public float baseDamage = 10f;
    public float currentDamage = 10f;
    public float damageCooldown = 1f;
    private float lastDamageTime;

    private float currentRandomizedSpeed;

    private float aiTickRate = 0.2f;
    private float aiTimer = 0f;
    private float stopDistanceSqr;
    private float currentDistanceSqr;
    private Vector3 currentDirection;

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
            playerHealth = playerTransform.GetComponent<PlayerHealth>();
        }

        stopDistanceSqr = stopDistance * stopDistance;
    }

    private void OnEnable()
    {
        lastDamageTime = Time.time;
        currentRandomizedSpeed = speed * Random.Range(0.8f, 1.2f);
        currentDamage = baseDamage;

        aiTimer = Random.Range(0f, aiTickRate);
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

            aiTimer = 0f;
        }

        if (currentDistanceSqr <= stopDistanceSqr && Time.time >= lastDamageTime + damageCooldown)
        {
            AttackPlayer();
        }

        if (animator != null)
        {
            float animSpeed = (currentDistanceSqr > stopDistanceSqr) ? currentRandomizedSpeed : 0f;
            animator.SetFloat("Speed", animSpeed);
        }
    }

    private void FixedUpdate()
    {
        if (playerTransform == null) return;

        if (currentDistanceSqr > stopDistanceSqr)
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
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * 10f);
        }
    }

    private void AttackPlayer()
    {
        lastDamageTime = Time.time;
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
    }

    public void Hit()
    {
        if (playerHealth == null) return;

        float distSqr = (playerTransform.position - transform.position).sqrMagnitude;
        if (distSqr <= (stopDistance + 0.5f) * (stopDistance + 0.5f))
        {
            playerHealth.TakeDamage(currentDamage);
        }
    }

    public void SetDamageMultiplier(float multiplier)
    {
        currentDamage = baseDamage * multiplier;
    }
}