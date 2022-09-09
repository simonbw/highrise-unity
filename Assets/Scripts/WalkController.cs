using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkController : MonoBehaviour
{

  public float walkSpeed = 4f;
  public float acceleration = 10f;
  public Rigidbody2D body;

  Vector2 targetVelocity = new(0, 0);

  public void WalkTowards(float angle, float speedPercent = 1f)
  {
    targetVelocity = FromPolar(angle, speedPercent * walkSpeed);
  }

  public void Stop()
  {
    this.targetVelocity.Set(0f, 0f);
  }

  // Update is called once per frame
  void FixedUpdate()
  {
    body.AddForce((this.targetVelocity - body.velocity) * body.mass * acceleration);
  }

  private Vector2 FromPolar(float theta, float r)
  {
    return new Vector2(r * Mathf.Cos(theta * Mathf.Deg2Rad), r * Mathf.Sin(theta * Mathf.Deg2Rad));
  }
}
