using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{

  public float speed = 10f;
  // Start is called before the first frame update

  public GameObject impactPrefab;

  void Start()
  {
    Rigidbody2D body = GetComponent<Rigidbody2D>();
    body.AddRelativeForce(Vector2.right * speed, ForceMode2D.Impulse);

    Destroy(gameObject, 1f);
  }


  void OnCollisionEnter2D(Collision2D collision)
  {
    // Remove from the simulation, but keep around for trail effects
    GetComponent<Rigidbody2D>().simulated = false;

    if (impactPrefab)
    {
      var contact = collision.GetContact(0);
      float normalAngle = Vector2.SignedAngle(Vector2.right, contact.normal);
      Instantiate(impactPrefab, contact.point, Quaternion.Euler(0, 0, normalAngle));
    }

    collision.otherRigidbody.GetComponent<BulletHitDetector>()?.OnBulletHit();
  }
}
