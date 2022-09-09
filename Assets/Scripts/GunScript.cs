using UnityEngine;

public class GunScript : MonoBehaviour
{
  [Header("Bullet")]
  public GameObject bulletPrefab;
  public Transform muzzlePosition;

  [Header("Stats")]
  public int ammoCapacity;


  public void Fire()
  {
    Instantiate(bulletPrefab, transform.position, transform.rotation);
  }
}
