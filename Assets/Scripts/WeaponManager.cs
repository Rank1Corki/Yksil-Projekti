using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Transform weaponHoldPoint;  // Reference to the weapon hold point on the player
    public Transform shootPoint;  // Reference to the shoot point
    public Transform playerTransform;  // Reference to the player

    private Gun currentGun;  // The currently equipped gun
    public GameObject baseWeaponPrefab;  // Reference to the base weapon prefab

    void Start()
    {
        // Equip the first weapon variant by default if available
        if (Inventory.Instance.acquiredWeapons.Count > 0)
        {
            EquipWeapon(Inventory.Instance.acquiredWeapons[0]);  // Pass WeaponVariant directly
        }
    }

    void Update()
    {
        // Switch weapons with number keys, only if the weapon is in the inventory
        for (int i = 0; i < Inventory.Instance.acquiredWeapons.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                EquipWeapon(Inventory.Instance.acquiredWeapons[i]);  // Pass WeaponVariant directly
            }
        }

        // Shoot the current gun
        if (currentGun != null && Input.GetButton("Fire1"))
        {
            currentGun.Shoot(shootPoint, playerTransform);
        }
    }

    public void EquipWeapon(WeaponVariant weaponVariant)
    {
        // Destroy the current gun if there is one
        if (currentGun != null)
        {
            Destroy(currentGun.gameObject);
        }

        // Instantiate the base weapon prefab
        GameObject newWeapon = Instantiate(baseWeaponPrefab, weaponHoldPoint.position, weaponHoldPoint.rotation);
        newWeapon.transform.SetParent(weaponHoldPoint);

        // Get the Gun component from the instantiated weapon
        currentGun = newWeapon.GetComponent<Gun>();

        // Apply the weapon variant properties to the new weapon
        currentGun.ApplyVariant(weaponVariant);
    }
}
