using UnityEngine;
using UnityEngine.Events;

// Give something a z value that scales the sprite
public class AudioRing : MonoBehaviour {
  public AudioSource audioSource;
  public AudioClip[] clips;

  public void Start() {
    if (audioSource == null) {
      audioSource = gameObject.AddComponent<AudioSource>();
    }
  }

  public void Play(float volume = 1f) {
    AudioClip clip = clips.Choose();
    if (clip != null && audioSource != null) {
      audioSource.PlayOneShot(clip, volume);
    }
  }
}
