using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    private float damageMultiplier = 1f;
    private float attackSpeedMultiplier = 1f;
    private float luck = 0f;
    private float xpGainMultiplier = 1f;
    private float goldGainMultiplier = 1f;
    private float healthRegen = 0.5f;
    private float lifeSteal = 0f;
    private float devourer = 0f;
    private float critChance = 0.05f;
    private float critDamage = 2.0f;
    private float lethalStrikeChance = 0.00f;

    public float DamageMultiplier => damageMultiplier;
    public float AttackSpeedMultiplier => attackSpeedMultiplier;
    public float LuckMultiplier => luck;
    public float XPGainMultiplier => xpGainMultiplier;
    public float GoldMultiplier => goldGainMultiplier;
    public float HealthRegen => healthRegen;
    public float LifeSteal => lifeSteal;
    public float Devourer => devourer;
    public float CritChance => critChance;
    public float CritDamage => critDamage;
    public float LethalStrikeChance => lethalStrikeChance;

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

    public void AddDamageMultiplier(float value) { damageMultiplier += value; }
    public void AddAttackSpeedMultiplier(float value) { attackSpeedMultiplier += value; }
    public void AddLuck(float value) { luck += value; }
    public void AddXPGainMultiplier(float value) { xpGainMultiplier += value; }
    public void AddGoldMultiplier(float value) { goldGainMultiplier += value; }
    public void AddHealthRegen(float value) { healthRegen += value; }
    public void AddMaxHealth(float value) { GetComponent<PlayerHealth>().AddMaxHealth(value); }
    public void AddLifeSteal(float value) { lifeSteal += value; }
    public void AddDevourer(float value) {  devourer += value; }
    public void AddCritChance(float value) { critChance += value; }
    public void AddCritDamage(float value) { critDamage += value; }
    public void AddLethalStrikeChance(float value) { lethalStrikeChance += value; }
}
