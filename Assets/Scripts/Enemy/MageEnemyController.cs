using UnityEngine;
using System.Collections;

public class MageEnemyController : MonoBehaviour
{
    private Transform playerTransform;
    private PlayerHealth playerHealth;
    private EnemyHealth myHealth;

    [Header("Mage Settings")]
    public float speed = 2f;
    public float attackRange = 15f;
    public float castTime = 5f;
    public float magicCooldown = 5f;
    public float groundOffsetY = 0f;

    private Rigidbody rb;
    private Animator animator;

    private bool isCasting = false;
    private float lastCastTime;

    public GameObject blueLaserVFX;

    private float currentRandomizedSpeed;
    private float attackRangeSqr;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        myHealth = GetComponent<EnemyHealth>();

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

        attackRangeSqr = attackRange * attackRange;
    }

    private void OnEnable()
    {
        lastCastTime = Time.time;
        isCasting = false;
        currentRandomizedSpeed = speed * Random.Range(0.8f, 1.2f);
    }

    void Update()
    {
        if (playerTransform == null || myHealth.IsDead) return;

        Vector3 offset = playerTransform.position - transform.position;
        float currentDistanceSqr = offset.sqrMagnitude;

        if (currentDistanceSqr <= attackRangeSqr && !isCasting)
        {
            if (Time.time >= lastCastTime + magicCooldown)
            {
                StartCoroutine(CastLightningRoutine());
            }
        }

        if (animator != null)
        {
            float animSpeed = (currentDistanceSqr > attackRangeSqr && !isCasting) ? currentRandomizedSpeed : 0f;
            animator.SetFloat("Speed", animSpeed);
        }
    }

    private void FixedUpdate()
    {
        if (playerTransform == null || myHealth.IsDead) return;

        Vector3 offset = playerTransform.position - transform.position;
        float currentDistanceSqr = offset.sqrMagnitude;

        if (currentDistanceSqr > attackRangeSqr && !isCasting)
        {
            Vector3 direction = offset;
            direction.y = 0;
            direction.Normalize();

            Vector3 newPosition = transform.position + direction * currentRandomizedSpeed * Time.fixedDeltaTime;

            if (Terrain.activeTerrain != null)
            {
                float terrainHeight = Terrain.activeTerrain.SampleHeight(newPosition);
                newPosition.y = terrainHeight + Terrain.activeTerrain.transform.position.y + groundOffsetY;
            }

            transform.position = newPosition;
            FaceTarget(direction);
        }
        else
        {
            if (!isCasting)
            {
                Vector3 direction = offset;
                direction.y = 0;
                direction.Normalize();
                FaceTarget(direction);
            }
        }
    }

    private void FaceTarget(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * 5f);
        }
    }

    private IEnumerator CastLightningRoutine()
    {
        isCasting = true;

        if (animator != null) animator.SetBool("Attack", true);
        if (blueLaserVFX != null) blueLaserVFX.SetActive(true);

        float timer = 0f;

        while (timer < castTime)
        {
            if (myHealth.IsDead)
            {
                isCasting = false;
                if (animator != null) animator.SetBool("Attack", false);
                if (blueLaserVFX != null) blueLaserVFX.SetActive(false);
                yield break;
            }

            timer += Time.deltaTime;

            Vector3 direction = (playerTransform.position - transform.position).normalized;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
            }

            yield return null;
        }

        if (animator != null) animator.SetBool("Attack", false);
        if (blueLaserVFX != null) blueLaserVFX.SetActive(false);

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(Mathf.Max(playerHealth.currentHealth * 0.05f, playerHealth.maxHealth * 0.01f));
        }

        lastCastTime = Time.time;
        isCasting = false;
    }

    private void OnDisable()
    {
        StopAllCoroutines();

        if (blueLaserVFX != null)
        {
            blueLaserVFX.SetActive(false);
        }
    }
}