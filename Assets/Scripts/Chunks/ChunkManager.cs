using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class ChunkManager : MonoBehaviour
  {
    public static ChunkManager Instance;
    private ChunkPopulator chunkPopulator;

    //Deactivated chunks
    private Dictionary<Vector2Int, Chunk> deactivatedChunks = 
      new Dictionary<Vector2Int, Chunk>();

    //Lazy chunks
    private Dictionary<Vector2Int, Chunk> lazyChunks = 
      new Dictionary<Vector2Int, Chunk>();

    //Active chunks
    private Dictionary<Vector2Int, Chunk> activeChunks = 
      new Dictionary<Vector2Int, Chunk>();

    //------------------------------------------------------------------------------

    [SerializeField] int chunkDiameter = 200;
    internal int chunkNumberNamer;

    private Transform playerTransform;
    private Vector2 playerCurrentChunkPosition;
    private Vector2 playerLastChunkPosition;

    //------------------------------------------------------------------------------

    internal int starCount;

    public Dictionary <Vector2Int, Chunk> ActiveChunks { 
      get => activeChunks;
      set => activeChunks = value;
    }

    public Dictionary <Vector2Int, Chunk> LazyChunks { 
      get => deactivatedChunks;
      set => deactivatedChunks = value;
    }

    public Dictionary <Vector2Int, Chunk> DeactivatedChunks { 
      get => deactivatedChunks;
      set => deactivatedChunks = value;
    }

    public int StarCount { get => starCount; set => starCount = value; }

    private void Awake()
    {
      chunkPopulator = Resources.Load("ScriptableObjects/ChunkPopulator") 
        as ChunkPopulator;

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
              Chunk lazyChunk = deactivatedChunks[lazyGridKey];

              lazyChunks.Add(lazyGridKey, lazyChunk);

              deactivatedChunks.Remove(lazyGridKey);
            }
            else   //Make a new chunk
            {
              GameObject newChunkObject = new GameObject("Chunk " + chunkNumberNamer);
              newChunkObject.transform.SetParent(this.transform);

              //Create chunk class with gameobject references
              Chunk newChunkInfo = new Chunk();
              newChunkInfo.SetChunkObject(lazyGridKey, newChunkObject);
              
              chunkPopulator.Populate(lazyGridKey, chunkDiameter, newChunkInfo);

              lazyChunks.Add(lazyGridKey, newChunkInfo);
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
            Chunk activeChunk = lazyChunks[activeGridKey];
            activeChunk.ChunkObject.SetActive(true);

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
        chunk.Value.ChunkObject.SetActive(false);
      }

      foreach (var chunk in lazyChunks)
      {
        chunk.Value.ChunkObject.SetActive(false);
      }
    }
  }
}