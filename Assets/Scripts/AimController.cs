using UnityEngine;

public class AimController : MonoBehaviour
{
  [Header("Paramters")]
  public float maxRotationSpeed = 1440f; // degrees per second
  public float rotationLerpAmount = 0.9f;

  [Header("References")]
  public Rigidbody2D body;

  [Header("Dynamic")]
  public float targetAngle = 0f;

  // Update is called once per frame
  void FixedUpdate()
  {
    body.MoveRotation(Mathf.MoveTowardsAngle(
      current: body.rotation,
      target: Mathf.LerpAngle(body.rotation, targetAngle, rotationLerpAmount),
      maxDelta: maxRotationSpeed * Time.fixedDeltaTime));
  }
}
