using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class ChunkManager2 : MonoBehaviour
  {
    public static ChunkManager2 Instance;

    private ChunkPopulator2 chunkPopulator = new ChunkPopulator2();
    private OcclusionManager2 occlusionManager;
    private PrefabInstantiator prefabInstantiator;


    [SerializeField] int chunkDiameter = 100;
    internal int chunkNumber;

    private Transform playerTransform;
    private Vector2Int playerChunkPosition;
    private Vector2Int playerLastChunkPosition;

    private Dictionary<Vector2Int, Chunk2> activeChunks =
      new Dictionary<Vector2Int, Chunk2>();

    private Dictionary<Vector2Int, Chunk2> lazyChunks = 
      new Dictionary<Vector2Int, Chunk2>();

    private Dictionary<Vector2Int, Chunk2> inactiveChunks =
      new Dictionary<Vector2Int, Chunk2>();

    //------------------------------------------------------------------------------

    public PrefabInstantiator Instantiator { get => prefabInstantiator; }
    public OcclusionManager2 OcclusionManager { get => occlusionManager; }

    public Dictionary<Vector2Int, Chunk2> ActiveChunks { get => activeChunks; }
    public Dictionary<Vector2Int, Chunk2> LazyChunks { get => lazyChunks; }
    public Dictionary<Vector2Int, Chunk2> InactiveChunks { get => inactiveChunks; }

    private void Awake()
    {
      playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
      prefabInstantiator = GetComponent<PrefabInstantiator>();
      occlusionManager = new OcclusionManager2(playerTransform, this);

      //Singleton pattern
      if (Instance == null) {
        Instance = this;
      } else {
        Destroy(gameObject);
      }
    }

    //Makes sure first chunks are loaded
    private void Start()
    {
      playerLastChunkPosition = GetChunkPosition(playerTransform.position);

      ChunkCreator(playerLastChunkPosition);
      SetActiveChunks(playerChunkPosition);
    }

    private void Update()
    {
      if (playerTransform == null) return;

      occlusionManager.UpdateOcclusion(activeChunks);

      playerChunkPosition = GetChunkPosition(playerTransform.position);

      if (playerChunkPosition == playerLastChunkPosition) return;

      DeactivateActiveChunks();
      ChunkCreator(playerChunkPosition);
      SetActiveChunks(playerChunkPosition);

      playerLastChunkPosition = playerChunkPosition;
    }

    public Vector2Int GetChunkPosition(Vector2 position)
    {
      return new Vector2Int(
        Mathf.RoundToInt(position.x / chunkDiameter),
        Mathf.RoundToInt(position.y / chunkDiameter)
      );
    }

    public Chunk2 GetChunk(Vector2Int chunkKey)
    {
      if (activeChunks.ContainsKey(chunkKey)) {
        return activeChunks[chunkKey];
      } else if (lazyChunks.ContainsKey(chunkKey)) {
        return lazyChunks[chunkKey];
      } else if (inactiveChunks.ContainsKey(chunkKey)) {
        return inactiveChunks[chunkKey];
      } else {
        return null;
      }
    }

    private void DeactivateActiveChunks()
    {
      //Active chunks
      if (activeChunks.Count != 0)
      {
        foreach (var activeChunk in activeChunks)
        {
          activeChunk.Value.AttachedObject.SetActive(false);
          inactiveChunks.Add(activeChunk.Key, activeChunk.Value);
        }

        activeChunks.Clear();
      }

      //Lazy chunks
      if (lazyChunks.Count != 0)
      {
        foreach (var lazyChunk in lazyChunks)
        {
          lazyChunk.Value.AttachedObject.SetActive(false);
          inactiveChunks.Add(lazyChunk.Key, lazyChunk.Value);
        }

        lazyChunks.Clear();
      }
    }

    private void ChunkCreator(Vector2Int chunkCenter)
    {
      Vector2Int lazyGridKey = new Vector2Int(chunkCenter.x -2, chunkCenter.y -2);

      for (int y = 0; y < 5; y++)
      {
        for (int x = 0; x < 5; x++)
        {
          if (inactiveChunks.ContainsKey(lazyGridKey))
          {
            Chunk2 inactiveChunk = inactiveChunks[lazyGridKey];

            inactiveChunks.Remove(lazyGridKey);
            lazyChunks.Add(inactiveChunk.Key, inactiveChunk);
          }
          else
          {
            GameObject newChunk = new GameObject("Chunk " + chunkNumber);
            newChunk.transform.parent = this.transform;
            newChunk.SetActive(false);

            Chunk2 newChunkInfo = new Chunk2(lazyGridKey, chunkDiameter, newChunk);
            chunkNumber++;

            var hasStar = chunkPopulator.PopulateLargeBodies(newChunkInfo);

            if (hasStar) {
              Debug.Log("Star in " + newChunkInfo.Position);
              activeChunks.Add(newChunkInfo.Key, newChunkInfo);
            }
            else {
              lazyChunks.Add(newChunkInfo.Key, newChunkInfo);
            }
          }

          lazyGridKey.x++;
        }

        lazyGridKey.y++;
        lazyGridKey.x -= 5;
      }
    }

    private void SetActiveChunks(Vector2Int chunkCenter)
    {
      Vector2Int activeGridKey = new Vector2Int(chunkCenter.x - 1, chunkCenter.y - 1);

      for (int y = 0; y < 3; y++)
      {
        for (int x = 0; x < 3; x++)
        {
          if (lazyChunks.ContainsKey(activeGridKey))
          {
            Chunk2 lazyChunk = lazyChunks[activeGridKey];
            lazyChunk.AttachedObject.SetActive(true);

            chunkPopulator.PopulateSmallBodies(lazyChunk);

            lazyChunks.Remove(activeGridKey);
            activeChunks.Add(lazyChunk.Key, lazyChunk);
          }
          else
          {
            Debug.LogError("Chunk not found in lazy chunks.");
          }

          activeGridKey.x++;
        }

        activeGridKey.y++;
        activeGridKey.x -= 3;
      }
    }
  }
}