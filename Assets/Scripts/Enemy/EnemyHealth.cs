using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public string uuid;
    public float maxHealth;
    public float currentHealth;

    public int coinDrop;
    public float xpReward = 10f;

    private Renderer[] renderers;
    private Color originalColor;
    public GameObject damageNumberPrefab;
    private Animator animator;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        renderers = GetComponentsInChildren<Renderer>();
        originalColor = renderers[0].material.color;
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth <= 0)
        {
            return;
        }
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
        Collider col = GetComponent<Collider>();
        col.enabled = false;
        GetComponent<EnemyController>().enabled = false;
        this.enabled = false;

        GameManager.Instance.EnemyKilled(uuid);
        CoinSpawner.Instance.SpawnCoins(coinDrop, transform.position);
        ExperienceManager.Instance.AddXP(xpReward);

        animator.SetTrigger("Die");
        Destroy(gameObject, 1f);
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
