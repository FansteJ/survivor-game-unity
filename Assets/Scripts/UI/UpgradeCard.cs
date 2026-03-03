using UnityEngine;

public class UpgradeCard : MonoBehaviour
{
    public UpgradeOption upgrade;

    public void OnClick()
    {
        switch (upgrade.upgradeType)
        {
            case UpgradeType.WeaponDamage:
                WeaponController.Instance.ApplyDamageUpgrade(upgrade.value);
                break;
            case UpgradeType.WeaponAttackSpeed:
                WeaponController.Instance.ApplyAttackSpeedUpgrade(upgrade.value);
                break;
            case UpgradeType.PlayerDamage:
                PlayerStats.Instance.AddDamageMultiplier(upgrade.value);
                break;
            case UpgradeType.PlayerAttackSpeed:
                PlayerStats.Instance.AddAttackSpeedMultiplier(upgrade.value);
                break;
            case UpgradeType.PlayerMaxHealth:
                PlayerStats.Instance.AddMaxHealth(upgrade.value);
                break;
            case UpgradeType.Luck:
                PlayerStats.Instance.AddLuck(upgrade.value);
                break;
            case UpgradeType.XpGain:
                PlayerStats.Instance.AddXPGainMultiplier(upgrade.value);
                break;
            case UpgradeType.GoldGain:
                PlayerStats.Instance.AddGoldMultiplier(upgrade.value);
                break;
            case UpgradeType.HealthRegeneration:
                PlayerStats.Instance.AddHealthRegen(upgrade.value);
                break;
        }
        ChestUIManager.Instance.HideChest();
    }
}
