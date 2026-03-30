using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public static WeaponController Instance { get; private set; }

    public List<WeaponBase> activeWeapons;
    public List<WeaponBase> allWeapons;


    private void Awake()
    {
        Instance = this;
    }



    void Start()
    {
        activeWeapons = new List<WeaponBase> ();
        allWeapons = new List<WeaponBase> ();
        allWeapons.AddRange(GetComponentsInChildren<WeaponBase>(true));

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
