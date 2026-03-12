using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public string weaponName;
    public float damage = 10f;
    public float attackSpeed = 1f;
    public float radius = 4f;

    protected float currentCooldown = 0f;
    protected Transform playerTransform;

    public virtual void Initialize(Transform player)
    {
        playerTransform = player;
    }

    protected virtual void Update()
    {
        currentCooldown += Time.deltaTime;  

        if(currentCooldown >= 1f/(attackSpeed * PlayerStats.Instance.AttackSpeedMultiplier))
        {
            currentCooldown = 0f;
            Attack();
        }
    }

    protected abstract void Attack();
}
