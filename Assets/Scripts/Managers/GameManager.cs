using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  //public enum GameState { Menu, Playing, Paused, GameOver }

  public class GameManager : MonoBehaviour
  {
    public static GameManager Instance;

    //public GameState State;
    //public static event Action<GameState> OnGameStateChanged;

    public void Awake()
    {
      if (Instance == null) 
      {
        Instance = this;
      } else {
        Destroy(gameObject);
      }
    }

    public void OnApplicationQuit()
    {
      GameManager.Instance = null;
    } 

    /*
    public void Start()
    {
      UpdateGameState(GameState.Playing);
    }

    public void UpdateGameState(GameState newState)
    {
      State = newState;

      switch (newState)
      {
        case GameState.Menu:
          break;
        case GameState.Playing:
          break;
        case GameState.Paused:
          break;
        case GameState.GameOver:
          break;
        default:
          throw new ArgumentOutOfRangeException("newState", newState, null);
      }

      OnGameStateChanged?.Invoke(newState);
    }
    */
  }
}