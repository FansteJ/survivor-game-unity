public enum SpecialUpgradeType
{
    Coinflip, BloodPact
}

public class SpecialUpgradeOption : UpgradeOption
{
    public SpecialUpgradeType upgradeType;
}
