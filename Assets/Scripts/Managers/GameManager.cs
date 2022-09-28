using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  //public enum GameState { Menu, Playing, Paused, GameOver }

  public class GameManager : MonoBehaviour
  {
    public static GameManager Instance;
    private ChunkManager chunkManager;

    private CameraController cameraController;

    private GameObject playerPrefab;


    private void Awake()
    {
      playerPrefab = Resources.Load<GameObject>("Prefabs/Player");

      if (Instance == null) 
      {
        Instance = this;
      } else {
        Destroy(gameObject);
      }
    }

    private void Start()
    {
      cameraController = CameraController.Instance;
      chunkManager = ChunkManager.Instance;
      ShipController.OnPlayerDied += RespawnPlayer;
    }

    private void RespawnPlayer()
    {
      StartCoroutine(RespawnPlayerCoroutine());

      IEnumerator RespawnPlayerCoroutine()
      {
        yield return new WaitForSeconds(4f);

        var respawnedPlayer = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        var playerNewTransform = respawnedPlayer.transform;

        cameraController.ChangeFocus(playerNewTransform);
        chunkManager.UpdatePlayerTransform(playerNewTransform);
      }
    }

    private void OnApplicationQuit()
    {
      GameManager.Instance = null;
    } 
  }
}