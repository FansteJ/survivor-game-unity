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

    private NormalUpgradeOption GetRandomUpgrade()
    {
        NormalUpgradeOption upgrade = new NormalUpgradeOption();
        upgrade.upgradeType =  (NormalUpgradeType)Random.Range(0, System.Enum.GetValues(typeof(NormalUpgradeType)).Length);
        upgrade.rarity = GetRandomRarity();

        int multiplier = (int)upgrade.rarity + 1;

        switch (upgrade.upgradeType)
        {
            case NormalUpgradeType.PlayerDamage:
                upgrade.value = 0.05f * multiplier;
                upgrade.name = "Player Damage";
                upgrade.description = $"Player damage: {PlayerStats.Instance.DamageMultiplier:F2} " +
                    $"-> {PlayerStats.Instance.DamageMultiplier + upgrade.value:F2}";
                break;

            case NormalUpgradeType.PlayerAttackSpeed:
                upgrade.value = 0.1f * multiplier;
                upgrade.name = "Player Attack Speed";
                upgrade.description = $"Player attack speed: " +
                    $"{PlayerStats.Instance.AttackSpeedMultiplier:F2} " +
                    $"-> {PlayerStats.Instance.AttackSpeedMultiplier + upgrade.value:F2}";
                break;

            case NormalUpgradeType.PlayerMaxHealth:
                upgrade.value = 50f * multiplier;
                upgrade.name = "Max Health";
                upgrade.description = $"Max health: {PlayerHealth.Instance.maxHealth:F0} " +
                    $"-> {PlayerHealth.Instance.maxHealth + upgrade.value:F0}";
                break;

            case NormalUpgradeType.Luck:
                upgrade.value = 1f * multiplier;
                upgrade.name = "Luck";
                upgrade.description = $"Luck: {PlayerStats.Instance.LuckMultiplier:F0} " +
                    $"-> {PlayerStats.Instance.LuckMultiplier + upgrade.value:F0}";
                break;

            case NormalUpgradeType.XpGain:
                upgrade.value = 0.1f * multiplier;
                upgrade.name = "XP Gain";
                upgrade.description = $"XP gain: {PlayerStats.Instance.XPGainMultiplier * 100:F0}% " +
                    $"-> {(PlayerStats.Instance.XPGainMultiplier + upgrade.value)*100:F0}%";
                break;

            case NormalUpgradeType.GoldGain:
                upgrade.value = 0.1f * multiplier;
                upgrade.name = "Gold Gain";
                upgrade.description = $"Gold gain: {PlayerStats.Instance.GoldMultiplier * 100:F0}% " +
                    $"-> {(PlayerStats.Instance.GoldMultiplier + upgrade.value) * 100:F0}%";
                break;

            case NormalUpgradeType.HealthRegeneration:
                upgrade.value = 1f * multiplier;
                upgrade.name = "Health Regen";
                upgrade.description = $"Health regen: {PlayerStats.Instance.HealthRegen:F0} hp/s " +
                    $"-> {PlayerStats.Instance.HealthRegen + upgrade.value:F0} hp/s";
                break;
            case NormalUpgradeType.Critical:
                upgrade.value = multiplier;
                upgrade.name = "Critical Mastery";

                float chanceGain = upgrade.value * 0.05f * (1f - PlayerStats.Instance.CritChance);
                float damageGain = upgrade.value * 0.1f;

                float currentChancePct = PlayerStats.Instance.CritChance * 100f;
                float nextChancePct = (PlayerStats.Instance.CritChance + chanceGain) * 100f;

                float currentDmgPct = PlayerStats.Instance.CritDamage * 100f;
                float nextDmgPct = (PlayerStats.Instance.CritDamage + damageGain) * 100f;

                upgrade.description = $"Critical Chance: {currentChancePct:F1}% -> {nextChancePct:F1}%\n" +
                                      $"Critical Damage: {currentDmgPct:F0}% -> {nextDmgPct:F0}%";
                break;
        }

        return upgrade;
    }

    private SpecialUpgradeOption GetRandomSpecialUpgrade()
    {
        SpecialUpgradeOption upgrade = new SpecialUpgradeOption();
        upgrade.upgradeType = (SpecialUpgradeType)Random.Range(0, System.Enum.GetValues(typeof(SpecialUpgradeType)).Length);
        upgrade.rarity = GetRandomRarity();

        int multiplier = (int)upgrade.rarity + 1;

        switch (upgrade.upgradeType)
        {
            case SpecialUpgradeType.Coinflip:
                upgrade.value = 50f * multiplier;
                upgrade.name = "Coinflip";
                upgrade.description = $"70/30: Win or lose {upgrade.value:F0} coins";
                break;
            case SpecialUpgradeType.BloodPact:
                upgrade.value = multiplier;
                upgrade.name = "Blood Pact";
                upgrade.description = $"+{upgrade.value * 10:F0}% damage, -{30:F0}% max HP";
                break;
            case SpecialUpgradeType.Vampirism:
                upgrade.value = 0.03f * multiplier;
                upgrade.name = "Vampirism";
                upgrade.description = $"Life steal: {PlayerStats.Instance.LifeSteal:F2} " +
                    $"-> {PlayerStats.Instance.LifeSteal + upgrade.value:F2}";
                break;
            case SpecialUpgradeType.Devourer:
                upgrade.value = 0.5f * multiplier;
                upgrade.name = "Devourer";
                upgrade.description = $"Max HP on kill: +{PlayerStats.Instance.Devourer:F0} " +
                    $"-> +{PlayerStats.Instance.Devourer + upgrade.value:F0}";
                break;
            case SpecialUpgradeType.LethalStrike:
                upgrade.value = multiplier;
                upgrade.name = "Lethal Strike";

                float lethalGain = upgrade.value * 0.01f * (1f - PlayerStats.Instance.LethalStrikeChance);

                float currentLethalPct = PlayerStats.Instance.LethalStrikeChance * 100f;
                float nextLethalPct = (PlayerStats.Instance.LethalStrikeChance + lethalGain) * 100f;

                upgrade.description = $"Instant Kill Chance: {currentLethalPct:F1}% -> {nextLethalPct:F1}%";
                break;
        }
        return upgrade;
    }

    public List<UpgradeOption> GetLevelUpUpgrades()
    {
        List<UpgradeOption> upgrades = new List<UpgradeOption>();
        while (upgrades.Count < 2)
        {
            SpecialUpgradeOption newUpgrade = GetRandomSpecialUpgrade();
            bool isDuplicate = false;
            foreach (SpecialUpgradeOption upgrade in upgrades)
            {
                if(upgrade.upgradeType == newUpgrade.upgradeType)
                {
                    isDuplicate = true;
                    break;
                }
            }
            if (!isDuplicate)
            {
                upgrades.Add(newUpgrade);
            }
        }
        upgrades.Add(GetRandomUpgrade());
        return upgrades;
    }

    public List<UpgradeOption> GetThreeUpgrades()
    {
        List<UpgradeOption> upgrades = new List<UpgradeOption>();

        upgrades.Add(GetRandomWeaponUpgrade());

        while (upgrades.Count < 3)
        {
            NormalUpgradeOption newUpgrade = GetRandomUpgrade();
            bool isDuplicate = false;
            foreach (var upgrade in upgrades)
            {
                if (upgrade is NormalUpgradeOption normalUpgrade && normalUpgrade.upgradeType == newUpgrade.upgradeType)
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

    private WeaponUpgradeOption GetRandomWeaponUpgrade()
    {
        WeaponUpgradeOption upgrade = new WeaponUpgradeOption();
        upgrade.rarity = GetRandomRarity();

        List<WeaponBase> allWeapons = WeaponController.Instance.allWeapons;
        WeaponBase chosenWeapon = allWeapons[Random.Range(0, allWeapons.Count)];

        upgrade.targetWeapon = chosenWeapon;
        upgrade.isUnlock = !chosenWeapon.gameObject.activeInHierarchy;

        int multiplier = (int)upgrade.rarity + 1;

        if (upgrade.isUnlock)
        {
            upgrade.name = $"Unlock {chosenWeapon.name}";
            upgrade.description = $"Equip the mighty {chosenWeapon.name}!";
        }
        else
        {
            upgrade.name = $"{chosenWeapon.weaponName} LvL {chosenWeapon.currentLevel + multiplier}";
            upgrade.description = chosenWeapon.GetNextLevelDescription(multiplier);
            upgrade.multiplier = multiplier;
        }

        return upgrade;
    }

}
