using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

  // Speed of movement
  public float moveSpeed = 5f;

  public Rigidbody2D body;

  Vector2 moveDirection;
  Vector2 mousePos;

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    float moveX = Input.GetAxisRaw("Horizontal");
    float moveY = Input.GetAxisRaw("Vertical");

    moveDirection = new Vector2(moveX, moveY);
    mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
  }

  void FixedUpdate()
  {
    // TODO: Use physics to set velocity, not just setting it directly
    body.velocity = moveDirection * moveSpeed;

    Vector2 aimDirection = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - body.position;
    float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
    body.rotation = aimAngle;
  }
}
