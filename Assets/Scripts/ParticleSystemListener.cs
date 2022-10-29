using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ParticleSystemListener : MonoBehaviour {
  public UnityEvent OnCallback;
  public bool destroyOnCallback = false;
  public bool disableOnCallback = false;

  public void OnParticleSystemStopped() {
    OnCallback.Invoke();
    if (destroyOnCallback) {
      Destroy(gameObject);
    }
    if (disableOnCallback) {
      gameObject.SetActive(false);
    }
  }
}
