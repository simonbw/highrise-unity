using UnityEngine;

public struct Void { }

[CreateAssetMenu(menuName = "Events/Game Event")]
public class GameEvent : BaseGameEvent<Void> {
  // Special case
  static readonly Void v;
  public void Invoke() {
    Invoke(v);
  }
}