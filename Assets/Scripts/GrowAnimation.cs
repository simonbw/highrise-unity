using UnityEngine;

public class GrowAnimation : MonoBehaviour {

  public float startScale = 0.0f;
  public float duration = 0.5f;

  float endScale;
  float timeLeft;

  void Awake() {
    timeLeft = duration;
    endScale = transform.localScale.x;
  }

  // Start is called before the first frame update
  void Start() {
    endScale = transform.localScale.x;
  }

  // Update is called once per frame
  void Update() {
    timeLeft -= Time.deltaTime;

    var scale = Mathf.Lerp(endScale, startScale, timeLeft / duration);
    transform.localScale = new Vector3(scale, scale, 1f);

    if (timeLeft < 0f) {
      Destroy(this);
    }
  }
}
