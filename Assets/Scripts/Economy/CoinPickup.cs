using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public float rotationSpeed = 90f;
    public int value = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            CoinManager.Instance.AddCoin(value);
        }
    }
}
