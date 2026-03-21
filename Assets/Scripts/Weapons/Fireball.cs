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

    private Transform target;
    private EnemyHealth targetHealth;
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        if (target != null)
        {
            targetHealth = target.GetComponent<EnemyHealth>();
        }
    }

    private void OnEnable()
    {
        StartCoroutine(DeactivateAfterTime());
    }

    private void Update()
    {
        if (target != null && target.gameObject.activeInHierarchy && targetHealth != null && !targetHealth.IsDead)
        {
            Vector3 targetPosition = target.position + Vector3.up * 0.5f;
            transform.LookAt(targetPosition);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        } else
        {
            StopAllCoroutines();
            PoolManager.Instance.Return(prefab, gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth health = other.GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(damage * PlayerStats.Instance.DamageMultiplier);
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