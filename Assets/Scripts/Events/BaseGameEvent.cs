using System;
using UnityEngine;
using UnityEngine.Events;

// Use an interface for all the weird generic stuff goin on
public interface IGameEventListener<T> {
  void OnEventInvoked(T value);
}

// All event types should inherit from this
public class BaseGameEvent<T> : ScriptableObject {
  public event Action<T> OnInvoke;

  public void Invoke(T payload) {
    OnInvoke?.Invoke(payload);
  }

  public void AddListener(IGameEventListener<T> listener) {
    OnInvoke += listener.OnEventInvoked;
  }

  public void RemoveListener(IGameEventListener<T> listener) {
    OnInvoke -= listener.OnEventInvoked;
  }
}

public class BaseGameEventListener<T, E> :
    MonoBehaviour,
    IGameEventListener<T>
    where E : BaseGameEvent<T> {
  public E gameEvent;
  public UnityEvent<T> response;

  private void OnEnable() {
    if (gameEvent == null) {
      Debug.LogWarning($"Empty GameEventListener in: {gameObject.name}");
    } else {
      gameEvent.AddListener(this);
    }
  }

  private void OnDisable() {
    if (gameEvent == null) {
      Debug.LogWarning($"Empty GameEventListener in: {gameObject.name}");
    } else {
      gameEvent.RemoveListener(this);
    }
  }

  public void OnEventInvoked(T value) {
    response.Invoke(value);
  }
}