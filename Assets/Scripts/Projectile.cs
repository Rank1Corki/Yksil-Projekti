using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;  // Speed of the projectile
    private int damage;        // Damage that the projectile will deal

    public void SetDamage(int damageAmount)
    {
        damage = damageAmount;  // Set the damage amount
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

        // Destroy the projectile after it hits something
        Destroy(gameObject);
    }
}
