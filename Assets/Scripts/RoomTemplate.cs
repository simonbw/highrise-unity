using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomTemplate : MonoBehaviour {
  public GameObject[] doors;
  public GameObject tileMap;

  public Vector2Int? getDimensions() {
    Tilemap t = tileMap.GetComponent(typeof(Tilemap)) as Tilemap;
    if (t == null) {
      Debug.LogWarning("expected only Tilemap components");
      return null;
    }
    return (Vector2Int)t.size;
  }

  void Start() {
    foreach (GameObject d in doors) {
      Vector2 p = d.transform.localPosition;
      Debug.Assert(p.x >= 0 && p.y >= 0, "Door has negative position " + p);
    }
  }

  void Update() {

  }
}
