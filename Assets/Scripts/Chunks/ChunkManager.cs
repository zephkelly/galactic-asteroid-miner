using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class ChunkManager : MonoBehaviour
  {
    public static ChunkManager Instance;

    private ChunkPopulator chunkPopulator = new ChunkPopulator();
    private PrefabInstantiator prefabInstantiator;
    private ShipStarCompass shipStarCompass;

    [SerializeField] int chunkDiameter = 100;
    internal int chunkNumber;

    private Transform playerTransform;
    private Vector2Int playerChunkPosition;
    private Vector2Int playerLastChunkPosition;

    private Dictionary<Vector2Int, Chunk> activeChunks =
      new Dictionary<Vector2Int, Chunk>();

    private Dictionary<Vector2Int, Chunk> lazyChunks = 
      new Dictionary<Vector2Int, Chunk>();

    private Dictionary<Vector2Int, Chunk> inactiveChunks =
      new Dictionary<Vector2Int, Chunk>();

    private Dictionary<Vector2Int, Chunk> allChunks =
      new Dictionary<Vector2Int, Chunk>();

    //------------------------------------------------------------------------------

    public PrefabInstantiator Instantiator { get => prefabInstantiator; }

    public Dictionary<Vector2Int, Chunk> AllChunks { get => allChunks; }
    public Dictionary<Vector2Int, Chunk> ActiveChunks { get => activeChunks; }
    public Dictionary<Vector2Int, Chunk> LazyChunks { get => lazyChunks; }
    public Dictionary<Vector2Int, Chunk> InactiveChunks { get => inactiveChunks; }

    public void UpdatePlayerTransform(Transform player)
    {
      playerTransform = player;
      playerChunkPosition = GetChunkPosition(playerTransform.position);
    }

    private void Awake()
    {
      playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
      prefabInstantiator = GetComponent<PrefabInstantiator>();
      shipStarCompass = GetComponent<ShipStarCompass>();

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
      
      OcclusionManager.Instance.UpdateChunks(activeChunks, lazyChunks);
      shipStarCompass.UpdateCompass();
    }

    private void Update()
    {
      if (playerTransform == null) return;

      playerChunkPosition = GetChunkPosition(playerTransform.position);

      if (playerChunkPosition != playerLastChunkPosition)
      {
        DeactivateActiveChunks();
        ChunkCreator(playerChunkPosition);
        SetActiveChunks(playerChunkPosition);

        OcclusionManager.Instance.UpdateChunks(activeChunks, lazyChunks);
        shipStarCompass.UpdateCompass();
      }

      playerLastChunkPosition = playerChunkPosition;
    }

    public Vector2Int GetChunkPosition(Vector2 position)
    {
      return new Vector2Int(
        Mathf.RoundToInt(position.x / chunkDiameter),
        Mathf.RoundToInt(position.y / chunkDiameter)
      );
    }

    public Chunk GetChunk(Vector2Int chunkKey)
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

    //Add 5x5 to lazy chunks
    private void ChunkCreator(Vector2Int chunkCenter)
    {
      Vector2Int lazyGridKey = new Vector2Int(chunkCenter.x -3, chunkCenter.y -3);

      for (int y = 0; y < 7; y++)
      {
        for (int x = 0; x < 7; x++)
        {
          if (inactiveChunks.ContainsKey(lazyGridKey))
          {
            Chunk inactiveChunk = inactiveChunks[lazyGridKey];

            inactiveChunks.Remove(lazyGridKey);
            lazyChunks.Add(inactiveChunk.Key, inactiveChunk);
          }
          else
          {
            GameObject newChunk = new GameObject("Chunk " + chunkNumber);
            newChunk.transform.parent = this.transform;
            newChunk.SetActive(false);

            Chunk newChunkInfo = new Chunk(lazyGridKey, chunkDiameter, newChunk);
            chunkNumber++;

            chunkPopulator.PopulateLargeBodies(newChunkInfo);

            lazyChunks.Add(newChunkInfo.Key, newChunkInfo);
            allChunks.Add(newChunkInfo.Key, newChunkInfo);
          }

          lazyGridKey.x++;
        }

        lazyGridKey.y++;
        lazyGridKey.x -= 7;
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
            Chunk lazyChunk = lazyChunks[activeGridKey];
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