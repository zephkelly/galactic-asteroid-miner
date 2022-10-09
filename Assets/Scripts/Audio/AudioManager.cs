using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace zephkelly
{
  public class AudioManager : MonoBehaviour
  {
    public static AudioManager Instance;
    private AudioScriptable audioSettings;

    public Sound[] effectsSounds;
    public Sound[] musicSounds;

    private bool isMusicOn;
    private bool isEffectsOn;

    private Slider musicSlider;
    private Slider effectsSlider;

    private Toggle musicToggle;
    private Toggle effectsToggle;

    private void Awake()
    {
      if (Instance == null) {
        Instance = this;
      } else {
        Destroy(this);
      }

      musicSlider = GameObject.Find("MusicSlider").GetComponent<Slider>();
      effectsSlider = GameObject.Find("SoundsSlider").GetComponent<Slider>();

      musicToggle = GameObject.Find("MusicToggle").GetComponent<Toggle>();
      effectsToggle = GameObject.Find("SoundsToggle").GetComponent<Toggle>();

      foreach (Sound s in effectsSounds)
      {
        s.source = gameObject.AddComponent<AudioSource>();
        s.source.clip = s.clip;
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
      }

      foreach (Sound s in musicSounds)
      {
        s.source = gameObject.AddComponent<AudioSource>();
        s.source.clip = s.clip;
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
      }

      audioSettings = Resources.Load<AudioScriptable>("ScriptableObjects/AudioSettings");
      GetOptionsFromScriptable();
    }

    public void SaveOptionsOnScriptable()
    {
      audioSettings.SetMusicVolume(musicSlider.value);
      audioSettings.SetEffectsVolume(effectsSlider.value);

      audioSettings.SetMusicOn(musicToggle.isOn);
      audioSettings.SetEffectsOn(effectsToggle.isOn);
    }

    public void GetOptionsFromScriptable()
    {
      musicSlider.value = audioSettings.MusicVolume;
      effectsSlider.value = audioSettings.EffectsVolume;

      musicToggle.isOn = audioSettings.IsMusicOn;
      effectsToggle.isOn = audioSettings.IsEffectOn;

      isMusicOn = audioSettings.IsMusicOn;
      isEffectsOn = audioSettings.IsEffectOn;
    }

    public void PlaySound(string name)
    {
      //if (!isEffectsOn) return;

      Sound sound = Array.Find(effectsSounds, sound => sound.name == name);
      if (sound == null)
      {
        Debug.LogWarning("Sound: " + name + " not found!");
        return;
      }

      sound.source.Play();
    }

    public void PlayMusic(string name)
    {
      //if (!isMusicOn) return;

      Sound song = Array.Find(musicSounds, sound => sound.name == name);
      if (song == null)
      {
        Debug.LogWarning("Music: " + name + " not found!");
        return;
      }

      song.source.Play();
    }

    public void PlaySoundRandomPitch(string name, float min, float max)
    {
      //if (!isEffectsOn) return;

      Sound sound = Array.Find(effectsSounds, sound => sound.name == name);
      if (sound == null)
      {
        Debug.LogWarning("Sound: " + name + " not found!");
        return;
      }

      sound.source.pitch = UnityEngine.Random.Range(min, max);
      sound.source.Play();
    }

    public bool IsSoundPlaying(string name)
    {
      Sound sound = Array.Find(effectsSounds, sound => sound.name == name);
      if (sound == null)
      {
        Debug.LogWarning("Sound: " + name + " not found!");
        return false;
      }

      if (sound.source == null) return false;
      return sound.source.isPlaying;
    }

    public void StopSound(string name)
    {
      Sound sound = Array.Find(effectsSounds, sound => sound.name == name);
      if (sound == null)
      {
        Debug.LogWarning("Sound: " + name + " not found!");
        return;
      }

      sound.source.Stop();
    }

    public void StopMusic(string name)
    {
      Sound song = Array.Find(musicSounds, sound => sound.name == name);
      if (song == null)
      {
        Debug.LogWarning("Music: " + name + " not found!");
        return;
      }

      song.source.Stop();
    }

    public void ToggleSounds()
    {
      isEffectsOn = !isEffectsOn;

      ToggleAllSounds(isEffectsOn);
    }

    public void ToggleMusic()
    {
      isMusicOn = !isMusicOn;

      ToggleAllMusic(isMusicOn);
    }

    public void ToggleAllMusic(bool isOn)
    {
      if (isOn)
      {
        foreach (Sound s in musicSounds)
        {
          s.source.volume = s.volume;
        }
      }
      else
      {
        foreach (Sound s in musicSounds)
        {
          s.source.volume = 0;
        }
      }
    }

    public void ToggleAllSounds(bool isOn)
    {
      if (isOn)
      {
        foreach (Sound s in effectsSounds)
        {
          s.source.volume = s.volume;
        }
      }
      else
      {
        foreach (Sound s in effectsSounds)
        {
          s.source.volume = 0;
        }
      }
    }

    public float GetClipLength(string name)
    {
      Sound song = Array.Find(musicSounds, sound => sound.name == name);
      if (song == null)
      {
        Debug.LogWarning("Music: " + name + " not found!");
        return 0;
      }

      return song.source.clip.length;
    }

    public void UpdateSoundVolumes()
    {
      foreach (Sound s in effectsSounds)
      {
        s.volume = effectsSlider.value;
        s.source.volume = s.volume;
      }
    }

    public void UpdateMusicVolumes()
    {
      foreach (Sound s in musicSounds)
      {
        s.volume = musicSlider.value;
        s.source.volume = s.volume;
      }
    }
  }
}
