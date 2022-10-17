using System.Collections.Generic;
using UnityEngine;

public static class RandomUtils {
  public static void ShuffleList<T>(List<T> l) {
    System.Random r = new System.Random();
    for (int i = 0; i < l.Count - 2; i++) {
      int swapIdx = r.Next(i + 1, l.Count - 1);
      T temp = l[i];
      l[i] = l[swapIdx];
      l[swapIdx] = temp;
    }
  }

  public static Vector2 Direction() {
    return VectorUtils.FromPolar(Random.Range(0f, 360f), 1);
  }

}

public static class VectorUtils {
  public static Vector2 FromPolar(float angle, float radius) {
    return new Vector2(radius * Mathf.Cos(angle * Mathf.Deg2Rad), radius * Mathf.Sin(angle * Mathf.Deg2Rad));
  }

}

// Adds new methods to Arary
public static class RandomExtension {
  // Choose a random element from this array.
  public static T Choose<T>(this T[] array) {
    if (array.Length == 0) {
      return default;
    }

    return array[Random.Range(0, array.Length)];
  }
}
