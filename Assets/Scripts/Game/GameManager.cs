using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    private float duration;
    public float Duration => duration;

    public int CurrentLoop { get; private set; } = 1;

    private Dictionary<string, EnemyKillDTO> killsTracker;
    public int TotalEnemiesKilled => killsTracker.Values.Sum(k => k.count);
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        killsTracker = new Dictionary<string, EnemyKillDTO>();
    }

    void Update()
    {
        duration += Time.deltaTime;   
    }

    public void AdvanceLoop()
    {
        CurrentLoop++;
        Debug.Log("Entered Loop " + CurrentLoop + "!");
    }

    public void EnemyKilled(string enemyTypeId)
    {
        string key = enemyTypeId + "_" + CurrentLoop;

        if (killsTracker.ContainsKey(key))
        {
            killsTracker[key].count++;
        }
        else
        {
            killsTracker.Add(key, new EnemyKillDTO
            {
                enemyTypeId = enemyTypeId,
                count = 1,
                loopNumber = CurrentLoop
            });
        }
    }

    public void SaveRunData()
    {
        if (GameSessionManager.Instance == null || GameSessionManager.Instance.CurrentSessionId == null)
        {
            Debug.LogWarning("No active game session, skipping FinishGame");
            SceneManager.LoadScene("MainMenu");
            return;
        }

        FinishGameSessionRequest request = new FinishGameSessionRequest();
        request.gameSessionId = GameSessionManager.Instance.CurrentSessionId;
        request.durationSeconds = (int) duration;
        request.levelReached = ExperienceManager.Instance.CurrentLevel;

        request.enemiesKilled = killsTracker.Values.ToList();

        GameSessionManager.Instance.FinishGame(request, OnSuccess, OnError);
    }

    void OnSuccess(UserProfileDTO updatedProfile)
    {
        Debug.Log("Run saved!");

        ApiManager.Instance.SetProfile(updatedProfile);
   
        SceneManager.LoadScene("MainMenu");
    }

    void OnError(string error)
    {
        Debug.LogError(error);
    }
}
