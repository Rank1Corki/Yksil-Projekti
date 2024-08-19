using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public WeaponVariant weaponVariant;  // The weapon variant this pickup represents

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Add the weapon to the player's inventory
            Inventory.Instance.AddWeapon(weaponVariant);

            // Optionally, equip the weapon immediately
            WeaponManager weaponManager = other.GetComponent<WeaponManager>();
            if (weaponManager != null)
            {
                // Equip the weapon by passing the WeaponVariant directly
                weaponManager.EquipWeapon(weaponVariant);
            }

            // Destroy the pickup object
            Destroy(gameObject);
        }
    }
}
