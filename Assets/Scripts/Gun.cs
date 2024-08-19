using UnityEngine;

public class Gun : MonoBehaviour
{
    public GunData gunData;  // Reference to the ScriptableObject containing base gun data

    private float nextFireTime = 0f;  // Tracks when the gun can fire next

    // Method to update the gun's properties based on a WeaponVariant
    public void ApplyVariant(WeaponVariant variant)
    {
        // Update gun properties based on the variant
        gunData.fireRate = variant.fireRate;
        gunData.damage = variant.damage;
        gunData.projectilePrefab = variant.projectilePrefab;

        // Update the gun's appearance
        transform.localScale = variant.weaponScale;
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = variant.weaponColor;
        }
    }

    // Method to handle shooting
    public void Shoot(Transform shootPoint, Transform playerTransform)
    {
        // Check if the gun can fire
        if (Time.time >= nextFireTime)
        {
            // Calculate the next fire time
            nextFireTime = Time.time + gunData.fireRate;

            // Instantiate and fire the projectile
            GameObject projectile = Instantiate(gunData.projectilePrefab, shootPoint.position, shootPoint.rotation);

            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = shootPoint.forward * gunData.projectileSpeed;
            }

            // Set the damage for the projectile if it has a Projectile script attached
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.SetDamage(gunData.damage);
            }
        }
    }
}
