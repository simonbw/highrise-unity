using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{

  public float speed = 10f;
  // Start is called before the first frame update
  void Start()
  {
    Rigidbody2D body = GetComponent<Rigidbody2D>();
    body.AddRelativeForce(Vector2.right * speed, ForceMode2D.Impulse);
  }

  void OnCollisionEnter2D(Collision2D collision)
  {
    Destroy(gameObject);
    // TODO: Actually do stuff
  }
}
