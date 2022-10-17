using UnityEngine;
using UnityEngine.Events;

// Give something a z value that scales the sprite
public class ZScaler : MonoBehaviour {
  [Header("Physics")]
  public float gravity = -9.8f;
  public float minBounceSpeed = 1f; // Minimum speed to bounce instead of just landing on the floor
  public float bounceRestitution = 0.0f; // What percent of speed is retained on bounce. 0 for no bounce.
  public float zScaling = 5.8f; // How much the sprite size changes with z
  public float scaleOnFloor = 1.0f; // What scale the sprite should be at when z = 0

  public float z = 1f; // Height in meters above the ground
  public float zVelocity; // In meters/sec

  [Header("Audio")]
  public AudioRing bounceSounds;

  [Header("Events")]
  public UnityEvent<float> OnBounce;
  public UnityEvent OnStop;
  public bool destroyOnStop = false;

  // Update is called once per frame
  public void Update() {
    var body = GetComponent<Rigidbody2D>();

    if (body) {
      if (z >= 0) { // Falling
        z += zVelocity * Time.deltaTime;
        zVelocity += gravity * Time.deltaTime;
      } else { // Hitting the ground

        z = 0;

        if (bounceRestitution > 0 && Mathf.Abs(zVelocity) > minBounceSpeed) {
          zVelocity *= -1f * bounceRestitution;

          body.angularVelocity *= bounceRestitution;
          body.velocity *= bounceRestitution;

          var volume = Mathf.Clamp01(Mathf.Abs(zVelocity) * 0.1f);
          bounceSounds?.Play(volume);

          OnBounce.Invoke(volume);
        } else {
          // laying still
          zVelocity = 0;
          OnStop.Invoke();

          Destroy(GetComponent<Collider2D>());
          Destroy(body);
          Destroy(this);

          if (destroyOnStop) {
            Destroy(gameObject);
          }
        }
      }

      var scale = (1f + z * zScaling) * scaleOnFloor;
      transform.localScale = new Vector3(scale, scale, scale);
    }
  }

  public void OnCollisionEnter2D() {
    var volume = Mathf.Clamp01(GetComponent<Rigidbody2D>().velocity.magnitude);
    bounceSounds?.Play(volume);
    OnBounce.Invoke(volume);
  }
}
