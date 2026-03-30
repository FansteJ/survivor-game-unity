public enum SpecialUpgradeType
{
    Coinflip, BloodPact, Vampirism, Devourer, LethalStrike
}

public class SpecialUpgradeOption : UpgradeOption
{
    public SpecialUpgradeType upgradeType;
}
