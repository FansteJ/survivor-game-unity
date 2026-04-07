using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class Wave
{
    public string waveName;
    public float waveDuration;
    public float spawnInterval;
    public GameObject[] enemiesToSpawn;
}

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    [Header("Wave Settings")]
    public Wave[] waves;

    [Header("Spawner Settings")]
    public Transform playerTransform;
    public float spawnRadius = 20f;

    private int currentWaveIndex = 0;
    private float waveTimer = 0f;
    private float spawnTimer = 0f;

    [Header("Endless Mode Settings")]
    public float currentHpMultiplier = 1f;
    public float currentDmgMultiplier = 1f;
    public const int START_DIFFICULTY = 1;
    public int difficulty = START_DIFFICULTY;

    public bool isWaitingForBossDeath = false;

    private void Awake()
    {
        Instance = this;
    }
    void Update()
    {
        if (isWaitingForBossDeath) return;

        if (currentWaveIndex >= waves.Length)
        {
            AdvanceToNextLoop();
        }

        Wave currentWave = waves[currentWaveIndex];

        waveTimer += Time.deltaTime;
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= currentWave.spawnInterval)
        {
            spawnTimer -= currentWave.spawnInterval;
            for(int i = 0; i < difficulty; i++)
            {
                SpawnRandomEnemyFromWave(currentWave);
                if (currentWaveIndex == waves.Length-1)
                    break;
            }
            difficulty++;
        }

        if (waveTimer >= currentWave.waveDuration)
        {
            currentWaveIndex++;
            waveTimer = 0f;
            difficulty = 10;

            if (currentWaveIndex < waves.Length)
                Debug.Log($"New wave begins: {waves[currentWaveIndex].waveName}");
        }
    }

    public void AdvanceToNextLoop()
    {
        isWaitingForBossDeath = false;
        currentWaveIndex = 0;
        waveTimer = 0f;
        spawnTimer = 0f;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AdvanceLoop();
        }

        currentHpMultiplier *= 2.5f;
        currentDmgMultiplier *= 2.5f;
        difficulty = START_DIFFICULTY;
    }

    private void SpawnRandomEnemyFromWave(Wave wave)
    {
        if (wave.enemiesToSpawn.Length == 0) return;

        GameObject randomEnemyPrefab = wave.enemiesToSpawn[Random.Range(0, wave.enemiesToSpawn.Length)];

        int maxAttempts = 10;

        for (int i = 0; i < maxAttempts; i++)
        {
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            Vector3 spawnPos = new Vector3(
                playerTransform.position.x + Mathf.Cos(angle) * spawnRadius,
                playerTransform.position.y,
                playerTransform.position.z + Mathf.Sin(angle) * spawnRadius
            );

            if (SpawnGameObject(randomEnemyPrefab, spawnPos))
            {
                return;
            }
        }
    }

    private bool SpawnGameObject(GameObject enemyPrefab, Vector3 position)
    {
        Vector3 rayStart = new Vector3(position.x, playerTransform.position.y + 50f, position.z);

        if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, 100f))
        {
            Vector3 finalSpawnPos = hit.point + Vector3.up * 1f;

            GameObject spawnedEnemy = PoolManager.Instance.Get(enemyPrefab, finalSpawnPos);
            EnemyHealth health = spawnedEnemy.GetComponent<EnemyHealth>();

            if (health != null)
            {
                health.prefab = enemyPrefab;
                health.SetDifficultyParameters(currentHpMultiplier, currentDmgMultiplier);
            }

            if (spawnedEnemy.GetComponent<EnemyBoss>() != null)
            {
                isWaitingForBossDeath = true;
            }

            return true;
        }

        return false;
    }
}