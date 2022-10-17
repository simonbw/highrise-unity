using UnityEngine;


// Class to call static functions in unity events
[CreateAssetMenu(menuName = "Statics")]
public class Statics : ScriptableObject {

  public static void DestroyGameObject(GameObject gameObject) {
    Destroy(gameObject);
  }
}
