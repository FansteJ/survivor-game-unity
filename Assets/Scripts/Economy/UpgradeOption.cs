public enum UpgradeType { WeaponDamage, WeaponAttackSpeed, 
    PlayerDamage, PlayerAttackSpeed, PlayerMaxHealth, Luck, XpGain, GoldGain, HealthRegeneration }

public class UpgradeOption
{
    public UpgradeType upgradeType;
    public Rarity rarity;
    public string name;
    public string description;
    public float value;
}
