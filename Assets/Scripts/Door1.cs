using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    public float health = 100f; // Health of the door
    public float shakeIntensity = 0.1f; // Intensity of the shake effect
    public float shakeDuration = 0.1f; // Duration of the shake effect
    public Color hitColor = Color.red; // Color when hit
    public Color originalColor = Color.white; // Original color of the door
    public string[] allowedWeaponIDs; // Array of allowed weapon IDs

    private Renderer doorRenderer;
    private Vector3 originalPosition;
    private bool isShaking = false;

    void Start()
    {
        doorRenderer = GetComponent<Renderer>();
        originalPosition = transform.position;

        if (doorRenderer != null)
        {
            originalColor = doorRenderer.material.color;
        }
    }

    public void TakeDamage(float damage, string weaponID)
    {
        if (IsWeaponAllowed(weaponID))
        {
            // Reduce health
            health -= damage;

            // Trigger shake effect and color change
            StartCoroutine(ShakeAndColorChange());

            // Check if the door is destroyed
            if (health <= 0f)
            {
                DestroyDoor();
            }
        }
    }

    private bool IsWeaponAllowed(string weaponID)
    {
        foreach (string id in allowedWeaponIDs)
        {
            if (id == weaponID)
            {
                return true;
            }
        }
        return false;
    }

    private IEnumerator ShakeAndColorChange()
    {
        if (isShaking)
            yield break;

        isShaking = true;

        // Change color to hit color
        if (doorRenderer != null)
        {
            doorRenderer.material.color = hitColor;
        }

        // Shake the door
        Vector3 originalPos = transform.position;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float xShake = Random.Range(-shakeIntensity, shakeIntensity);
            float zShake = Random.Range(-shakeIntensity, shakeIntensity);
            transform.position = new Vector3(originalPos.x + xShake, originalPos.y, originalPos.z + zShake);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Reset position and color
        transform.position = originalPos;
        if (doorRenderer != null)
        {
            doorRenderer.material.color = originalColor;
        }

        isShaking = false;
    }

    private void DestroyDoor()
    {
        // Play destruction effects, if any
        // Example: Instantiate a destruction effect prefab, play sound, etc.

        Destroy(gameObject);
    }
}
