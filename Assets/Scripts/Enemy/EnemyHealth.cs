using System.Collections;
using TMPro;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public string uuid;
    public float maxHealth;
    public float currentHealth;

    public int coinDrop;

    private Renderer[] renderers;
    private Color originalColor;
    public GameObject damageNumberPrefab;

    private void Start()
    {
        currentHealth = maxHealth;

        renderers = GetComponentsInChildren<Renderer>();
        originalColor = renderers[0].material.color;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        GameObject dmgNum = Instantiate(damageNumberPrefab, transform.position + Vector3.up * 1f, Quaternion.identity);
        dmgNum.GetComponentInChildren<TMP_Text>().text = $"-{damage}";
        StartCoroutine(FlashRed());
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        GameManager.Instance.EnemyKilled(uuid);
        CoinSpawner.Instance.SpawnCoins(coinDrop, transform.position);

        GetComponent<Animator>().SetTrigger("Die");
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
