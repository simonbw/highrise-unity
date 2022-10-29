using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlicker : MonoBehaviour {
  public new Light2D light;

  public int hitsLeft = 2;

  public void Flicker() {
    if (hitsLeft > 0) {
      hitsLeft -= 1;
    } else {
      light.enabled = false;
    }
  }
}
