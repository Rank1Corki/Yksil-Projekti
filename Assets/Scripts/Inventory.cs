using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;  // Singleton instance

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<WeaponVariant> acquiredWeapons = new List<WeaponVariant>();  // List of acquired weapons

    public bool HasWeapon(WeaponVariant weapon)
    {
        return acquiredWeapons.Contains(weapon);  // Check if a weapon is in the inventory
    }

    public void AddWeapon(WeaponVariant weapon)
    {
        if (!acquiredWeapons.Contains(weapon))
        {
            acquiredWeapons.Add(weapon);  // Add weapon to the inventory
        }
    }
}
