using System.Collections.Generic;

public static class RandomUtils {
  public static void shuffleList<T>(List<T> l) {
    System.Random r = new System.Random();
    for (int i = 0; i < l.Count - 2; i++) {
      int swapIdx = r.Next(i + 1, l.Count - 1);
      T temp = l[i];
      l[i] = l[swapIdx];
      l[swapIdx] = temp;
    }
  }
}
