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
    private OcclusionManager occlusionManager;

    //Deactivated chunks
    private Dictionary<Vector2Int, Chunk> deactivatedChunks = 
      new Dictionary<Vector2Int, Chunk>();

    //Active chunks
    private Dictionary<Vector2Int, Chunk> activeChunks = 
      new Dictionary<Vector2Int, Chunk>();

    //------------------------------------------------------------------------------

    [SerializeField] int chunkDiameter = 200;
    internal int chunkNumberNamer;

    private Transform playerTransform;
    private Vector2Int playerCurrentChunkPosition;
    private Vector2Int playerLastChunkPosition;

    //------------------------------------------------------------------------------

    internal int starCount;

    public OcclusionManager OcclusionManager { get => occlusionManager; }
    public Transform PlayerTransform { get => playerTransform; }

    public Dictionary <Vector2Int, Chunk> ActiveChunks { 
      get => activeChunks;
    }

    public Dictionary <Vector2Int, Chunk> DeactivatedChunks { 
      get => deactivatedChunks;
    }

    public int StarCount { get => starCount; set => starCount = value; }

    private void Awake()
    {
      chunkPopulator = Resources.Load("ScriptableObjects/ChunkPopulator") 
        as ChunkPopulator;

      occlusionManager = GetComponent<OcclusionManager>();
      playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
  
      //Singleton pattern
      if (Instance == null) {
        Instance = this;
      } else {
        Destroy(gameObject);
      }
    }

    private void Start()
    {
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

    public void AddAsteroidToChunk(Asteroid asteroidInfo)
    {
      var chunkPosition = QuantisePosition(asteroidInfo.CurrentPosition);

      foreach (var chunk in activeChunks)
      {
        if (chunk.Key == chunkPosition)
        {
          chunk.Value.AddForiegnAsteroid(asteroidInfo);
          return;
        }
      }

      foreach (var chunk in deactivatedChunks)
      {
        if (chunk.Key == chunkPosition)
        {
          chunk.Value.AddForiegnAsteroid(asteroidInfo);
         return;
        }
      }
    }

    //Quantise position to nearest chunk
    private Vector2Int QuantisePosition(Vector2 position)
    {
      return new Vector2Int(
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
    }

    private void ActivateOrGenerateChunks(Vector2Int playerGridKey)
    { 
      Vector2Int activeGridKey = new Vector2Int(
        playerGridKey.x - 1, playerGridKey.y - 1);

      activeChunks.Clear();

      for (int y = 0; y < 3; y++)
      {
        for (int x = 0; x < 3; x++)
        {
          if (deactivatedChunks.ContainsKey(activeGridKey))
          {
            Chunk activeChunk = deactivatedChunks[activeGridKey];
            activeChunk.ChunkObject.SetActive(true);

            activeChunks.Add(activeGridKey, activeChunk);
            deactivatedChunks.Remove(activeGridKey);
          }
          else   //Make a new chunk
          {
            GameObject newChunkObject = new GameObject("Chunk " + chunkNumberNamer);
            newChunkObject.transform.SetParent(this.transform);

            Chunk newChunkInfo = new Chunk();
            newChunkInfo.SetChunkObject(activeGridKey, newChunkObject);
            
            chunkPopulator.Populate(activeGridKey, chunkDiameter, newChunkInfo);

            activeChunks.Add(activeGridKey, newChunkInfo);
            chunkNumberNamer++;
          }

          activeGridKey.x++;
        }

        activeGridKey.y++;
        activeGridKey.x -= 3;   //Need to reset x axis for next row
      }
      
      foreach (var chunk in deactivatedChunks)
      {
        chunk.Value.ChunkObject.SetActive(false);
      }
    }
  }
}