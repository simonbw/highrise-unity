using System.Collections;
using UnityEngine;

[System.Serializable]
public struct CameraShakeInfo {
  public float amount;
  public int numberOfShakes;
  public float timePerShake;

  public CameraShakeInfo(float amount = 0.5f, int numberOfShakes = 1, float timePerShake = 0.1f) : this() {
    this.amount = amount;
    this.numberOfShakes = numberOfShakes;
    this.timePerShake = timePerShake;
  }
}

public delegate void CameraShakeEvent(CameraShakeInfo cameraShakeInfo);

public class CameraShake : MonoBehaviour {
  // Make an event that other classes can raise
  public static event CameraShakeEvent CameraShakeEvent;
  public static void Shake(float amount = 0.5f, int numberOfShakes = 1, float timePerShake = 0.1f) {
    Shake(new CameraShakeInfo(amount, numberOfShakes, timePerShake));
  }
  public static void Shake(CameraShakeInfo cameraShakeInfo) {
    CameraShakeEvent?.Invoke(cameraShakeInfo);
  }

  public Camera shakeCamera;
  // How much the camera shakes
  public float intensity = 10f;
  // How quickly it goes back to normal
  public float smoothSpeed = 0.2f;

  public Vector2 velocity;

  public void Awake() {
    if (shakeCamera == null) {
      shakeCamera = Camera.main;
    }
  }

  void OnEnable() {
    CameraShakeEvent += OnShake;
  }

  void OnDisable() {
    CameraShakeEvent -= OnShake;
  }

  public void FixedUpdate() {
    var movement = velocity * Time.fixedDeltaTime;
    shakeCamera.transform.Translate(movement.x, movement.y, 0f);
    velocity = Vector2.MoveTowards(velocity, new Vector2(), smoothSpeed * Time.fixedDeltaTime);
  }

  void OnShake(CameraShakeInfo shakeInfo) {
    StartCoroutine(DoShake(shakeInfo.amount, shakeInfo.numberOfShakes, shakeInfo.timePerShake));
  }

  private IEnumerator DoShake(float amount = 1f, int num = 3, float timePerShake = 0.05f) {
    for (int i = 0; i < num; i++) {
      velocity += RandomUtils.Direction() * amount * intensity;
      yield return new WaitForSeconds(timePerShake);
    }
    yield return null;
  }
}
