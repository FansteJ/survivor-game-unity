using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    private float duration;
    public float Duration => duration;
    private Dictionary<string, int> enemiesKilled;
    public int TotalEnemiesKilled => enemiesKilled.Values.Sum();
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemiesKilled = new Dictionary<string, int>();
    }

    // Update is called once per frame
    void Update()
    {
        duration += Time.deltaTime;   
    }

    public void EnemyKilled(string enemyTypeId)
    {
        if (enemiesKilled.ContainsKey(enemyTypeId))
            enemiesKilled[enemyTypeId]++;
        else
            enemiesKilled.Add(enemyTypeId, 1);
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

        List<EnemyKillDTO> enemyKillDTOs = new List<EnemyKillDTO>();
        foreach (string key in enemiesKilled.Keys)
        {
            enemyKillDTOs.Add(new EnemyKillDTO { enemyTypeId = key, count = enemiesKilled[key] }); 
        }

        request.enemiesKilled = enemyKillDTOs;
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
