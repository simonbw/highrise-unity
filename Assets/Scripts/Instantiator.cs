using UnityEngine;

public class Instantiator : MonoBehaviour {
  public GameObject prefab;
  public Transform instanceTransform;

  public void Instantiate() {
    Instantiate(prefab, instanceTransform.position, instanceTransform.rotation);
  }
}
