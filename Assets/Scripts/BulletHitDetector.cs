using UnityEngine;
using UnityEngine.Events;

// For things that can be hit by a bullet
public class BulletHitDetector : MonoBehaviour {

  [Tooltip("Color that the impact particle effect will use")]
  public Gradient impactColor;

  [Tooltip("Called when hit by a bullet")]
  public UnityEvent onHit;

  public void OnBulletHit() {
    onHit.Invoke();
  }
}
