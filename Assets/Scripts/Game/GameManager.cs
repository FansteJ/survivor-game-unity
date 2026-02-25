using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    private float duration;
    private Dictionary<string, int> enemiesKilled;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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

    void EnemyKilled(string uuid)
    {
        if (enemiesKilled.ContainsKey(uuid))
            enemiesKilled[uuid]++;
        else
            enemiesKilled.Add(uuid, 1);
    }

    public void GameOver()
    {
        Time.timeScale = 0;

        if (GameSessionManager.Instance == null || GameSessionManager.Instance.CurrentSessionId == null)
        {
            Debug.LogWarning("No active game session, skipping FinishGame");
            SceneManager.LoadScene("MainMenu");
            return;
        }

        FinishGameSessionRequest request = new FinishGameSessionRequest();
        request.gameSessionId = GameSessionManager.Instance.CurrentSessionId;
        request.durationSeconds = (int) duration;
        request.levelReached = 1;

        List<EnemyKillDTO> enemyKillDTOs = new List<EnemyKillDTO>();
        foreach (string key in enemiesKilled.Keys)
        {
            enemyKillDTOs.Add(new EnemyKillDTO { enemyTypeId = key, count = enemiesKilled[key] }); 
        }

        request.enemiesKilled = enemyKillDTOs;
        GameSessionManager.Instance.FinishGame(request, OnSuccess, OnError);
    }

    void OnSuccess()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    void OnError(string error)
    {
        Debug.LogError(error);
    }
}
