
using System.Collections.Generic;

[System.Serializable]
public class EnemyKillDTO
{
    public string enemyTypeId;
    public int count;
    public int loopNumber;
}

[System.Serializable]
public class FinishGameSessionRequest
{
    public string gameSessionId;
    public int durationSeconds;
    public int levelReached;
    public List<EnemyKillDTO> enemiesKilled;
}

[System.Serializable]
public class StartGameSessionResponse
{
    public string gameSessionId;
}
