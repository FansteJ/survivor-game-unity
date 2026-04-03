using UnityEngine;
using UnityEngine.AI;
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

    private NavMeshAgent agent;
    private Animator animator;

    private bool isCasting = false;
    private float lastCastTime;

    public GameObject blueLaserVFX;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        myHealth = GetComponent<EnemyHealth>();

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerHealth = playerTransform.GetComponent<PlayerHealth>();
        }
    }

    private void OnEnable()
    {
        lastCastTime = Time.time;
        isCasting = false;

        if (agent != null)
        {
            agent.speed = speed;
            agent.stoppingDistance = attackRange;
            agent.enabled = true;
            agent.ResetPath();
        }
    }

    void Update()
    {
        if (!agent.enabled || playerTransform == null || isCasting || myHealth.IsDead) return;

        agent.SetDestination(playerTransform.position);

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= attackRange)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            FaceTarget();

            if (Time.time >= lastCastTime + magicCooldown)
            {
                StartCoroutine(CastLightningRoutine());
            }
        }
        else
        {
            agent.isStopped = false;
        }

        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    private void FaceTarget()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private IEnumerator CastLightningRoutine()
    {
        isCasting = true;
        agent.isStopped = true;

        if (animator != null)
        {
            animator.SetBool("Attack", true);
        }

        if (blueLaserVFX != null)
        {
            blueLaserVFX.SetActive(true);
        }

        // TODO show red circle around player

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

            FaceTarget();

            yield return null;
        }

        if (animator != null) animator.SetBool("Attack", false);
        if (blueLaserVFX != null) blueLaserVFX.SetActive(false);

        if (playerHealth != null)
        {
            // TODO strike player with lightning
            playerHealth.TakeDamage(Mathf.Max(playerHealth.currentHealth * 0.05f, playerHealth.maxHealth * 0.01f));
        }

        lastCastTime = Time.time;
        isCasting = false;

        if (agent.enabled) agent.isStopped = false;
    }

    private void OnDisable()
    {
        if (agent != null) agent.enabled = false;
        StopAllCoroutines();
    }
}