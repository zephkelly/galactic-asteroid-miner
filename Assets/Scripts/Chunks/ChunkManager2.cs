using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class ChunkManager2 : MonoBehaviour
  {
    public static ChunkManager2 Instance { get; private set; }

    private OcclusionManager2 occlusionManager;
    private ChunkPopulator2 chunkPopulator = new ChunkPopulator2();
    private PrefabInstantiator prefabGetter = new PrefabInstantiator();

    private Dictionary<Vector2Int, Chunk2> activeChunks = 
      new Dictionary<Vector2Int, Chunk2>();

    private Dictionary<Vector2Int, Chunk2> lazyChunks = 
      new Dictionary<Vector2Int, Chunk2>();

    private Dictionary<Vector2Int, Chunk2> inactiveChunks = 
      new Dictionary<Vector2Int, Chunk2>();

    //------------------------------------------------------------------------------

    [SerializeField] int chunkDiameter;   // = 100;
    internal int chunkName;
    internal int starCount;

    private Transform managerTransform;
    private Transform playerTransform;
    private Vector2Int playerChunkPosition;
    private Vector2Int playerLastChunkPosition;

    //------------------------------------------------------------------------------

    public OcclusionManager2 OcclusionManager { get => occlusionManager; }
    public PrefabInstantiator Instantiator { get => prefabGetter; }

    public Dictionary<Vector2Int, Chunk2> ActiveChunks { get => activeChunks; }
    public Dictionary<Vector2Int, Chunk2> LazyChunks { get => lazyChunks; }
    public Dictionary<Vector2Int, Chunk2> InactiveChunks { get => inactiveChunks; }

    public int StarCount { get => starCount; }
    public int ChunkDiameter { get => chunkDiameter; }

    //------------------------------------------------------------------------------

    private void Awake()
    {
      playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
      occlusionManager = new OcclusionManager2(playerTransform, this);
      managerTransform = transform;

      //Singleton pattern
      if (Instance == null) {
        Instance = this;
      } else {
        Destroy(gameObject);
      }
    }

    private void Start()
    {
      playerChunkPosition = QuantisePosition(playerTransform.position);

      GenerateChunks(playerChunkPosition);
      playerLastChunkPosition = playerChunkPosition;
    }

    public Vector2Int QuantisePosition(Vector2 _position)
    {
      return new Vector2Int(
        Mathf.RoundToInt(_position.x / chunkDiameter),
        Mathf.RoundToInt(_position.y / chunkDiameter)
      );
    }

    public Chunk2 GetChunk(Vector2Int _chunkPosition)
    {
      if (activeChunks.ContainsKey(_chunkPosition)) {
        return activeChunks[_chunkPosition];
      } else if (lazyChunks.ContainsKey(_chunkPosition)) {
        return lazyChunks[_chunkPosition];
      } else if (inactiveChunks.ContainsKey(_chunkPosition)) {
        return inactiveChunks[_chunkPosition];
      } else {
        Debug.LogError("Chunk not found");
        return null;
      }
    }

    private void Update()
    {
      if (playerTransform == null) return;

      playerChunkPosition = QuantisePosition(playerTransform.position);

      if (playerChunkPosition != playerLastChunkPosition) 
      {
        GenerateChunks(playerChunkPosition);
        playerLastChunkPosition = playerChunkPosition;
      }

      occlusionManager.UpdateOcclusion();
    }

    private void GenerateChunks(Vector2Int center)
    {
      Vector2Int lazyGridKey = new Vector2Int(center.x -2, center.y -2);
      Vector2Int activeGridKey = new Vector2Int(center.x -1, center.y -1);

      //Order important!
      DeactivateChunks();

      GenerateLazyChunks(lazyGridKey);

      chunkPopulator.PopulateLargeBodies(lazyChunks);

      SetActiveChunks(activeGridKey);

      chunkPopulator.PopulateSmallBodies(activeChunks);
    }

    private void DeactivateChunks()
    {
      foreach (var chunk in activeChunks)
      { 
        Chunk2 chunkInfo = chunk.Value;

        chunkInfo.AttachedObject.SetActive(false);

        inactiveChunks.Add(chunkInfo.Key, chunkInfo);
      }

      foreach (var chunk in lazyChunks)
      { 
        Chunk2 chunkInfo = chunk.Value;

        chunkInfo.AttachedObject.SetActive(false);

        inactiveChunks.Add(chunkInfo.Key, chunkInfo);
      }  

      activeChunks.Clear();
      lazyChunks.Clear();
    }

    private void GenerateLazyChunks(Vector2Int _lazyKey)
    {
      Vector2Int lazyKey = _lazyKey;

      //5x5 grid: 25 lazy chunks
      for (int y = 0; y < 5; y++)
      {
        for (int x = 0; x < 5; x++)
        {
          if (lazyChunks.ContainsKey(lazyKey)) return;

          if (inactiveChunks.ContainsKey(lazyKey))   //Already exists
          {
            Chunk2 newLazyChunk = inactiveChunks[lazyKey];

            lazyChunks.Add(lazyKey, newLazyChunk);
            inactiveChunks.Remove(lazyKey);
          }
          else   //Create a new chunk
          {
            GameObject newChunkObject = new GameObject("Chunk " + chunkName);
            newChunkObject.transform.SetParent(managerTransform);
            newChunkObject.SetActive(false);

            //Populate chunk with asteroids

            Chunk2 newChunk = new Chunk2(lazyKey, chunkDiameter, newChunkObject);
            lazyChunks.Add(lazyKey, newChunk);
            chunkName++;
          }

          lazyKey.x++;
        }

        lazyKey.y++;
        lazyKey.x -= 5;
      }
    }

    private void SetActiveChunks(Vector2Int _activeKey)
    {
      Vector2Int activeKey = _activeKey;

      //3x3 grid: 9 active chunks
      for (int y = 0; y < 3; y++)
      {
        for (int x = 0; x < 3; x++)
        {
          if (lazyChunks.ContainsKey(activeKey))
          {
            Chunk2 newActiveChunk = lazyChunks[activeKey];
            newActiveChunk.AttachedObject.SetActive(true);

            activeChunks.Add(activeKey, newActiveChunk);
            lazyChunks.Remove(activeKey);
          }
          else
          {
            Debug.LogError("Error: chunk does not exist");
            return;
          }

          activeKey.x++;
        }

        activeKey.y++;
        activeKey.x -= 3;
      }
    }
  }
}