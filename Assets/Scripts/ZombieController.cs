using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ZombieController : MonoBehaviour {
  // Start is called before the first frame update
  public float health = 100f;

  public GameObject dropPrefab;
  public GameObject splatterPrefab;

  public UnityEvent onDie;

  // Update is called once per frame
  void Update() {
    var nearestHuman = GameObject.FindGameObjectsWithTag("Human")
      .OrderBy((human) => (human.transform.position - transform.position).sqrMagnitude)
      .First();

    Vector2 walkDireciton = nearestHuman.transform.position - transform.position;
    float targetAngle = Vector2.SignedAngle(Vector2.right, walkDireciton);
    GetComponent<AimController>().targetAngle = targetAngle;
    GetComponent<WalkController>().WalkTowards(targetAngle);
  }

  public void OnHit() {
    float damage = 30;
    health -= damage;

    // TODO: Get this based on location, maybe don'd do it in this controller
    int numDrops = 3;
    for (int i = 0; i < numDrops; i++) {
      var drop = Instantiate(dropPrefab, transform.position, transform.rotation);
      drop.GetComponent<Rigidbody2D>()?.AddForce(RandomUtils.Direction() * damage * Random.Range(0.1f, 1f));
      var dropScaler = drop.GetComponent<ZScaler>();
      if (dropScaler) {
        var dropSize = Random.Range(0.5f, 1.5f) * damage / 30f;
        dropScaler.scaleOnFloor *= dropSize;
      }
    }

    if (health <= 0f) {
      var splatter = Instantiate(splatterPrefab, transform.position, transform.rotation);
      onDie.Invoke();
      Destroy(gameObject);
    }
  }
}
