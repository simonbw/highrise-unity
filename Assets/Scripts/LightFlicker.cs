using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class LightFlicker : MonoBehaviour {
  public new Light2D light;

  public int hitsLeft = 3;

  public float onMin = 0.05f;
  public float onMax = 0.15f;
  public float offMin = 0.05f;
  public float offMax = 0.15f;

  public UnityEvent<bool> OnFlicker;

  public Coroutine flickerRoutine = null;

  public void Flicker() {
    if (hitsLeft > 0) {
      hitsLeft -= 1;
      flickerRoutine = StartCoroutine(DoFlicker());
    } else {
      light.enabled = false;
    }
  }

  public IEnumerator DoFlicker() {
    if (flickerRoutine != null) {
      StopCoroutine(flickerRoutine);
    }
    int n = Random.Range(2, 5);
    for (int i = 0; i < n; i++) {
      yield return new WaitForSeconds(Random.Range(onMin, onMax));
      OnFlicker.Invoke(false);
      light.enabled = false;
      yield return new WaitForSeconds(Random.Range(onMin, onMax));
      OnFlicker.Invoke(true);
      light.enabled = true;
    }

    flickerRoutine = null;
  }
}
