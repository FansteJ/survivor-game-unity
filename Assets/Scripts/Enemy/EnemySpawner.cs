using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform playerTransform;
    public float spawnRadius;
    public float spawnInterval;
    public float timeSinceStart;
    public float currentTime;

    void Update()
    {
        timeSinceStart += Time.deltaTime;
        currentTime += Time.deltaTime;

        if (currentTime >= spawnInterval)
        {
            currentTime -= spawnInterval;
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            Vector3 spawnPos = new Vector3(
                playerTransform.position.x + Mathf.Cos(angle) * spawnRadius,
                playerTransform.position.y + 0.1f,
                playerTransform.position.z + Mathf.Sin(angle) * spawnRadius
            );

            if (timeSinceStart >= 120f) SpawnGameObject(enemyPrefabs[0], spawnPos);
            else if (timeSinceStart >= 60f) SpawnGameObject(enemyPrefabs[0], spawnPos);
            else if (timeSinceStart >= 5f) SpawnGameObject(enemyPrefabs[0], spawnPos);
        }
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