using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public float rotationSpeed = 90f;
    public int value = 1;

    public GameObject prefab;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PoolManager.Instance.Return(prefab, gameObject);
            CoinManager.Instance.AddCoin(value);
        }
    }

    private void OnEnable()
    {
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
    }
}
