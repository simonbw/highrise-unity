using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletImpact : MonoBehaviour {
  public void Init(Gradient color) {
    var particles = GetComponent<ParticleSystem>();
    var colorOverLifetime = particles.colorOverLifetime;
    colorOverLifetime.color = color;
  }
}
