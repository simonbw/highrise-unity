using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GunData", order = 1)]
public class GunData : ScriptableObject
{
  [Header("Sprites")]
  public Sprite pickupSprite;
  public Sprite holdingSprite;

  [Header("Bullet")]
  public GameObject bulletPrefab;
  public Transform muzzlePosition;

  [Header("Stats")]
  public int ammoCapacity;
}
