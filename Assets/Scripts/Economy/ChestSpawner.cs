using UnityEngine;

public class ChestSpawner : MonoBehaviour
{
    public static ChestSpawner Instance {  get; private set; }

    public GameObject chestPrefab;
    public float spawnInterval = 30f;
    public int maxChests = 5;
    public int existingChests = 0;

    public float currentTime = 0f;

    private int chestsOpened = 0;
    private float nextChestCost = 10f;

    public float GetNextChestCost() => nextChestCost;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if( existingChests < maxChests && currentTime >= spawnInterval )
        {
            currentTime -= spawnInterval;
            SpawnChest();
        }
    }

    private void SpawnChest()
    {
        existingChests++;
        Instantiate(chestPrefab, new Vector3(Random.Range(-20f, 20f), 10, 
            Random.Range(-20f, 20f)), Quaternion.identity);
    }

    public void ChestOpened()
    {
        existingChests--;
        chestsOpened++;
        nextChestCost *= 1.2f;
    }
}
