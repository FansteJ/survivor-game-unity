using System;
using System.Collections.Generic;

[Serializable]
public class UpgradeShopItemDTO
{
    public string upgradeTypeId;
    public string name;
    public string description;
    public string effectType;
    public string currencyType;
    public float value;
    public int level;
    public int maxLevel;
    public long cost;
    public bool canBuy;
}

[Serializable]
public class BuyUpgradeResponse
{
    public int newLevel;
    public long remainingGold;
    public long remainingGems;
}

[Serializable]
public class PlayerModifiers
{
    public float goldMultiplier;
    public float xpMultiplier;
    public float damageMultiplier;
    public float startHpBonus;
    public int revives;
    public float luckMultiplier;
    public float speedBonus;
    public float hpRegen;
    public float lifesteal;
}