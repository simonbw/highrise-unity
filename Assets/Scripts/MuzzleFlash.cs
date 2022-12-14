using UnityEngine;
using UnityEngine.Rendering.Universal;
public class MuzzleFlash : MonoBehaviour {

  public float lifespan = 0.1f; // in seconds

  [Header("references")]
  public SpriteRenderer sprite;
  public Light2D spotLight;
  public Light2D spriteLight;

  float timeLeft;

  // Start is called before the first frame update
  void Start() {
    timeLeft = lifespan;
    spriteLight.lightCookieSprite = sprite.sprite;
  }

  // Update is called once per frame
  void Update() {
    if (timeLeft > 0) {
      var t = Mathf.Clamp01(timeLeft / lifespan);

      spotLight.intensity = Mathf.Pow(t, 2f);

      sprite.color = new Color(1f, 1f, 1f, Mathf.Sqrt(t));

      var scale = 2f - Mathf.Pow(t, 2f);
      sprite.transform.localScale.Set(scale, scale, scale);

      timeLeft -= Time.deltaTime;

      // Called once, when we run out of time
      if (timeLeft <= 0) {
        Destroy(sprite);
        Destroy(spriteLight);
        Destroy(spotLight);
      }
    } else {
      var particleSystems = GetComponentsInChildren<ParticleSystem>();
      if (particleSystems.Length == 0) {
        Destroy(gameObject);
      }
    }
  }
}