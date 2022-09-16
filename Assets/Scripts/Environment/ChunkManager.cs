using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class ChunkManager : MonoBehaviour
  {
    public static ChunkManager Instance;
    private PopulateChunk populateChunk;

    //Deactivated chunks
    private Dictionary<Vector2Int, GameObject> deactivatedChunks = 
      new Dictionary<Vector2Int, GameObject>();

    //Lazy chunks
    private Dictionary<Vector2Int, GameObject> lazyChunks = 
      new Dictionary<Vector2Int, GameObject>();

    //Active chunks
    private Dictionary<Vector2Int, GameObject> activeChunks = 
      new Dictionary<Vector2Int, GameObject>();

    //------------------------------------------------------------------------------

    [SerializeField] int chunkDiameter = 200;
    internal int chunkNumberNamer;

    private Transform playerTransform;
    private Vector2 playerCurrentChunkPosition;
    private Vector2 playerLastChunkPosition;

    //------------------------------------------------------------------------------

    internal int starCount;

    public int StarCount { get => starCount; set => starCount = value; }

    private void Awake()
    {
      populateChunk = Resources.Load("ScriptableObjects/PopulateChunkManager") 
        as PopulateChunk;

      //Singleton pattern
      if (Instance == null) {
        Instance = this;
      } else {
        Destroy(gameObject);
      }
    }

    private void Start()
    {
      playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

      playerLastChunkPosition = QuantisePosition(playerTransform.position);

      ActivateOrGenerateChunks(playerLastChunkPosition);
    }

    private void Update()
    {
      if (playerTransform == null) return;

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
      foreach (var chunk in lazyChunks)
      {
        deactivatedChunks.Add(chunk.Key, chunk.Value);
      }

      foreach (var chunk in activeChunks)
      {
        deactivatedChunks.Add(chunk.Key, chunk.Value);
      }
      
      lazyChunks.Clear();
      activeChunks.Clear();
    }

    private void ActivateOrGenerateChunks(Vector2 playerGridKey)
    { 
      //5x5 grid around chunk position
      Vector2Int lazyGridKey = new Vector2Int(
        (int)playerGridKey.x - 2, (int)playerGridKey.y - 2);
      //3x3 grid around chunk position
      Vector2Int activeGridKey = new Vector2Int(
        (int)playerGridKey.x - 1, (int)playerGridKey.y - 1);

      LoadLazyChunks();

      LoadActiveChunks();

      void LoadLazyChunks()
      {
        for (int y = 0; y < 5; y++)
        {
          for (int x = 0; x < 5; x++)
          {
            if (deactivatedChunks.ContainsKey(lazyGridKey))
            {
              GameObject lazyChunk = deactivatedChunks[lazyGridKey];

              lazyChunks.Add(lazyGridKey, lazyChunk);

              deactivatedChunks.Remove(lazyGridKey);
            }
            else   //Make a new chunk
            {
              GameObject newChunk = new GameObject("Chunk " + chunkNumberNamer);
              newChunk.transform.SetParent(this.transform);

              //Populate the chunk
              //newChunk.AddComponent<PopulateChunk>()
                //.Populate(lazyGridKey, chunkDiameter);

              populateChunk.Populate(lazyGridKey, chunkDiameter, newChunk.transform);
              
              lazyChunks.Add(lazyGridKey, newChunk);
              chunkNumberNamer++;
            }

            lazyGridKey.x++;
          }

          lazyGridKey.y++;
          lazyGridKey.x -= 5;   //Need to reset x axis for next row
        }
      }

      void LoadActiveChunks()
      {
        for (int y = 0; y < 3; y++)
        {
          for (int x = 0; x < 3; x++)
          {
            GameObject activeChunk = lazyChunks[activeGridKey];
            activeChunk.SetActive(true);

            activeChunks.Add(activeGridKey, activeChunk);

            lazyChunks.Remove(activeGridKey);

            activeGridKey.x++;
          }

          activeGridKey.y++;
          activeGridKey.x -= 3;   //Need to reset x axis for next row
        }
      }
      
      foreach (var chunk in deactivatedChunks)
      {
        chunk.Value.SetActive(false);
      }
    }
  }
}