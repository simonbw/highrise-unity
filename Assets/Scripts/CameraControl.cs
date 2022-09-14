using UnityEngine;

public class CameraControl : MonoBehaviour
{
  public Transform origin;
  public Vector3 offset;
  [Range(0f, 1f)]
  public float smoothTime = 0.1f;
  [Range(0f, 1f)]
  public float mouseBias = 0.5f;

  private Vector3 velocity = new(0f, 0f, 0f);

  void Update()
  {
    var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    var target = Vector3.Lerp(origin.position, mousePos, mouseBias);
    target.z = -10;

    Camera.main.transform.position = Vector3.SmoothDamp(
      Camera.main.transform.position,
      target,
      ref velocity,
      smoothTime
    );
  }
}
