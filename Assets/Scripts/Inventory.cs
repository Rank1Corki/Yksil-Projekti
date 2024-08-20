using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // For UI elements
using TMPro;  // For TextMeshPro

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;  // Singleton instance

    public GameObject weaponSlotPrefab;  // Prefab for displaying weapons in the UI
    public Transform inventoryContent;  // The Content object in the Scroll View

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
            UpdateInventoryUI();  // Update the UI to reflect the new weapon
        }
    }

    void UpdateInventoryUI()
    {
        // Clear existing items
        foreach (Transform child in inventoryContent)
        {
            Destroy(child.gameObject);
        }

        // Instantiate and populate UI elements for each weapon
        foreach (WeaponVariant weapon in acquiredWeapons)
        {
            GameObject weaponSlot = Instantiate(weaponSlotPrefab, inventoryContent);

            // Find and set up the icon and text
            Image icon = weaponSlot.GetComponentInChildren<Image>();
            TMP_Text nameText = weaponSlot.GetComponentInChildren<TMP_Text>();

            if (icon != null)
            {
                // Assume WeaponVariant has a weaponIcon property
                icon.sprite = weapon.weaponIcon;
            }

            if (nameText != null)
            {
                // Set the weapon name text
                nameText.text = weapon.weaponName;
            }
        }
    }
}
