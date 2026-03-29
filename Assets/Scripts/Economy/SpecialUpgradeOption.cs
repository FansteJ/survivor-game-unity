public enum SpecialUpgradeType
{
    Coinflip, BloodPact, Vampirism, Devourer
}

public class SpecialUpgradeOption : UpgradeOption
{
    public SpecialUpgradeType upgradeType;
}
