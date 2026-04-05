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
        Vector3 rayStartPos = playerTransform.position + Vector3.up * 1f;

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy") && hit.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
            {
                if (enemyHealth.IsDead)
                {
                    continue;
                }

                Vector3 targetCenter = hit.bounds.center;
                float distance = Vector3.Distance(rayStartPos, targetCenter);

                if (distance < minDistance)
                {
                    Vector3 directionToEnemy = (targetCenter - rayStartPos).normalized;

                    if (!Physics.Raycast(rayStartPos, directionToEnemy, distance, obstacleMask))
                    {
                        minDistance = distance;
                        targetCollider = hit;
                    }
                }
            }
        }

        if (targetCollider != null)
        {
            GameObject fireballObj = PoolManager.Instance.Get(projectilePrefab, rayStartPos);

            Vector3 targetCenter = targetCollider.bounds.center;

            fireballObj.transform.LookAt(targetCenter);

            Fireball fb = fireballObj.GetComponent<Fireball>();
            if (fb != null)
            {
                fb.damage = this.damage;
                fb.prefab = this.projectilePrefab;
                fb.SetTarget(targetCollider.transform);
            }
        }
    }

    public override string GetNextLevelDescription(float multiplier)
    {
        float nextDmg = damage + (5f * multiplier);
        float nextAttackSpeed = attackSpeed + (0.1f * multiplier);

        return $"Damage: {damage:F1} -> {nextDmg:F1}\nAttack Speed: {attackSpeed:F2} -> {nextAttackSpeed:F2}";
    }

    public override void Upgrade(float multiplier)
    {
        damage += 5f * multiplier;
        attackSpeed += 0.1f * multiplier;
    }
}