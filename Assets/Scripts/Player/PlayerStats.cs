using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    private float damageMultiplier = 1f;
    private float attackSpeedMultiplier = 1f;
    private float luck = 0f;
    private float xpGainMultiplier = 1f;
    private float goldGainMultiplier = 1f;
    private float healthRegen = 0f;

    public float DamageMultiplier => damageMultiplier;
    public float AttackSpeedMultiplier => attackSpeedMultiplier;
    public float LuckMultiplier => luck;
    public float XPGainMultiplier => xpGainMultiplier;
    public float GoldMultiplier => goldGainMultiplier;
    public float HealthRegen => healthRegen;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddDamageMultiplier(float value) { damageMultiplier += value; }
    public void AddAttackSpeedMultiplier(float value) { attackSpeedMultiplier += value; }
    public void AddLuck(float value) { luck += value; }
    public void AddXPGainMultiplier(float value) { xpGainMultiplier += value; }
    public void AddGoldMultiplier(float value) { goldGainMultiplier += value; }
    public void AddHealthRegen(float value) { healthRegen += value; }
    public void AddMaxHealth(float value) { GetComponent<PlayerHealth>().AddMaxHealth(value); }
}
