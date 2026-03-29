using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public float rotationSpeed = 90f;
    public int value = 1;

    public GameObject prefab;

    private bool isMagnetized = false;
    private Transform playerTransform;
    public float flySpeed = 20f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));

        if (isMagnetized && playerTransform != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, flySpeed * Time.deltaTime);
        }
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
        isMagnetized = false;
        playerTransform = null;

        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.RegisterCoin(this);
        }
    }

    private void OnDisable()
    {
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.UnregisterCoin(this);
        }
    }

    public void StartMagnetMode(Transform player)
    {
        isMagnetized = true;
        playerTransform = player;
    }
}
