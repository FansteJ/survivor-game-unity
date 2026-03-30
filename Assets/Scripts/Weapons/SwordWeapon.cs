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
        playerAnimator.SetTrigger("Attack");
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

    public override string GetNextLevelDescription(int levelMultiplier = 1)
    {
        float nextDmg = damage + (2f * levelMultiplier);
        float nextAttackSpeed = attackSpeed + (0.1f * levelMultiplier);

        return $"Damage: {damage:F1} -> {nextDmg:F1}\nAttack Speed: {attackSpeed:F2} -> {nextAttackSpeed:F2}";
    }

    public override void Upgrade(int levelMultiplier)
    {
        currentLevel += levelMultiplier;
        damage += 2f * levelMultiplier;
        attackSpeed += 0.1f * levelMultiplier;
    }
}