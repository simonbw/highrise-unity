using System;
using UnityEngine;

public class PlayerController : MonoBehaviour {

  public GunScript currentWeapon = null;

  void OnEnable() {
    foreach (var gun in GetComponentsInChildren<GunScript>()) {
      gun.onFire.AddListener(OnFire);
    }
  }

  void OnDisable() {
    foreach (var gun in GetComponentsInChildren<GunScript>()) {
      gun.onFire.RemoveListener(OnFire);
    }
  }

  void OnFire(GunScript gun) {
    CameraShake.Shake(gun.cameraShakeInfo);
  }

  void Start() {
    SelectWeapon(0);
  }

  void Update() {
    // TODO: Is this the best way to be checking input?
    if (Input.GetButtonDown("Fire1")) {
      currentWeapon?.Fire();
    } else if (currentWeapon?.fireMode == FireMode.FullAuto && Input.GetButton("Fire1")) {
      currentWeapon.Fire();
    }

    if (Input.GetButtonDown("Reload")) {
      currentWeapon?.Reload();
    }

    if (Input.GetKeyDown(KeyCode.Alpha1)) {
      SelectWeapon(0);
    }
    if (Input.GetKeyDown(KeyCode.Alpha2)) {
      SelectWeapon(1);
    }
    if (Input.GetKeyDown(KeyCode.Alpha3)) {
      SelectWeapon(2);
    }
    if (Input.GetKeyDown(KeyCode.Tab)) {
      SelectNextWeapon();
    }
  }

  void FixedUpdate() {
    Rigidbody2D body = GetComponent<Rigidbody2D>();

    Vector2 moveDirection = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    float angle = Vector2.SignedAngle(Vector2.right, moveDirection);
    float speedPercent = Mathf.Clamp01(moveDirection.magnitude);
    GetComponent<WalkController>().WalkTowards(angle, speedPercent);

    // TODO: Support controller aiming
    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    float targetAngle = Vector2.SignedAngle(Vector2.right, mousePosition - body.position);
    float headAngle = Mathf.Clamp(Mathf.DeltaAngle(targetAngle, body.rotation) % 360, -90f, 90f);

    GetComponent<AimController>().targetAngle = targetAngle;
    BodyPoser bodyPoser = GetComponent<BodyPoser>();
    bodyPoser.headAngle = headAngle;

    if (currentWeapon) {
      bodyPoser.leftHandPosition = transform.InverseTransformPoint(currentWeapon.GetLeftHandPosition());
      bodyPoser.rightHandPosition = transform.InverseTransformPoint(currentWeapon.GetRightHandPosition());
      bodyPoser.stanceAngle = currentWeapon.stanceAngle;
      bodyPoser.stanceOffset = currentWeapon.stanceOffset;
    } else {
      // TODO: Empty hand better 
      bodyPoser.leftHandPosition = new Vector2(0f, 0f);
      bodyPoser.rightHandPosition = new Vector2(0f, 0f);
      bodyPoser.stanceAngle = 0f;
      bodyPoser.stanceOffset = new Vector2(0f, 0f);
    }
  }

  // TODO: Handle Shoving

  public void SelectWeapon(int weaponIndex = 0) {
    var weapons = GetComponentsInChildren<GunScript>(includeInactive: true);
    for (int i = 0; i < weapons.Length; i++) {
      var weapon = weapons[i].gameObject;
      weapon.SetActive(i == weaponIndex);
    }
    currentWeapon = weapons[weaponIndex];
  }

  public void SelectNextWeapon() {
    if (currentWeapon == null) {
      SelectWeapon(0);
    } else {
      var weapons = GetComponentsInChildren<GunScript>(includeInactive: true);
      var currentIndex = Array.IndexOf(weapons, currentWeapon);
      if (currentIndex < 0) {
        SelectWeapon(0);
      } else {
        SelectWeapon(currentIndex % weapons.Length);
      }
    }
  }

}


// export function stepToward(from: number, to: number, stepSize: number): number {
//   if (to > from) {
//     return Math.min(from + stepSize, to);
//   } else {
//     return Math.max(from - stepSize, to);
//   }
// }