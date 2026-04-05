using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance { get; private set; }

    public float maxHealth;
    public float currentHealth;

    public event Action OnHealthChange;

    [Header("Regeneration Settings")]
    public float regenTickRate = 0.5f;
    private float regenTimer = 0f;

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
        if (PlayerStats.Instance.HealthRegen > 0 && currentHealth < maxHealth)
        {
            regenTimer += Time.deltaTime;
            if (regenTimer >= regenTickRate)
            {
                Heal(PlayerStats.Instance.HealthRegen * regenTickRate);
                regenTimer = 0f;
            }
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
        currentHealth = Mathf.Min(maxHealth, currentHealth);
        OnHealthChange?.Invoke();
    }

    public void Heal(float value)
    {
        if (currentHealth >= maxHealth) return;

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
