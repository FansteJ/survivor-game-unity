public enum NormalUpgradeType
{
    PlayerDamage, PlayerAttackSpeed, PlayerMaxHealth, Luck, XpGain, GoldGain, HealthRegeneration, Critical
}

public class NormalUpgradeOption : UpgradeOption
{
    public NormalUpgradeType upgradeType;
}
