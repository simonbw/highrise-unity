using UnityEngine;


// Adds new methods to Arary
public static class RandomExtension
{
  // Choose a random element from this array.
  public static T Choose<T>(this T[] array)
  {
    if (array.Length == 0)
    {
      return default;
    }

    return array[Random.Range(0, array.Length)];
  }
}