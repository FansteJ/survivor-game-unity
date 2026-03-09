using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    private Dictionary<GameObject, ObjectPool> pools;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            pools = new Dictionary<GameObject, ObjectPool>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject Get(GameObject prefab, Vector3 position, int initialSize = 10)
    {
        if (!pools.ContainsKey(prefab))
            pools.Add(prefab, new ObjectPool(prefab, initialSize));
        return pools[prefab].Get(position);
    }

    public void Return(GameObject prefab, GameObject obj)
    {
        if (pools.ContainsKey(prefab))
            pools[prefab].Return(obj);
        else
            Object.Destroy(obj);
    }
}