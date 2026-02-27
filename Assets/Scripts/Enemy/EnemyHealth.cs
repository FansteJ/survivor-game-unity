using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public string uuid;
    public float maxHealth;
    public float currentHealth;

    public int coinDrop;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
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
        Destroy(gameObject);
    }
}
