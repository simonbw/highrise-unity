using UnityEngine;

public class FollowCamera : MonoBehaviour {
  public Transform origin;
  public Camera targetCamera;

  public Vector3 offset;
  [Range(0f, 1f)]
  public float smoothTime = 0.1f;
  [Range(0f, 1f)]
  public float mouseBias = 0.5f;

  private Vector3 velocity = new(0f, 0f, 0f);

  void Update() {
    if (targetCamera == null) {
      targetCamera = Camera.main;
    }

    var mousePos = targetCamera.ScreenToWorldPoint(Input.mousePosition);
    var target = Vector3.Lerp(origin.position, mousePos, mouseBias);
    target.z = -10;

    targetCamera.transform.position = Vector3.SmoothDamp(
      targetCamera.transform.position,
      target,
      ref velocity,
      smoothTime
    );
  }
}
