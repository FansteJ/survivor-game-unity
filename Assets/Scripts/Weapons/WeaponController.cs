using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public List<Weapon> weapons;
    public Transform playerTransform;
    public List<float> currentTimes;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        weapons = new List<Weapon> ();
        currentTimes = new List<float> ();
        weapons.Add(new Weapon {attackSpeed = 1, damage = 10, description = "Beginner's weapon", name = "Wooden sword", radius = 1f});
        currentTimes.Add(0);
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        foreach (Weapon weapon in weapons)
        {
            currentTimes[i] += Time.deltaTime;
            if (currentTimes[i] >= 1f / weapon.attackSpeed)
            {
                currentTimes[i] -= 1f/weapon.attackSpeed;
                Attack(weapon);
            }
            i++;
        }
    }

    void Attack(Weapon weapon)
    {
        Collider[] hits = Physics.OverlapSphere(playerTransform.position, weapon.radius);
        Collider closest = null;
        float closestDistance = float.MaxValue;

        foreach(Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(playerTransform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closest = hit;
                }
            }
        }

        if(closest != null)
        {
            closest.GetComponent<EnemyHealth>().TakeDamage(weapon.damage);
        }
    }
}
