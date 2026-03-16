using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 18f;
    public float damage = 25f;
    public float lifetime = 4f;

    private float currentLifetime;
    public GameObject prefab;

    private void OnEnable()
    {
        currentLifetime = 0f;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        currentLifetime += Time.deltaTime;
        if (currentLifetime >= lifetime)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            PoolManager.Instance.Return(prefab, gameObject);
        }
        else if (other.CompareTag("Obstacle"))
        {
            PoolManager.Instance.Return(prefab, gameObject);
        }
    }
}