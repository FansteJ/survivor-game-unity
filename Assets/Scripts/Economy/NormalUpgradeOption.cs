public enum NormalUpgradeType
{
    WeaponDamage, WeaponAttackSpeed,
    PlayerDamage, PlayerAttackSpeed, PlayerMaxHealth, Luck, XpGain, GoldGain, HealthRegeneration
}

public class NormalUpgradeOption : UpgradeOption
{
    public NormalUpgradeType upgradeType;
}
