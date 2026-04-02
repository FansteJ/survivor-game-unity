using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    public string enemyTypeId;
    public float maxHealth;
    private float baseMaxHealth;
    public float currentHealth;
    public bool IsDead => currentHealth <= 0f;

    public int coinDrop;
    public float xpReward = 10f;
    [Range(0f, 1f)] public float magnetDropChance = 0.005f;

    public GameObject damageNumberPrefab;
    public GameObject xpOrbPrefab;
    public GameObject prefab;
    public GameObject magnetPrefab;

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

        baseMaxHealth = maxHealth;
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

        bool isCrit = false;
        bool isLethal = false;

        if (PlayerStats.Instance.LethalStrikeChance > 0 && Random.value < PlayerStats.Instance.LethalStrikeChance)
        {
            isLethal = true;
            damage = currentHealth;
        }
        else if (Random.value < PlayerStats.Instance.CritChance)
        {
            isCrit = true;
            damage *= PlayerStats.Instance.CritDamage;
        }

        currentHealth -= damage;
        if(PlayerStats.Instance.LifeSteal > 0)
        {
            PlayerHealth.Instance.Heal(damage * PlayerStats.Instance.LifeSteal);
        }

        GameObject dmgNum = PoolManager.Instance.Get(damageNumberPrefab, transform.position + Vector3.up * 1f);
        DamageNumber dn = dmgNum.GetComponent<DamageNumber>();
        dn.prefab = damageNumberPrefab;

        if (isLethal)
        {
            dn.text.text = "LETHAL!";
            dn.text.color = Color.red;
        }
        else if (isCrit)
        {
            dn.text.text = $"{damage:F1}!";
            dn.text.color = Color.yellow;
        }
        else
        {
            dn.text.text = $"-{damage:F1}";
            dn.text.color = Color.white;
        }

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

        if (PlayerStats.Instance.Devourer > 0)
        {
            PlayerHealth.Instance.AddMaxHealth(PlayerStats.Instance.Devourer);
        }
        GameManager.Instance.EnemyKilled(enemyTypeId);
        CoinSpawner.Instance.SpawnCoins(coinDrop, transform.position);
        GameObject orb = PoolManager.Instance.Get(xpOrbPrefab, transform.position);
        XpOrb orbScript = orb.GetComponent<XpOrb>();
        
        orbScript.prefab = xpOrbPrefab;
        orbScript.xpAmount = xpReward;

        if (Random.value < magnetDropChance)
        {
            Instantiate(magnetPrefab, transform.position + Vector3.up * 0.5f, magnetPrefab.transform.rotation);
        }

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

    public void SetDifficultyParameters(float hpMultiplier, float dmgMultiplier)
    {
        maxHealth = baseMaxHealth * hpMultiplier;
        currentHealth = maxHealth;

        IDamageScaler damageScaler = GetComponent<IDamageScaler>();

        if (damageScaler != null)
        {
            damageScaler.SetDamageMultiplier(dmgMultiplier);
        }
    }
}