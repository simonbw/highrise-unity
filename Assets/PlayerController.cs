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
    }

    void FixedUpdate() {
        body.velocity = moveDirection * moveSpeed;
    }
}
