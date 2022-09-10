using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GunScript : MonoBehaviour
{
  [Header("Bullet")]
  public GameObject bulletPrefab;
  public Transform muzzlePosition;

  [Header("Stats")]
  public int ammoCapacity;

  public GameObject[] muzzleFlashPrefabs;

  public UnityEvent onFire;

  public void Fire()
  {
    Instantiate(bulletPrefab, muzzlePosition.position, muzzlePosition.rotation);

    Instantiate(muzzleFlashPrefabs[0], muzzlePosition.position, muzzlePosition.rotation);

    onFire?.Invoke();
  }
}
