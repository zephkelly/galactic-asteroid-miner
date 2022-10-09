using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  [CreateAssetMenu(menuName = "ScriptableObjects/AudioSettings")]
  public class AudioScriptable : ScriptableObject
  {
    public float masterEffectsVolume;
    public float masterMusicVolume;

    public bool isMusicOn;
    public bool isEffectOn;

    public float EffectsVolume { get => masterEffectsVolume; }
    public float MusicVolume { get => masterMusicVolume; }

    public bool IsMusicOn { get => isMusicOn; }
    public bool IsEffectOn { get => isEffectOn; }

    public void SetEffectsVolume(float volume)
    {
      masterEffectsVolume = volume;
    }

    public void SetMusicVolume(float volume)
    {
      masterMusicVolume = volume;
    }

    public void SetMusicOn(bool isMuted)
    {
      isMusicOn = isMuted;
    }

    public void SetEffectsOn(bool isMuted)
    {
      isEffectOn = isMuted;
    }
  }
}
