using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 20f;  // Speed of the projectile
    public int damage = 10;    // Damage that the projectile will deal
    public float lifetime = 5f; // Lifetime of the projectile before it gets destroyed

    private void Start()
    {
        // Automatically destroy the projectile after 'lifetime' seconds
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move the projectile forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the object hit is the player
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            // Deal damage to the player
            player.TakeDamage(damage);
            Destroy(gameObject); // Destroy the projectile after hitting the player
        }

        // Destroy the projectile if it hits anything
        Destroy(gameObject);
    }
}
