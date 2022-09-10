using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Vector3 offset;

    void Start()
    {
    }

    void Update()
    {
        Camera.main.transform.position = new Vector3(
            transform.position.x + offset.x,
            transform.position.y + offset.y,
            offset.z
        );
    }
}
