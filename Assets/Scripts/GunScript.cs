using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public enum FireMode {
  // One bullet per trigger pull
  SemiAuto,
  // Constant bullets as long as trigger is down
  FullAuto,
}

public enum ReloadStyle {
  // Rounds are loaded one-at-a-time
  Individual,
  // One reload action brings the gun back to full ammo
  Magazine
}

public enum EjectionType {
  // Shells eject on each shot
  Automatic,
  // Shells eject when pumped
  Pump,
  // Shells eject at start of reload
  Reload,
}

public class GunScript : MonoBehaviour {
  [Header("Prefabs")]
  public GameObject bulletPrefab;
  public GameObject casingPrefab;
  public GameObject[] muzzleFlashPrefabs;

  [Header("Connections")]
  public AudioSource audioSource;
  public Transform recoilTransform;

  [Header("Audio")]
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
  public float cooldownOnShoot = 0.1f; // in seconds
  public ReloadStyle reloadStyle = ReloadStyle.Magazine;
  public EjectionType ejectionType = EjectionType.Automatic;
  public FireMode fireMode = FireMode.SemiAuto;

  [Header("Events")]
  public UnityEvent onFire;

  // Private variables
  [SerializeField]
  int ammo;
  [SerializeField]
  int shellsToEject = 0;
  [SerializeField]
  bool isReloading = false;
  [SerializeField]
  float shootCooldown = 0f;
  [SerializeField]
  float pumpAmount = 0f;
  [SerializeField]
  bool roundChambered = true;

  public void Start() {
    ammo = ammoCapacity;
  }

  public Vector2 GetLeftHandPosition() {
    float pumpOffset = -0.3f * pumpAmount;
    return leftHandPosition.TransformPoint(new Vector2(pumpOffset, 0f));
  }

  public Vector2 GetRightHandPosition() {
    return rightHandPosition.position;
  }

  public void Update() {
    if (shootCooldown > 0) {
      shootCooldown -= Time.deltaTime;
    }

    float recoilOffset = -0.5f * Mathf.Pow(CurrentRecoil(), 1.5f);
    recoilTransform.localPosition = new Vector2(recoilOffset, 0f);

    float targetAngle = isReloading ? 50f : 0f;
    float angle = Mathf.MoveTowards(recoilTransform.localEulerAngles.z, targetAngle, 600f * Time.deltaTime);
    recoilTransform.localEulerAngles = new Vector3(0f, 0f, angle);
  }

  float CurrentRecoil() {
    return Mathf.Clamp01(shootCooldown / cooldownOnShoot);
  }

  public void Fire() {
    StartCoroutine(DoFire());
  }

  public IEnumerator DoFire() {
    if (isReloading) { /* Cancel? Do Nothing? */ } else if (shootCooldown > 0 || pumpAmount > 0) { /* Do Nothing? */ } else if (!roundChambered) { PlaySound(emptySounds); } else {
      // Actually shoot
      ammo -= 1;
      shellsToEject += 1;
      roundChambered = false;
      shootCooldown += cooldownOnShoot;

      Instantiate(bulletPrefab, muzzlePosition.position, muzzlePosition.rotation);
      Instantiate(muzzleFlashPrefabs[0], muzzlePosition.position, muzzlePosition.rotation);
      PlaySound(shootSounds);
      onFire?.Invoke();

      switch (ejectionType) {
        case EjectionType.Automatic:
          EjectShell();
          if (ammo > 0) {
            roundChambered = true;
          }
          break;
        case EjectionType.Reload:
          if (ammo > 0) {
            roundChambered = true;
          }
          break;
        case EjectionType.Pump:
          yield return new WaitForSeconds(0.175f);
          yield return DoPump();
          break;
      }
    }

    yield return null;
  }

  public void Pump() {
    StartCoroutine(DoPump());
  }

  public IEnumerator DoPump() {
    pumpAmount = 0f;

    PlaySound(pumpSounds);

    float duration = pumpDuration * 0.4f;

    float timeRemaining = duration;
    while (timeRemaining > 0) {
      timeRemaining -= Time.deltaTime;
      pumpAmount = Mathf.Clamp01(1f - timeRemaining / duration);
      yield return null;
    }

    pumpAmount = 1f;
    EjectShell();

    yield return new WaitForSeconds(pumpDuration * 0.2f);

    timeRemaining = duration;
    while (timeRemaining > 0) {
      timeRemaining -= Time.deltaTime;
      pumpAmount = Mathf.Clamp01(timeRemaining / duration);
      yield return null;
    }

    if (ammo > 0) {
      roundChambered = true;
    }
    pumpAmount = 0f;
  }

  public void Reload() {
    if (!isReloading) {
      StartCoroutine(DoReload());
    } else {
      // TODO: Cancel reloading?
    }
  }

  private IEnumerator DoReload() {
    isReloading = true;

    PlaySound(reloadStartSounds);
    yield return new WaitForSeconds(reloadStartDuration);

    // Ejection
    if (ejectionType == EjectionType.Reload) {
      while (this.shellsToEject > 0) {
        EjectShell();
        yield return new WaitForSeconds(0.02f);
      }
    }

    // Insert
    if (reloadStyle == ReloadStyle.Magazine) {
      yield return new WaitForSeconds(reloadInsertDuration);
      ammo = ammoCapacity;
    } else {
      while (ammo < ammoCapacity) {
        PlaySound(reloadInsertSounds);
        yield return new WaitForSeconds(reloadInsertDuration);
        ammo += 1;
      }
    }

    // Finish
    PlaySound(reloadFinishSounds);
    yield return new WaitForSeconds(reloadFinishDuration);

    isReloading = false;

    if (ejectionType == EjectionType.Reload) {
      roundChambered = true;
    }

    if (!roundChambered) {
      yield return DoPump();
    }
  }

  public void EjectShell() {
    if (shellsToEject > 0) {
      this.shellsToEject -= 1;
      var shell = Instantiate(casingPrefab, ejectionPosition.position, ejectionPosition.rotation);
      var shellBody = shell.GetComponent<Rigidbody2D>();

      shellBody.velocity = GetComponentInParent<Rigidbody2D>().velocity;

      if (ejectionType == EjectionType.Reload) {
        var force = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
        shellBody.AddRelativeForce(force, ForceMode2D.Impulse);
        float torque = Random.Range(-0.005f, 0.005f);
        shellBody.AddTorque(torque, ForceMode2D.Impulse);
        shell.GetComponent<CasingBounce>().zVelocity = 0f;
      } else {
        var force = new Vector2(Random.Range(-0.05f, 0.05f), Random.Range(-0.2f, -0.3f));
        float torque = Random.Range(-0.01f, 0.01f);

        shellBody.AddRelativeForce(force, ForceMode2D.Impulse);
        shellBody.AddTorque(torque, ForceMode2D.Impulse);
        shell.GetComponent<CasingBounce>().zVelocity = Random.Range(0f, 2f);
      }
    }
  }

  private void PlaySound(AudioClip[] clips) {
    var clip = clips.Choose();
    if (clip != null) {
      audioSource.PlayOneShot(clip);
    }
  }
}
