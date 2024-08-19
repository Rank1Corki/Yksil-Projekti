using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Weapon/Gun")]
public class GunData : ScriptableObject
{
    public string gunName;
    public GameObject projectilePrefab;
    public float fireRate = 0.1f;
    public float projectileSpeed = 10f;
    public int damage = 10;
}
