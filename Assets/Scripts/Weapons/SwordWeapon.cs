using UnityEngine;

public class SwordWeapon : WeaponBase
{
    private Animator playerAnimator;

    public override void Initialize(Transform player)
    {
        base.Initialize(player);
        playerAnimator = player.GetComponent<Animator>();
    }

    protected override void Attack()
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetFloat("CleaveSpeed", attackSpeed * PlayerStats.Instance.AttackSpeedMultiplier);
            playerAnimator.SetTrigger("Attack");
        }
    }

    public void PerformCleave()
    {
        Collider[] hits = Physics.OverlapSphere(playerTransform.position, radius);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                if (Vector3.Dot(playerTransform.forward, (hit.transform.position - playerTransform.position).normalized) > 0.5f)
                {
                    hit.GetComponent<EnemyHealth>().TakeDamage(damage * PlayerStats.Instance.DamageMultiplier);
                }
            }
        }
    }

    public override string GetNextLevelDescription(float multiplier)
    {
        float nextDmg = damage + (2f * multiplier);
        float nextAttackSpeed = attackSpeed + (0.1f * multiplier);

        return $"Damage: {damage:F1} -> {nextDmg:F1}\nAttack Speed: {attackSpeed:F2} -> {nextAttackSpeed:F2}";
    }

    public override void Upgrade(float multiplier)
    {
        damage += 2f * multiplier;
        attackSpeed += 0.1f * multiplier;
    }
}