using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidSpriteController : MonoBehaviour
{

  // References
  [Header("References")]
  public GameObject head;
  public GameObject torso;
  public GameObject rightArm;
  public GameObject leftArm;
  public GameObject rightHand;
  public GameObject leftHand;

  // Local Positions
  [Header("Positions")]
  public Vector2 leftHandPosition = new(0.5f, 0.2f);
  public Vector2 rightHandPosition = new(0.5f, -0.2f);
  public Vector2 leftShoulderPosition = new(0f, 0.38f);
  public Vector2 rightShoulderPosition = new(0f, -0.38f);

  [Header("Sizes")]
  public float leftArmSize = 0.7f;
  public float rightArmSize = 0.7f;

  // Start is called before the first frame update
  void Update()
  {
    // TODO: Set arm/hand positions
    leftHand.transform.localPosition = leftHandPosition;
    rightHand.transform.localPosition = rightHandPosition;

    // Arm position
    leftArm.transform.localPosition = (leftShoulderPosition + leftHandPosition) * 0.5f;
    rightArm.transform.localPosition = (rightShoulderPosition + rightHandPosition) * 0.5f;

    // Arm rotation
    Vector2 leftShoulderDirection = leftHandPosition - leftShoulderPosition;
    leftArm.transform.localRotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, leftShoulderDirection));

    Vector2 rightShoulderDirection = rightHandPosition - rightShoulderPosition;
    rightArm.transform.localRotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, rightShoulderDirection));

    // Arm size
    leftArm.transform.localScale = new Vector3((leftHandPosition - leftShoulderPosition).magnitude / leftArmSize, 1f, 1f);
    rightArm.transform.localScale = new Vector3((rightHandPosition - rightShoulderPosition).magnitude / rightArmSize, 1f, 1f);
  }
}
