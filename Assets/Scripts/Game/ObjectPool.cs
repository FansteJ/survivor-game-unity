using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private GameObject prefab;
    private Queue<GameObject> pool;

    public ObjectPool(GameObject prefab, int initialSize)
    {
        pool = new Queue<GameObject>();
        this.prefab = prefab;
        for(int i = 0; i < initialSize; i++)
        {
            GameObject gameObject = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            gameObject.SetActive(false);
            pool.Enqueue(gameObject);
        }
    }

    public GameObject Get(Vector3 position)
    {
        GameObject gameObject;
        if(pool.Count > 0)
        {
            gameObject = pool.Dequeue();
            gameObject.transform.position = position;
            gameObject.SetActive(true);
        } else
        {
            gameObject = Object.Instantiate(prefab, position, Quaternion.identity);
        }

        return gameObject;
    }

    public void Return(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}