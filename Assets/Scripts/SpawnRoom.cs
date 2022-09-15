using UnityEngine;

public class SpawnRoom : MonoBehaviour {
  public GameObject player;
  public GameObject spawnPoint;

  // Start is called before the first frame update
  void Start() {
    Instantiate(player, spawnPoint.transform.position, Quaternion.identity, transform);
  }

  // Update is called once per frame
  void Update() {

  }
}
