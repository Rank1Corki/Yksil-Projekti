using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;  // The enemy's maximum health
    private int currentHealth;   // The enemy's current health

    void Start()
    {
        // Initialize the enemy's health
        currentHealth = maxHealth;
    }

    // Method to handle taking damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;  // Reduce health by the damage amount
        Debug.Log($"enemy health: {currentHealth}");
        // Check if health falls below or equals 0
        if (currentHealth <= 0)
        {
            Die();  // Call Die function if health is zero or less
        }
    }

    // Method to destroy the enemy when health reaches 0
    void Die()
    {
        Destroy(gameObject);  // Destroy the enemy game object
    }
}
