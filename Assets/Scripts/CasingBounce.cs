using UnityEngine;

public class CasingBounce : MonoBehaviour
{
  public float gravity = -9.8f;
  public float minBounceSpeed = 1f;
  public float bounceRestitution = 0.3f;
  public float zScaling = 5.8f;

  public AudioSource[] bounceSounds;

  public float z;
  public float zVelocity;

  // Start is called before the first frame update
  void Start()
  {
    z = 1f;
  }

  // Update is called once per frame
  public void Update()
  {
    var body = GetComponent<Rigidbody2D>();

    if (body)
    {
      if (z >= 0)
      { // Falling
        z += zVelocity * Time.deltaTime;
        zVelocity += gravity * Time.deltaTime;
      }
      else
      { // Hitting the ground

        z = 0;

        if (Mathf.Abs(zVelocity) > minBounceSpeed)
        {
          // bounce
          zVelocity *= -1f * bounceRestitution;
          body.angularVelocity *= bounceRestitution;
          body.velocity *= bounceRestitution;

          // TODO: Play sound
          var sound = bounceSounds[Random.Range(0, bounceSounds.Length)];
          sound.volume = Mathf.Clamp01(Mathf.Abs(zVelocity) * 0.1f);
          sound.Play();
        }
        else
        {
          // laying still
          zVelocity = 0;
          Destroy(GetComponent<Collider2D>());
          Destroy(body);
          Destroy(this);
        }
      }

      var scale = 1f + z * zScaling;
      transform.localScale = new Vector3(scale, scale, scale);
    }
  }

  public void OnCollisionEnter2D()
  {
    var sound = bounceSounds[Random.Range(0, bounceSounds.Length)];
    sound.volume = Mathf.Clamp01(GetComponent<Rigidbody2D>().velocity.magnitude);
    sound.Play();
  }
}
