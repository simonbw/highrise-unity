using UnityEngine;
using UnityEngine.Events;

public class GunScript : MonoBehaviour
{
  [Header("Prefabs")]
  public GameObject bulletPrefab;
  public GameObject[] muzzleFlashPrefabs;
  public GameObject casingPrefab;

  [Header("Positions")]
  public Transform muzzlePosition;
  public Transform ejectionPosition;
  public Transform leftHandPosition;
  public Transform rightHandPosition;
  [Range(-180f, 180f)]
  public float stanceAngle = 0f;
  public Vector2 stanceOffset = new(0f, 0f);

  [Header("Stats")]
  [Min(0)]
  public int ammoCapacity;

  [Header("Events")]
  public UnityEvent onFire;

  // Private variables
  int ammo;
  int shellsToEject = 0;

  public void Start()
  {
    ammo = ammoCapacity;
  }

  public void Reload()
  {
    // TODO: Reload sound
    // TODO: Reload duration
    ammo = ammoCapacity;
  }

  public void Fire()
  {
    if (ammo > 0)
    {
      ammo -= 1;
      shellsToEject += 1;
      Instantiate(bulletPrefab, muzzlePosition.position, muzzlePosition.rotation);
      Instantiate(muzzleFlashPrefabs[0], muzzlePosition.position, muzzlePosition.rotation);

      EjectShell();
      onFire?.Invoke();
    }
  }

  public void EjectShell()
  {
    if (shellsToEject > 0)
    {
      this.shellsToEject -= 1;
      var shell = Instantiate(casingPrefab, ejectionPosition.position, ejectionPosition.rotation);
      var shellBody = shell.GetComponent<Rigidbody2D>();

      shellBody.velocity = GetComponentInParent<Rigidbody2D>().velocity;
      shellBody.AddRelativeForce(new Vector2(Random.Range(-0.05f, 0.05f), Random.Range(-0.3f, -0.45f)), ForceMode2D.Impulse);
      shellBody.AddTorque(Random.Range(-0.01f, 0.01f), ForceMode2D.Impulse);

      shell.GetComponent<CasingBounce>().zVelocity = Random.Range(1f, 5f);
    }
  }
}
