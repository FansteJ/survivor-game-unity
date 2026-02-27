using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public static CoinSpawner Instance { get; private set; }

    public GameObject coinPrefab1;
    public GameObject coinPrefab5;
    public GameObject coinPrefab25;
    public GameObject coinPrefab100;

    public float spawnRadius = 0.2f;

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

    public void SpawnCoins(int value, Vector3 position)
    {
        while(value > 0)
        {
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            Vector3 spawnPos = new Vector3(position.x + Mathf.Cos(angle) * spawnRadius,
                position.y + 0.1f, position.z + Mathf.Sin(angle) * spawnRadius);
            if (value >= 100)
            {
                SpawnGameObject(coinPrefab100, spawnPos);
                value -= 100;
            } else if (value >= 25)
            {
                SpawnGameObject(coinPrefab25, spawnPos);
                value -= 25;
            } else if (value >= 5)
            {
                SpawnGameObject(coinPrefab5, spawnPos);
                value -= 5;
            } else
            {
                SpawnGameObject(coinPrefab1, spawnPos);
                value--;
            }
        }
    }

    private void SpawnGameObject(GameObject coin, Vector3 position)
    {
        Instantiate(coin, position, Quaternion.identity);
    }
}
