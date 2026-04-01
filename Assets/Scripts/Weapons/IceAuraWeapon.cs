using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class IceAura : WeaponBase
{
    [Header("Ice Aura Specifics")]
    [Range(0f, 0.99f)]
    public float slowPercent = 0.20f;

    private List<Collider> enemiesInAura = new List<Collider>();
    
    public MeshRenderer auraRenderer;

    public override void Initialize(Transform player)
    {
        base.Initialize(player);

        weaponName = "Ice Aura";
    }

    protected override void Update()
    {
        base.Update();

        Color color = auraRenderer.material.color;
        color.a = Mathf.PingPong(Time.time * 0.5f, 0.4f) + 0.1f;
        auraRenderer.material.color = color;
    }

    protected override void Attack()
    {
        DealDamageToEnemies();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!enemiesInAura.Contains(other))
            {
                enemiesInAura.Add(other);
                ApplySlow(other, true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (enemiesInAura.Contains(other))
            {
                enemiesInAura.Remove(other);
                ApplySlow(other, false);
            }
        }
    }

    private void ApplySlow(Collider enemyCollider, bool apply)
    {
        NavMeshAgent agent = enemyCollider.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            if (apply)
            {
                agent.speed = agent.speed * (1f - slowPercent);
            }
            else
            {
                agent.speed = agent.speed / (1f - slowPercent);
            }
        }
    }

    private void DealDamageToEnemies()
    {
        for (int i = enemiesInAura.Count - 1; i >= 0; i--)
        {
            Collider col = enemiesInAura[i];

            if (col == null || !col.gameObject.activeInHierarchy)
            {
                enemiesInAura.RemoveAt(i);
                continue;
            }

            EnemyHealth health = col.GetComponent<EnemyHealth>();
            if (health != null && !health.IsDead)
            {
                health.TakeDamage(damage * PlayerStats.Instance.DamageMultiplier);
            }
        }
    }

    public override string GetNextLevelDescription(float multiplier = 1)
    {
        float nextDmg = damage + (1f * multiplier);

        float slowUpgradeFactor = 0.05f * multiplier;
        float nextSlowFloat = slowPercent + (1f - slowPercent) * slowUpgradeFactor;

        return $"Damage: {damage:F1} -> {nextDmg:F1}\nSlow: {(slowPercent * 100f):F0}% -> {(nextSlowFloat * 100f):F0}%";
    }

    public override void Upgrade(float multiplier)
    {
        damage += 1f * multiplier;

        float slowUpgradeFactor = 0.05f * multiplier;
        slowPercent += (1f - slowPercent) * slowUpgradeFactor;
    }
}