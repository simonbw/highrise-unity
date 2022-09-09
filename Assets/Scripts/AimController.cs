using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
  public float acceleration = 0.01f;
  public float accelExp = 0.9f;
  public float dampingFactor = 0.01f;
  public float dampingExp = 0.9f;

  public Rigidbody2D body;
  public float targetAngle = 0f;

  // Update is called once per frame
  void Update()
  {
    float angleDisplacement = Mathf.DeltaAngle(body.rotation, targetAngle);
    float dampingTorque = dampingFactor * -Mathf.Sign(body.angularVelocity) * Mathf.Pow(Mathf.Abs(body.angularVelocity), dampingExp);
    float aimingTorque = acceleration * Mathf.Sign(angleDisplacement) * Mathf.Pow(Mathf.Abs(angleDisplacement), accelExp);
    float torque = aimingTorque + dampingTorque;
    body.AddTorque(torque);
  }

  // Return the difference between two angles
  float AngleDelta(float a, float b)
  {
    return ((b - a + 180) % 360) - 180;
  }
}
