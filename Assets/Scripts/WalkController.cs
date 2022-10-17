using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkController : MonoBehaviour {

  public float walkSpeed = 4f;
  public float acceleration = 10f;
  public Rigidbody2D body;

  Vector2 targetVelocity = new(0, 0);

  public void WalkTowards(float angle, float speedPercent = 1f) {
    targetVelocity = VectorUtils.FromPolar(angle, speedPercent * walkSpeed);
  }

  public void Stop() {
    this.targetVelocity.Set(0f, 0f);
  }

  // Update is called once per frame
  void FixedUpdate() {
    body.AddForce((this.targetVelocity - body.velocity) * body.mass * acceleration);
  }
}
