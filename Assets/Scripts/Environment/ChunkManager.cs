using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class ChunkManager : MonoBehaviour
  {
    [SerializeField] int chunkDiameter = 500;

    //Deactivated chunks
    Dictionary<Vector2, GameObject> deactivatedChunks = new Dictionary<Vector2, GameObject>();

    //Activated chunks
    Dictionary<Vector2, GameObject> activeChunks = new Dictionary<Vector2, GameObject>();

    //----------------------------------------------------------------------------------------------

    private Transform playerTransform;

    private Vector2 playerCurrentChunkPosition;

    private Vector2 playerLastChunkPosition;

    private int _chunkNamer;

    //----------------------------------------------------------------------------------------------

    private void Start()
    {
      playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

      playerLastChunkPosition = QuantisePosition(playerTransform.position);

      ActivateOrGenerateChunks(playerLastChunkPosition);
    }

    private void Update()
    {
      playerCurrentChunkPosition = QuantisePosition(playerTransform.position);

      if (playerCurrentChunkPosition != playerLastChunkPosition)
      {
        DeactivateActiveChunks();
        ActivateOrGenerateChunks(playerCurrentChunkPosition);

        playerLastChunkPosition = playerCurrentChunkPosition;
      }
    }

    //Quantise position to nearest chunk
    private Vector2 QuantisePosition(Vector2 position)
    {
      return new Vector2(
        Mathf.RoundToInt(position.x / chunkDiameter),
        Mathf.RoundToInt(position.y / chunkDiameter)
      );
    }

    private void DeactivateActiveChunks()
    {
      foreach (var chunk in activeChunks)
      {
        deactivatedChunks.Add(chunk.Key, chunk.Value);
      }
      
      activeChunks.Clear();
    }

    private void ActivateOrGenerateChunks(Vector2 key)
    { 
      //3x3 grid around chunk position
      Vector2Int gridAroundKey = new Vector2Int((int)key.x - 1, (int)key.y - 1);

      for (int y = 0; y < 3; y++)
      {
        for (int x = 0; x < 3; x++)
        {
          if (deactivatedChunks.ContainsKey(gridAroundKey))
          {
            GameObject reactivatedChunk = deactivatedChunks[gridAroundKey];
            reactivatedChunk.SetActive(true);

            activeChunks.Add(gridAroundKey, reactivatedChunk);
            deactivatedChunks.Remove(gridAroundKey);

            print("Reactivating chunks");
          }
          else   //Make a new chunk
          {
            GameObject newChunk = new GameObject("Chunk " + _chunkNamer);
            newChunk.transform.SetParent(this.transform);

            //Populate the chunk
            newChunk.AddComponent<PopulateChunk>().Populate(gridAroundKey, chunkDiameter);
            
            activeChunks.Add(gridAroundKey, newChunk);
            _chunkNamer++;

            print("Generating chunks");
          }

          gridAroundKey.x++;
        }

        gridAroundKey.y++;
        gridAroundKey.x -= 3;   //Need to reset x axis for next row
      }

      //After chunks are made we 
      foreach (var chunk in deactivatedChunks)
      {
        chunk.Value.SetActive(false);
      }
    }
  }
}