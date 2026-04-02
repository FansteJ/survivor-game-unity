using System;
using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    public static ExperienceManager Instance { get; private set; }
    private float currentXP;
    public float CurrentXP => currentXP;
    private int currentLevel;
    public int CurrentLevel => currentLevel;

    public event Action OnXPChanged;
    public event Action OnLevelUpEvent;

    private void Awake()
    {
        if(Instance == null)
        {   
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddXP(float amount)
    {
        currentXP += amount * PlayerStats.Instance.XPGainMultiplier;
        while(currentXP >= GetXPForNextLevel())
        {
            currentXP -= GetXPForNextLevel();
            currentLevel++;
            OnLevelUp();
        }
        OnXPChanged?.Invoke();
    }

    public float GetXPForNextLevel()
    {
        return 100 * Mathf.Pow(currentLevel+1, 1.5f);
    }

    private void OnLevelUp()
    {
        PlayerHealth.Instance.AddMaxHealth(0.1f * PlayerHealth.Instance.maxHealth);
        ChestUIManager.Instance.ShowChest(UpgradeManager.Instance.GetLevelUpUpgrades());
        OnLevelUpEvent?.Invoke();
    }
}
