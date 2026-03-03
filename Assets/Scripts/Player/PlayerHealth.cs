using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void AddMaxHealth(float value)
    {
        maxHealth += value;
        currentHealth += value;
    }

    public void Heal(float value)
    {
        currentHealth = Mathf.Min(currentHealth + value, maxHealth);
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
        GameManager.Instance.GameOver();
    }
}
