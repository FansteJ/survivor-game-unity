using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public List<Weapon> weapons;
    public Transform playerTransform;
    public List<float> currentTimes;

    private Animator animator;
    private Weapon currentAttackingWeapon;

    public static WeaponController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void ApplyDamageUpgrade(float value)
    {
        foreach (Weapon weapon in weapons)
            weapon.damage += value;
    }

    public void ApplyAttackSpeedUpgrade(float value)
    {
        foreach (Weapon weapon in weapons)
            weapon.attackSpeed += value;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        weapons = new List<Weapon> ();
        currentTimes = new List<float> ();
        weapons.Add(new Weapon {attackSpeed = 1, damage = 10, description = "Beginner's weapon", name = "Wooden sword", radius = 4f});
        currentTimes.Add(0);

        animator = GetComponent<Animator> ();
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        foreach (Weapon weapon in weapons)
        {
            currentTimes[i] += Time.deltaTime;
            if (currentTimes[i] >= 1f / (weapon.attackSpeed * PlayerStats.Instance.AttackSpeedMultiplier))
            {
                currentTimes[i] = 0;
                StartAttackAnimation(weapons[i]);
            }
            i++;
        }
    }

    void StartAttackAnimation(Weapon weapon)
    {
        currentAttackingWeapon = weapon;
        animator.SetTrigger("Attack");
    }

    public void OnHitEvent()
    {
        if (currentAttackingWeapon == null) return;

        Collider[] hits = Physics.OverlapSphere(playerTransform.position, currentAttackingWeapon.radius);

        foreach(Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                if(Vector3.Dot(playerTransform.forward, (hit.transform.position - playerTransform.position).normalized) > 0.5f)
                {
                    hit.GetComponent<EnemyHealth>().TakeDamage(currentAttackingWeapon.damage * PlayerStats.Instance.DamageMultiplier);
                }
            }
        }
    }

}
