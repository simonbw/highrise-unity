using UnityEngine;
using UnityEngine.Events;

// Give something a random sprite on start
public class RandomSprite : MonoBehaviour {

  // The sprites to choose from
  public Sprite[] sprites;

  public bool randomAngle = false;

  [Header("Color")]
  public bool useRandomColor;
  public Gradient gradient;

  public void Awake() {
    var renderer = GetComponent<SpriteRenderer>();

    if (renderer) {
      renderer.sprite = sprites.Choose();

      if (randomAngle) {
        transform.Rotate(Vector3.forward, Random.Range(0f, 360f));
      }

      if (useRandomColor) {
        renderer.color = gradient.Evaluate(Random.Range(0f, 1f));
      }
    }
  }
}
