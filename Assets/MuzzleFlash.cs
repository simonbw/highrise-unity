using Unity.VisualScripting;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{

  public Sprite[] muzzleFlashSprites;
  public float lifespan = 0.1f; // in seconds

  float timeLeft;

  // Start is called before the first frame update
  void Start()
  {
    timeLeft = lifespan;
    int i = Random.Range(0, muzzleFlashSprites.Length);
    GetComponent<SpriteRenderer>().sprite = muzzleFlashSprites[i];
  }

  // Update is called once per frame
  void Update()
  {
    var sprite = GetComponent<SpriteRenderer>();
    var t = timeLeft / lifespan;

    sprite.color = new Color(1f, 1f, 1f, t);
    var scale = 2f - Mathf.Pow(t, 2f);
    transform.localScale.Set(scale, scale, scale);

    timeLeft -= Time.deltaTime;
    if (timeLeft <= 0)
    {
      Destroy(gameObject);
    }
  }
}