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
        float value = Random.value;
        Rarity rarity;

        if (value < 0.5)
        {
            rarity = Rarity.Common;
        } else if(value < 0.75)
        {
            rarity = Rarity.Uncommon;
        } else if(value < 0.9)
        {
            rarity = Rarity.Rare;
        } else if(value < 0.98)
        {
            rarity = Rarity.Epic;
        } else
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
                upgrade.description = $"+{upgrade.value} damage";
                break;
            case UpgradeType.WeaponAttackSpeed:
                upgrade.value = 0.1f * multiplier;
                upgrade.name = "Attack Speed";
                upgrade.description = $"+{upgrade.value} attack speed";
                break;
            case UpgradeType.PlayerDamage:
                upgrade.value = 0.05f * multiplier;
                upgrade.name = "Player Damage";
                upgrade.description = $"+{upgrade.value}% damage";
                break;
            case UpgradeType.PlayerAttackSpeed:
                upgrade.value = 0.1f * multiplier;
                upgrade.name = "Player Attack Speed";
                upgrade.description = $"+{upgrade.value} attack speed";
                break;
            case UpgradeType.PlayerMaxHealth:
                upgrade.value = 5f * multiplier;
                upgrade.name = "Max Health";
                upgrade.description = $"+{upgrade.value} max health";
                break;
            case UpgradeType.Luck:
                upgrade.value = 1f * multiplier;
                upgrade.name = "Luck";
                upgrade.description = $"+{upgrade.value} luck";
                break;
            case UpgradeType.XpGain:
                upgrade.value = 5f * multiplier;
                upgrade.name = "XP Gain";
                upgrade.description = $"+{upgrade.value}% xp gain";
                break;
            case UpgradeType.GoldGain:
                upgrade.value = 5f * multiplier;
                upgrade.name = "Gold Gain";
                upgrade.description = $"+{upgrade.value}% gold gain";
                break;
            case UpgradeType.HealthRegeneration:
                upgrade.value = 1f * multiplier;
                upgrade.name = "Health Regen";
                upgrade.description = $"+{upgrade.value} hp/s";
                break;
        }

        return upgrade;
    }

    public List<UpgradeOption> GetThreeUpgrades()
    {
        List<UpgradeOption> upgrades = new List<UpgradeOption>();
        for (int i = 0; i < 3; i++)
        {
            upgrades.Add(GetRandomUpgrade());
        }
        return upgrades;
    }

}
