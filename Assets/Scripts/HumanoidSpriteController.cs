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
  public Vector2 leftShoulderBase = new(0f, 0.38f);
  public Vector2 rightShoulderBase = new(0f, -0.38f);

  public Vector2 stanceOffset = new(0f, 0f);
  public float stanceAngle = 0f;
  public float headAngle = 0f;

  [Header("Sizes")]
  public float leftArmSize = 0.7f;
  public float rightArmSize = 0.7f;

  // Start is called before the first frame update
  void Update()
  {
    Vector2 leftShoulderPosition = FromPolar(stanceAngle + Vector2.SignedAngle(Vector2.right, leftShoulderBase), leftShoulderBase.magnitude) + stanceOffset;
    Vector2 rightShoulderPosition = FromPolar(stanceAngle + Vector2.SignedAngle(Vector2.right, rightShoulderBase), rightShoulderBase.magnitude) + stanceOffset;

    leftHand.transform.localPosition = leftHandPosition;
    rightHand.transform.localPosition = rightHandPosition;

    torso.transform.localPosition = stanceOffset;
    torso.transform.localEulerAngles = new Vector3(0f, 0f, stanceAngle);

    leftArm.transform.localPosition = (leftShoulderPosition + leftHandPosition) * 0.5f;
    Vector2 leftShoulderDirection = leftHandPosition - leftShoulderPosition;
    leftArm.transform.localEulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.right, leftShoulderDirection));
    leftArm.transform.localScale = new Vector3((leftHandPosition - leftShoulderPosition).magnitude / leftArmSize, 1f, 1f);

    rightArm.transform.localPosition = (rightShoulderPosition + rightHandPosition) * 0.5f;
    Vector2 rightShoulderDirection = rightHandPosition - rightShoulderPosition;
    rightArm.transform.localEulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.right, rightShoulderDirection));
    rightArm.transform.localScale = new Vector3((rightHandPosition - rightShoulderPosition).magnitude / rightArmSize, 1f, 1f);

    head.transform.localEulerAngles = new Vector3(0f, 0f, headAngle);
  }


  // Vector2 getShoulderPositions()
  // {
  //   const r = this.radius - this.armThickness / 2;
  //   return [
  //     polarToVec(stanceAngle - Math.PI / 2, r).iadd(offset),
  //     polarToVec(stanceAngle + Math.PI / 2, r).iadd(offset),
  //   ];
  // }

  private Vector2 FromPolar(float theta, float r)
  {
    return new Vector2(r * Mathf.Cos(theta * Mathf.Deg2Rad), r * Mathf.Sin(theta * Mathf.Deg2Rad));
  }
}


