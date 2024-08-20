using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;  // Speed of the projectile
    private int damage;        // Damage that the projectile will deal
    private string weaponID;   // Weapon ID

    public void SetDamage(int damageAmount, string weaponID)
    {
        damage = damageAmount;  // Set the damage amount
        this.weaponID = weaponID;  // Set the weapon ID
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the object hit has the Enemy script
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            // Deal damage to the enemy
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }

        // Check if the object hit has the Door script
        Door door = collision.gameObject.GetComponent<Door>();

        if (door != null)
        {
            // Deal damage to the door
            door.TakeDamage(damage, weaponID);
        }

        // Destroy the projectile after it hits something
        Destroy(gameObject);
    }
}

        