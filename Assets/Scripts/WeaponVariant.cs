using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Variant", menuName = "Weapon/Variant")]
public class WeaponVariant : ScriptableObject
{
    public string weaponName;
    public Color weaponColor;
    public Vector3 weaponScale;
    public float fireRate;
    public int damage;
    public GameObject projectilePrefab;
}
