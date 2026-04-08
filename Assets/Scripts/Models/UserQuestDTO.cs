using System;
using System.Collections.Generic;

[Serializable]
public class UserQuestDTO
{
    public string id;
    public QuestTypeDTO questType;
    public long progress;
    public bool completed;
    public bool claimed;
}

[Serializable]
public class QuestTypeDTO
{
    public string id;
    public string name;
    public string description;
    public string goalType;
    public long goal;
    public long rewardGems;
}

[Serializable]
public class ClaimRewardResponse
{
    public long newGemBalance;
}