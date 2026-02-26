using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public string uuid;
    public float maxHealth;
    public float currentHealth;

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
        Destroy(gameObject);
    }
}
