using UnityEngine;

public class MagnetPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CoinManager.Instance.TriggerMagnet(other.transform);

            Destroy(gameObject);
        }
    }
}