using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum FireMode
{
  // One bullet per trigger pull
  SemiAuto,
  // Constant bullets as long as trigger is down
  FullAuto,
}

public enum ReloadStyle
{
  // Rounds are loaded one-at-a-time
  Individual,
  // One reload action brings the gun back to full ammo
  Magazine
}

public enum EjectionType
{
  // Shells eject on each shot
  Automatic,
  // Shells eject when pumped
  Pump,
  // Shells eject at start of reload
  Reload,
}

public class GunScript : MonoBehaviour
{
  [Header("Prefabs")]
  public GameObject bulletPrefab;
  public GameObject[] muzzleFlashPrefabs;
  public GameObject casingPrefab;

  [Header("Prefabs")]
  public AudioSource audioSource;

  public AudioClip[] shootSounds;
  public AudioClip[] emptySounds;
  public AudioClip[] reloadStartSounds;
  public AudioClip[] reloadInsertSounds;
  public AudioClip[] reloadFinishSounds;
  public AudioClip[] pumpSounds;
  public AudioClip[] pickupSounds;

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
  public float reloadStartDuration = 0.1f;
  public float reloadInsertDuration = 0.2f;
  public float reloadFinishDuration = 1f;
  public float pumpDuration = 0.15f;
  public ReloadStyle reloadStyle = ReloadStyle.Magazine;
  public EjectionType ejectionType = EjectionType.Automatic;
  public FireMode fireMode = FireMode.SemiAuto;

  [Header("Events")]
  public UnityEvent onFire;

  // Private variables
  int ammo;
  int shellsToEject = 0;
  bool isReloading = false;

  public void Start()
  {
    ammo = ammoCapacity;
  }

  public void Fire()
  {
    StartCoroutine(DoFire());
  }

  public IEnumerator DoFire()
  {
    if (isReloading)
    {
      // Cancel? Do Nothing?
    }
    else if (ammo > 0)
    {
      ammo -= 1;
      shellsToEject += 1;

      Instantiate(bulletPrefab, muzzlePosition.position, muzzlePosition.rotation);
      Instantiate(muzzleFlashPrefabs[0], muzzlePosition.position, muzzlePosition.rotation);
      PlaySound(shootSounds);

      onFire?.Invoke();

      if (ejectionType == EjectionType.Automatic)
      {
        EjectShell();
      }
      else if (ejectionType == EjectionType.Pump)
      {
        yield return new WaitForSeconds(0.175f);
        yield return DoPump();
      }

    }
    else
    {
      PlaySound(emptySounds);
    }

    yield return default;
  }

  public void Pump()
  {
    StartCoroutine(DoPump());
  }

  public IEnumerator DoPump()
  {
    PlaySound(pumpSounds);
    yield return new WaitForSeconds(pumpDuration);
    yield return default;
  }

  public void Reload()
  {
    if (!isReloading)
    {
      StartCoroutine(DoReload());
    }
    else
    {
      // TODO: Cancel reloading?
    }
  }

  private IEnumerator DoReload()
  {
    isReloading = true;

    PlaySound(reloadStartSounds);
    yield return new WaitForSeconds(reloadStartDuration);

    // Ejection
    if (ejectionType == EjectionType.Reload)
    {
      while (this.shellsToEject > 0)
      {
        EjectShell();
        yield return new WaitForSeconds(0.02f);
      }
    }

    // Insert
    if (reloadStyle == ReloadStyle.Magazine)
    {
      yield return new WaitForSeconds(reloadInsertDuration);
      ammo = ammoCapacity;
    }
    else
    {
      while (ammo < ammoCapacity)
      {
        PlaySound(reloadInsertSounds);
        yield return new WaitForSeconds(reloadInsertDuration);
        ammo += 1;
      }
    }

    // Finish
    PlaySound(reloadFinishSounds);
    yield return new WaitForSeconds(reloadInsertDuration);

    isReloading = false;

    yield return default;
  }

  public void EjectShell()
  {
    if (shellsToEject > 0)
    {
      this.shellsToEject -= 1;
      var shell = Instantiate(casingPrefab, ejectionPosition.position, ejectionPosition.rotation);
      var shellBody = shell.GetComponent<Rigidbody2D>();

      shellBody.velocity = GetComponentInParent<Rigidbody2D>().velocity;

      if (ejectionType == EjectionType.Reload)
      {
        var force = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
        shellBody.AddRelativeForce(force, ForceMode2D.Impulse);
        float torque = Random.Range(-0.005f, 0.005f);
        shellBody.AddTorque(torque, ForceMode2D.Impulse);
        shell.GetComponent<CasingBounce>().zVelocity = 0f;
      }
      else
      {
        var force = new Vector2(Random.Range(-0.05f, 0.05f), Random.Range(-0.3f, -0.45f));
        float torque = Random.Range(-0.01f, 0.01f);

        shellBody.AddRelativeForce(force, ForceMode2D.Impulse);
        shellBody.AddTorque(torque, ForceMode2D.Impulse);
        shell.GetComponent<CasingBounce>().zVelocity = Random.Range(1f, 5f);
      }

    }
  }

  private void PlaySound(AudioClip[] clips)
  {
    var clip = clips.Choose();
    if (clip != null)
    {
      audioSource.PlayOneShot(clip);
    }
  }
}
