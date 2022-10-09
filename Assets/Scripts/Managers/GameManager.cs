using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace zephkelly
{
  public class GameManager : MonoBehaviour
  {
    public static GameManager Instance;
    private StatisticsManager statisticsManager;

    private GameObject player;

    private GameObject pauseMenu;
    private GameObject optionsMenu;
    private GameObject gameOverMenu;

    private int currentSong = 0;
    private float songTimeRemaining;
    private bool playingMusic = false;

    private bool gamePaused = false;

    public StatisticsManager StatisticsManager { get => statisticsManager; }
    public bool GamePaused { get => gamePaused; }

    private void Awake()
    {
      statisticsManager = Resources.Load("ScriptableObjects/StatisticsManager") as StatisticsManager;
      player = GameObject.FindGameObjectWithTag("Player");

      pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
      optionsMenu = GameObject.FindGameObjectWithTag("OptionsMenu");
      gameOverMenu = GameObject.FindGameObjectWithTag("GameOverMenu");

      if (Instance == null) {
        Instance = this;
      } else {
        Destroy(this);
      }
    }

    private void Start()
    {
      pauseMenu.SetActive(false);
      optionsMenu.SetActive(false);
      gameOverMenu.SetActive(false);

      statisticsManager.Init();
      //zephkelly.AudioManager.Instance.PlayMusic("Game_" + Random.Range(0, 2));
    }

    private void Update()
    {
      GameMusic();

      UpdateStatManager();

      if (Input.GetKeyDown(KeyCode.Escape) && DepoUIManager.Instance.depotToggle != true) 
      {
        gamePaused = !gamePaused;

        if (gamePaused) {
          PauseGame();
        } else {
          ResumeGame();
        }
      }
    }

    public void QuitGame()
    {
      Application.Quit();
    }

    private void GameMusic()
    {
      if (playingMusic) 
      {
        songTimeRemaining -= Time.deltaTime;

        if (songTimeRemaining <= 0)
        {
          playingMusic = false;
        }
      }
      else
      {
        playingMusic = true;

        var randomMusic = UnityEngine.Random.Range(0, 2);

        if (randomMusic == currentSong) {
          randomMusic = (randomMusic + 1) % 2;
        }

        currentSong = randomMusic;
        zephkelly.AudioManager.Instance.PlayMusic("Game_" + currentSong);
        songTimeRemaining = zephkelly.AudioManager.Instance.GetClipLength("Game_" + currentSong) + 8f;
      }
    }

    private void UpdateStatManager()
    {
      statisticsManager.IncrementTimeAlive();

      //increment distance to player
      if (player == null) return;
      var distance = (int)Vector2.Distance(player.transform.position, Vector2.zero);
      statisticsManager.UpdateCurrentDistance(distance);
    }

    public void PauseGame()
    {
      Time.timeScale = 0;
      pauseMenu.SetActive(true);

      zephkelly.AudioManager.Instance.ToggleAllSounds(false);

      gamePaused = true;
    }

    public void ResumeGame()
    {
      Time.timeScale = 1;
      pauseMenu.SetActive(false);
      optionsMenu.SetActive(false);

      zephkelly.AudioManager.Instance.ToggleAllSounds(true);

      gamePaused = false;
    }

    public void GameOver(string deathMessage)
    {
      pauseMenu.SetActive(false);
      DepoUIManager.Instance.DisableMenu();

      StartCoroutine(WaitAndLoadGameOver());

      IEnumerator WaitAndLoadGameOver()
      {
        yield return new WaitForSeconds(3f);
        
        gameOverMenu.SetActive(true);
        GameObject.Find("DeathMessage").GetComponent<TextMeshProUGUI>().text = deathMessage;
        GameObject.Find("GameOverScoreText").GetComponent<TextMeshProUGUI>().text = statisticsManager.GetScore().ToString();
      }
    }

    public void ReturnToMenu(bool saveScore = false)
    {
      if (saveScore) {
        statisticsManager.SaveScore();
      }

      Time.timeScale = 1;
      SceneManager.LoadScene("MenuScene");
    }

    public void RestartGame()
    {
      statisticsManager.SaveScore();

      Time.timeScale = 1;
      SceneManager.LoadScene("GameScene");
    }
  }
}