using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

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

    private Rarity GetRandomRarity()
    {
        int rolls = 1 + Mathf.FloorToInt(PlayerStats.Instance.LuckMultiplier / 5f);
        Rarity best = Rarity.Common;
        for (int i = 0; i < rolls; i++)
        {
            Rarity r = RollRarity();
            if (r > best) best = r;
        }
        return best;
    }

    private Rarity RollRarity()
    {
        float value = Random.value;
        Rarity rarity;

        if (value < 0.5)
        {
            rarity = Rarity.Common;
        }
        else if (value < 0.75)
        {
            rarity = Rarity.Uncommon;
        }
        else if (value < 0.9)
        {
            rarity = Rarity.Rare;
        }
        else if (value < 0.98)
        {
            rarity = Rarity.Epic;
        }
        else
        {
            rarity = Rarity.Legendary;
        }

        return rarity;
    }

    private UpgradeOption GetRandomUpgrade()
    {
        UpgradeOption upgrade = new UpgradeOption();
        upgrade.upgradeType =  (UpgradeType)Random.Range(0, System.Enum.GetValues(typeof(UpgradeType)).Length);
        upgrade.rarity = GetRandomRarity();

        float[] rarityMultipliers = { 1f, 2f, 4f, 7f, 10f };
        float multiplier = rarityMultipliers[(int)upgrade.rarity];

        switch (upgrade.upgradeType)
        {
            case UpgradeType.WeaponDamage:
                upgrade.value = 0.05f * multiplier;
                upgrade.name = "Weapon Damage";
                upgrade.description = $"Weapon damage: " +
                    $"{WeaponController.Instance.weapons[0].damage:F1} " +
                    $"-> {WeaponController.Instance.weapons[0].damage * (1 + upgrade.value):F1}";
                break;

            case UpgradeType.WeaponAttackSpeed:
                upgrade.value = 0.1f * multiplier;
                upgrade.name = "Attack Speed";
                upgrade.description = $"Weapon attack speed: " +
                    $"{WeaponController.Instance.weapons[0].attackSpeed:F1} " +
                    $"-> {WeaponController.Instance.weapons[0].attackSpeed * (1+ upgrade.value):F1}";
                break;

            case UpgradeType.PlayerDamage:
                upgrade.value = 0.05f * multiplier;
                upgrade.name = "Player Damage";
                upgrade.description = $"Player damage: {PlayerStats.Instance.DamageMultiplier:F1} " +
                    $"-> {PlayerStats.Instance.DamageMultiplier + upgrade.value:F1}";
                break;

            case UpgradeType.PlayerAttackSpeed:
                upgrade.value = 0.1f * multiplier;
                upgrade.name = "Player Attack Speed";
                upgrade.description = $"Player attack speed: {PlayerStats.Instance.AttackSpeedMultiplier:F1} " +
                    $"-> {PlayerStats.Instance.AttackSpeedMultiplier + upgrade.value:F1}";
                break;

            case UpgradeType.PlayerMaxHealth:
                upgrade.value = 5f * multiplier;
                upgrade.name = "Max Health";
                upgrade.description = $"Max health: {PlayerHealth.Instance.maxHealth:F0} " +
                    $"-> {PlayerHealth.Instance.maxHealth + upgrade.value:F0}";
                break;

            case UpgradeType.Luck:
                upgrade.value = 1f * multiplier;
                upgrade.name = "Luck";
                upgrade.description = $"Luck: {PlayerStats.Instance.LuckMultiplier:F0} " +
                    $"-> {PlayerStats.Instance.LuckMultiplier + upgrade.value:F0}";
                break;

            case UpgradeType.XpGain:
                upgrade.value = 0.05f * multiplier;
                upgrade.name = "XP Gain";
                upgrade.description = $"XP gain: {PlayerStats.Instance.XPGainMultiplier * 100:F0}% " +
                    $"-> {(PlayerStats.Instance.XPGainMultiplier + upgrade.value)*100:F0}%";
                break;

            case UpgradeType.GoldGain:
                upgrade.value = 0.05f * multiplier;
                upgrade.name = "Gold Gain";
                upgrade.description = $"Gold gain: {PlayerStats.Instance.GoldMultiplier * 100:F0}% " +
                    $"-> {(PlayerStats.Instance.GoldMultiplier + upgrade.value) * 100:F0}%";
                break;

            case UpgradeType.HealthRegeneration:
                upgrade.value = 1f * multiplier;
                upgrade.name = "Health Regen";
                upgrade.description = $"Health regen: {PlayerStats.Instance.HealthRegen:F0} hp/s " +
                    $"-> {PlayerStats.Instance.HealthRegen + upgrade.value:F0} hp/s";
                break;
        }

        return upgrade;
    }

    public List<UpgradeOption> GetThreeUpgrades()
    {
        List<UpgradeOption> upgrades = new List<UpgradeOption>();
        while (upgrades.Count < 3)
        {
            UpgradeOption newUpgrade = GetRandomUpgrade();
            bool isDuplicate = false;
            foreach (UpgradeOption upgrade in upgrades)
            {
                if (upgrade.upgradeType == newUpgrade.upgradeType)
                {
                    isDuplicate = true;
                    break;
                }
            }
            if (!isDuplicate)
                upgrades.Add(newUpgrade);
        }
        return upgrades;
    }

}
