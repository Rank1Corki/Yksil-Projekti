using UnityEngine;
using TMPro;  // For TextMeshPro if used

[CreateAssetMenu(fileName = "New Weapon Variant", menuName = "Weapon/Variant")]
public class WeaponVariant : ScriptableObject
{
    public string weaponName;
    public string weaponID; // Unique identifier for this weapo
    public Color weaponColor;
    public Vector3 weaponScale;
    public float fireRate;
    public int damage;
    public GameObject projectilePrefab;
    public Sprite weaponIcon;  // Add a Sprite field for the weapon icon
}

