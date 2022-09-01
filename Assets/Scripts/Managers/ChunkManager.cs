using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class ChunkManager : MonoBehaviour
  {
    //Chunk size
    [SerializeField] int chunkDiameter = 500;

    //Deactivated chunks
    Dictionary<Vector2, GameObject> deactivatedChunks = new Dictionary<Vector2, GameObject>();

    //Activated chunks
    Dictionary<Vector2, GameObject> activeChunks = new Dictionary<Vector2, GameObject>();

    //----------------------------------------------------------------------------------------------

    private Transform playerTransform;

    private Vector2 playerCurrentChunk;

    private Vector2 playerLastChunk;

    //----------------------------------------------------------------------------------------------

    private static float _quantisePositionTime = 0.1f;

    private float _quantiseTimer;

    private int _chunkNamer;

    //----------------------------------------------------------------------------------------------

    private void Start()
    {
      //Get the player transform
      playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
      //Get the player position rounded to the nearest chunk
      playerLastChunk = QuantisePosition(playerTransform.position);

      ActivateOrGenerateChunk(playerLastChunk);
    }

    private void Update()
    {
      playerCurrentChunk = QuantisePosition(playerTransform.position);

      //If we are in a new chunk, make a new chunk
      if (playerCurrentChunk != playerLastChunk)
      {
        playerLastChunk = playerCurrentChunk;
        ActivateOrGenerateChunk(playerCurrentChunk);
      }
    }

    //Function to quantise position to the nearest chunk
    private Vector2 QuantisePosition(Vector2 position)
    {
      if (_quantiseTimer < 0)
      {
        _quantiseTimer = _quantisePositionTime;

        return new Vector2(
          Mathf.RoundToInt(position.x / chunkDiameter),
          Mathf.RoundToInt(position.y / chunkDiameter)
        );
      }
      else
      {
        _quantiseTimer -= Time.deltaTime;
        return playerLastChunk;
      }
    }

    private void ActivateOrGenerateChunk(Vector2 key)
    { 
      //Split the vector and minus one by both values
      Vector2Int gridAroundKey = new Vector2Int((int)key.x - 1, (int)key.y - 1);

      //Loop through the grid around the key
      for (int y = 0; y < 3; y++)
      {
        for (int x = 0; x < 3; x++)
        {
          if (activeChunks.ContainsKey(gridAroundKey)) return;

          if (deactivatedChunks.ContainsKey(gridAroundKey))
          {
            Debug.Log("Activating chunk: " + gridAroundKey);
            
            GameObject reactivatedChunk = deactivatedChunks[gridAroundKey];
            reactivatedChunk.SetActive(true);

            activeChunks.Add(gridAroundKey, reactivatedChunk);
            deactivatedChunks.Remove(gridAroundKey);
          }
          else
          {
            Debug.Log("Generating new chunk: " + gridAroundKey);

            GameObject newChunk = new GameObject("Chunk " + _chunkNamer);

            activeChunks.Add(gridAroundKey, newChunk);
            _chunkNamer++;
          }

          gridAroundKey.x++;
        }

        gridAroundKey.y++;
        //Reset the x axis to keep it all aligned
        gridAroundKey.x -= 3;
      }
    }
  }
}