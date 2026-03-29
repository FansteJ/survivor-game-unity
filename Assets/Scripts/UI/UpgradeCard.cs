using UnityEngine;

public class UpgradeCard : MonoBehaviour
{
    public UpgradeOption upgrade;

    public void OnClick()
    {
        if (upgrade is NormalUpgradeOption normal)
        {
            ApplyNormalUpgrade(normal);
        }
        else if (upgrade is SpecialUpgradeOption special)
        {
            ApplySpecialUpgrade(special);
        } else if(upgrade is WeaponUpgradeOption weaponUpgrade)
        {
            ApplyWeaponUpgrade(weaponUpgrade);
        }

        ChestUIManager.Instance.HideChest();
    }

    private void ApplyNormalUpgrade(NormalUpgradeOption upgrade)
    {
        switch (upgrade.upgradeType)
        {
            case NormalUpgradeType.PlayerDamage:
                PlayerStats.Instance.AddDamageMultiplier(upgrade.value);
                break;
            case NormalUpgradeType.PlayerAttackSpeed:
                PlayerStats.Instance.AddAttackSpeedMultiplier(upgrade.value);
                break;
            case NormalUpgradeType.PlayerMaxHealth:
                PlayerStats.Instance.AddMaxHealth(upgrade.value);
                break;
            case NormalUpgradeType.Luck:
                PlayerStats.Instance.AddLuck(upgrade.value);
                break;
            case NormalUpgradeType.XpGain:
                PlayerStats.Instance.AddXPGainMultiplier(upgrade.value);
                break;
            case NormalUpgradeType.GoldGain:
                PlayerStats.Instance.AddGoldMultiplier(upgrade.value);
                break;
            case NormalUpgradeType.HealthRegeneration:
                PlayerStats.Instance.AddHealthRegen(upgrade.value);
                break;
        }
    }

    private void ApplySpecialUpgrade(SpecialUpgradeOption upgrade)
    {
        switch (upgrade.upgradeType)
        {
            case SpecialUpgradeType.Coinflip:
                if (Random.value > 0.3f)
                    CoinManager.Instance.AddCoin((int)upgrade.value);
                else
                    CoinManager.Instance.SpendCoins((int)upgrade.value);
                break;
            case SpecialUpgradeType.BloodPact:
                PlayerStats.Instance.AddDamageMultiplier(PlayerStats.Instance.DamageMultiplier * upgrade.value * 0.1f);
                PlayerHealth.Instance.RemoveMaxHealth(PlayerHealth.Instance.maxHealth * 0.3f);
                break;
            case SpecialUpgradeType.Vampirism:
                PlayerStats.Instance.AddLifeSteal(upgrade.value);
                break;
            case SpecialUpgradeType.Devourer:
                PlayerStats.Instance.AddDevourer(upgrade.value);
                break;
        }
    }

    private void ApplyWeaponUpgrade(WeaponUpgradeOption upgrade)
    {
        if (upgrade.isUnlock)
        {
            upgrade.targetWeapon.gameObject.SetActive(true);
            WeaponController.Instance.RefreshActiveWeapons();
        }
        else
        {
            upgrade.targetWeapon.damage += upgrade.damageIncrease;
            upgrade.targetWeapon.attackSpeed += upgrade.speedIncrease;
        }
    }
}
