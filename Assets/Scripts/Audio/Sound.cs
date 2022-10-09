using System;
using UnityEngine.Audio;
using UnityEngine;

namespace zephkelly
{
  [System.Serializable]
  public class Sound
  {
    public AudioClip clip;

    public string name;

    [Range(0f, 1f)]
    public float volume;

    [Range(0.1f, 2f)]
    public float pitch = 1f;

    public bool looped = false;

    [HideInInspector]
    public AudioSource source;
  }
}
