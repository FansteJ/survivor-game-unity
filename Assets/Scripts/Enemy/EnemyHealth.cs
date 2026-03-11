using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    public string uuid;
    public float maxHealth;
    public float currentHealth;

    public int coinDrop;
    public float xpReward = 10f;

    public GameObject damageNumberPrefab;
    public GameObject xpOrbPrefab;
    public GameObject prefab;

    private Renderer[] renderers;
    private Color originalColor;
    private Animator animator;
    private EnemyController controller;
    private Collider col;
    private NavMeshAgent agent;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<EnemyController>();
        col = GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();

        renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
        {
            originalColor = renderers[0].material.color;
        }
    }

    private void OnEnable()
    {
        currentHealth = maxHealth;

        if (col != null) col.enabled = false;
        if (controller != null) controller.enabled = false;
        if (agent != null) agent.enabled = false;

        if (renderers != null)
        {
            foreach (Renderer r in renderers)
                if (r != null) r.material.color = originalColor;
        }

        StartCoroutine(WakeUpSequence());
    }

    private IEnumerator WakeUpSequence()
    {
        yield return null;

        if (animator != null)
        {
            animator.SetBool("IsDead", false);
            animator.ResetTrigger("Attack");
            animator.Play("idle", -1, 0f);
        }

        if (agent != null) agent.enabled = true;
        if (col != null) col.enabled = true;
        if (controller != null) controller.enabled = true;
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        GameObject dmgNum = PoolManager.Instance.Get(damageNumberPrefab, transform.position + Vector3.up * 1f);
        DamageNumber dn = dmgNum.GetComponent<DamageNumber>();
        dn.prefab = damageNumberPrefab;
        dn.text.text = $"-{damage:F1}";

        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        if (col != null) col.enabled = false;
        if (controller != null) controller.enabled = false;
        if (agent != null) agent.enabled = false;

        GameManager.Instance.EnemyKilled(uuid);
        CoinSpawner.Instance.SpawnCoins(coinDrop, transform.position);
        GameObject orb = PoolManager.Instance.Get(xpOrbPrefab, transform.position);
        XpOrb orbScript = orb.GetComponent<XpOrb>();
        
        orbScript.prefab = xpOrbPrefab;
        orbScript.xpAmount = xpReward;

        animator.SetBool("IsDead", true);

        StartCoroutine(DieRoutine());
    }

    private IEnumerator DieRoutine()
    {
        yield return new WaitForSeconds(3f);

        if (animator != null)
        {
            animator.SetBool("IsDead", false);
            animator.ResetTrigger("Attack");
            animator.Play("idle", -1, 0f);
            animator.Update(0f);
        }

        PoolManager.Instance.Return(prefab, gameObject);
    }

    private IEnumerator FlashRed()
    {
        foreach (Renderer r in renderers)
            r.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        foreach (Renderer r in renderers)
            r.material.color = originalColor;
    }
}