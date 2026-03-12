using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public static WeaponController Instance { get; private set; }

    public List<WeaponBase> activeWeapons;


    private void Awake()
    {
        Instance = this;
    }



    void Start()
    {
        activeWeapons = new List<WeaponBase> ();
        RefreshActiveWeapons();
    }

    public void RefreshActiveWeapons()
    {
        activeWeapons.Clear ();

        activeWeapons.AddRange(GetComponentsInChildren<WeaponBase>(false));

        foreach(WeaponBase weapon in activeWeapons)
        {
            weapon.Initialize(transform);
        }
    }

    public void ApplyDamageUpgrade(float value)
    {
        foreach (WeaponBase weapon in activeWeapons)
            weapon.damage += value;
    }

    public void ApplyAttackSpeedUpgrade(float value)
    {
        foreach (WeaponBase weapon in activeWeapons)
            weapon.attackSpeed += value;
    }

    public void OnHitEvent()
    {
        foreach (WeaponBase weapon in activeWeapons)
        {
            if (weapon is SwordWeapon sword)
            {
                sword.PerformCleave();
            }
        }
    }
}
