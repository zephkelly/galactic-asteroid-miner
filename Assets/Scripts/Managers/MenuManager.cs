using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace zephkelly
{
  public class MenuManager : MonoBehaviour
  {
    private AudioManager audioManager;
    private GameObject mainMenu;
    private GameObject optionsMenu;

    private const float musicBreathingRoom = 5f;
    private float currentSongTimeRemaining = 0;

    private void Awake()
    {
      mainMenu = GameObject.FindGameObjectWithTag("MainMenu");
      optionsMenu = GameObject.FindGameObjectWithTag("OptionsMenu");
      
      audioManager = GetComponent<AudioManager>();
    }

    private void Start()
    {
      mainMenu.SetActive(true);
      optionsMenu.SetActive(false);

      PlayRandomMenuMusic();
    }

    private void PlayRandomMenuMusic()
    {
      var randomMusic = Random.Range(0, 2);
      audioManager.PlayMusic("Menu_" + randomMusic);
      currentSongTimeRemaining = audioManager.GetClipLength("Menu_" + randomMusic) + musicBreathingRoom;
    }

    public void Update()
    {
      if (currentSongTimeRemaining > 0) currentSongTimeRemaining -= Time.deltaTime;
      else PlayRandomMenuMusic();
    }

    public void PlayGame()
    {
      audioManager.SaveOptionsOnScriptable();
      SceneManager.LoadScene(1);
      audioManager.SaveOptionsOnScriptable();
    }

    public void SaveSoundOptions()
    {
      audioManager.SaveOptionsOnScriptable();
    }

    public void EnterOptions()
    {
      mainMenu.SetActive(false);
      optionsMenu.SetActive(true);
    }

    public void ExitOptions()
    {
      mainMenu.SetActive(true);
      optionsMenu.SetActive(false);
    }

    public void QuitGame()
    {
      Application.Quit();
    }
  }
}