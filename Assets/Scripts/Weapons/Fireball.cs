using System.Collections;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 15f;
    public float lifeTime = 3f;

    [HideInInspector]
    public float damage;
    [HideInInspector]
    public GameObject prefab;

    private void OnEnable()
    {
        StartCoroutine(DeactivateAfterTime());
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth health = other.GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }

            StopAllCoroutines();
            PoolManager.Instance.Return(prefab, gameObject);
        }
    }

    private IEnumerator DeactivateAfterTime()
    {
        yield return new WaitForSeconds(lifeTime);

        PoolManager.Instance.Return(prefab, gameObject);
    }
}