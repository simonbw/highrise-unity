using System.Collections;
using Cinemachine;
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
  //////////////////
  // Static Stuff //
  //////////////////

  // Make an event that other classes can raise
  public static event CameraShakeEvent CameraShakeEvent;

  public static void Shake(float amount = 0.5f, int numberOfShakes = 1, float timePerShake = 0.1f) {
    Shake(new CameraShakeInfo(amount, numberOfShakes, timePerShake));
  }

  public static void Shake(CameraShakeInfo cameraShakeInfo) {
    CameraShakeEvent?.Invoke(cameraShakeInfo);
  }

  ////////////////////
  // Instance Stuff //
  ////////////////////

  public CinemachineVirtualCamera virtualCamera;
  // How much the camera shakes
  public float Intensity = 10f;
  // How quickly it goes back to normal
  public float ResetSpeed = 0.2f;

  [SerializeField]
  public float CurrentShakeAmount { get; private set; }

  void Awake() {
    virtualCamera ??= GetComponent<CinemachineVirtualCamera>();
  }

  void OnEnable() {
    CameraShakeEvent += OnShake;
  }

  void OnDisable() {
    CameraShakeEvent -= OnShake;
  }

  public void Update() {
    var noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    noise.m_AmplitudeGain = Mathf.Sqrt(CurrentShakeAmount) * Intensity;

    CurrentShakeAmount = Mathf.MoveTowards(CurrentShakeAmount, 0f, ResetSpeed * Time.deltaTime);
  }

  void OnShake(CameraShakeInfo shakeInfo) {
    CurrentShakeAmount += shakeInfo.amount;
  }
}
