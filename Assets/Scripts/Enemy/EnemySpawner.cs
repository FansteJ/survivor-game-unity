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
    [Header("Wave Settings")]
    public Wave[] waves;

    [Header("Spawner Settings")]
    public Transform playerTransform;
    public float spawnRadius = 20f;

    private int currentWaveIndex = 0;
    private float waveTimer = 0f;
    private float spawnTimer = 0f;

    void Update()
    {
        if (currentWaveIndex >= waves.Length)
            currentWaveIndex--;

        Wave currentWave = waves[currentWaveIndex];

        waveTimer += Time.deltaTime;
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= currentWave.spawnInterval)
        {
            spawnTimer -= currentWave.spawnInterval;
            SpawnRandomEnemyFromWave(currentWave);
        }

        if (waveTimer >= currentWave.waveDuration)
        {
            currentWaveIndex++;
            waveTimer = 0f;

            if (currentWaveIndex < waves.Length)
                Debug.Log($"New wave begins: {waves[currentWaveIndex].waveName}");
        }
    }

    private void SpawnRandomEnemyFromWave(Wave wave)
    {
        if (wave.enemiesToSpawn.Length == 0) return;

        GameObject randomEnemyPrefab = wave.enemiesToSpawn[Random.Range(0, wave.enemiesToSpawn.Length)];

        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector3 spawnPos = new Vector3(
            playerTransform.position.x + Mathf.Cos(angle) * spawnRadius,
            playerTransform.position.y + 0.1f,
            playerTransform.position.z + Mathf.Sin(angle) * spawnRadius
        );

        SpawnGameObject(randomEnemyPrefab, spawnPos);
    }

    private void SpawnGameObject(GameObject enemyPrefab, Vector3 position)
    {
        NavMeshHit hit;

        if (NavMesh.SamplePosition(position, out hit, 2f, NavMesh.AllAreas))
        {
            GameObject spawnedEnemy = PoolManager.Instance.Get(enemyPrefab, hit.position);
            EnemyHealth health = spawnedEnemy.GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.prefab = enemyPrefab;
            }
        }
    }
}