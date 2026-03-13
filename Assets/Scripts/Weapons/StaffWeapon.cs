using UnityEngine;

public class StaffWeapon : WeaponBase
{
    public GameObject projectilePrefab;

    public LayerMask obstacleMask;

    protected override void Attack()
    {
        Collider[] hits = Physics.OverlapSphere(playerTransform.position, radius);

        Collider targetCollider = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(playerTransform.position, hit.transform.position);

                if (distance < minDistance)
                {
                    Vector3 directionToEnemy = (hit.transform.position - playerTransform.position).normalized;

                    if (!Physics.Raycast(playerTransform.position, directionToEnemy, distance, obstacleMask))
                    {
                        minDistance = distance;
                        targetCollider = hit;
                    }
                }
            }
        }

        if (targetCollider != null)
        {
            Vector3 spawnPosition = playerTransform.position + Vector3.up * 1f;
            GameObject fireballObj = PoolManager.Instance.Get(projectilePrefab, spawnPosition);

            Vector3 targetCenter = targetCollider.bounds.center;

            fireballObj.transform.LookAt(targetCenter);

            Fireball fb = fireballObj.GetComponent<Fireball>();
            if (fb != null)
            {
                fb.damage = this.damage;
                fb.prefab = this.projectilePrefab;
                fb.target = targetCollider.transform;
            }
        }
    }
}