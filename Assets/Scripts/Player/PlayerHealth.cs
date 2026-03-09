using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance { get; private set; }

    public float maxHealth;
    public float currentHealth;

    public event Action OnHealthChange;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            currentHealth = maxHealth;
        } else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if(PlayerStats.Instance.HealthRegen > 0)
        {
            Heal(PlayerStats.Instance.HealthRegen * Time.deltaTime);
        }
    }

    public void AddMaxHealth(float value)
    {
        maxHealth += value;
        currentHealth += value;
        OnHealthChange?.Invoke();
    }

    public void RemoveMaxHealth(float value)
    {
        maxHealth -= value;
        OnHealthChange?.Invoke();
    }

    public void Heal(float value)
    {
        currentHealth = Mathf.Min(currentHealth + value, maxHealth);
        OnHealthChange?.Invoke();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        OnHealthChange?.Invoke();
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        Time.timeScale = 0;
        GameOverUIManager.Instance.ShowGameOver();
    }
}
