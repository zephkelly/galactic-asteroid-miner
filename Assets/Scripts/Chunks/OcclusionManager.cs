using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class OcclusionManager : MonoBehaviour
  {
    public static OcclusionManager Instance;
    private ChunkManager chunkManager;
    private PrefabInstantiator instantiator;

    private Transform playerTransform;
    private const int starOcclusionRadius = 650;   //Should make it the largest star radius + players viewport size
    private const int asteroidOcclusionDistance = 60;

    Dictionary<Vector2Int, Chunk> currentActiveChunks = new Dictionary<Vector2Int, Chunk>();
    Dictionary<Vector2Int, Chunk> currentLazyChunks = new Dictionary<Vector2Int, Chunk>();

    private Dictionary<Asteroid, Chunk> activeAsteroids = new Dictionary<Asteroid, Chunk>();
    private Dictionary<Asteroid, Chunk> lazyAsteroids = new Dictionary<Asteroid, Chunk>();
    private Dictionary<Asteroid, Chunk> inactiveAsteroids = new Dictionary<Asteroid, Chunk>();

    private Dictionary<Asteroid, Chunk> asteroidToChangeChunk = new Dictionary<Asteroid, Chunk>();
    private Dictionary<Asteroid, Chunk> asteoridToRemove = new Dictionary<Asteroid, Chunk>();
    private Dictionary<Asteroid, Chunk> asteroidToAdd = new Dictionary<Asteroid, Chunk>();

    private Dictionary<Star, Chunk> activeStars = new Dictionary<Star, Chunk>();
    private Dictionary<Star, Chunk> inactiveStars = new Dictionary<Star, Chunk>();

    //------------------------------------------------------------------------------

    public Dictionary<Asteroid, Chunk> ChangeAsteroidChunk { get => activeAsteroids; set => activeAsteroids = value; }
    public Dictionary<Asteroid, Chunk> RemoveAsteroid { get => asteoridToRemove; set => asteoridToRemove = value; }
    public Dictionary<Asteroid, Chunk> AddAsteroid { get => asteroidToAdd; set => asteroidToAdd = value; }

    private void Awake()
    {
      if (Instance == null) {
        Instance = this;
      } else {
        Destroy(gameObject);
      }
    }

    private void Start()
    {
      chunkManager = ChunkManager.Instance;
      instantiator = ChunkManager.Instance.Instantiator;
      playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void UpdateChunks(Dictionary<Vector2Int, Chunk> activeChunks, Dictionary<Vector2Int, Chunk> lazyChunks)
    {
      currentActiveChunks = activeChunks;
      currentLazyChunks = lazyChunks;

      activeStars.Clear();
      lazyAsteroids.Clear();

      GetActiveStars();
      GetLazyAsteroids();
    }

    private void GetActiveStars()
    {
      foreach (var lazyChunk in currentLazyChunks)
      {
        if (!lazyChunk.Value.HasStar) continue;
        if (activeStars.ContainsKey(lazyChunk.Value.ChunkStar)) continue;
        activeStars.Add(lazyChunk.Value.ChunkStar, lazyChunk.Value);
      }

      foreach (var activeChunk in currentActiveChunks)
      {
        if (!activeChunk.Value.HasStar) continue;
        if (activeStars.ContainsKey(activeChunk.Value.ChunkStar)) continue;
        activeStars.Add(activeChunk.Value.ChunkStar, activeChunk.Value);
      }
    }

    private void GetLazyAsteroids()
    {
      if (activeStars.Count == 0) return;

      foreach (var activeStar in activeStars)
      {
        var starAsteroids = activeStar.Value.Asteroids;

        foreach (var asteroid in starAsteroids)
        {
          var asteroidDistance = FastDistance(asteroid.SpawnPoint, playerTransform.position);

          lazyAsteroids.Add(asteroid, activeStar.Value);
        }
      }
    }

    private void Update()
    {
      UpdateChunkContents();

      ActiveStarOcclusion();
      LazyAsteroidOcclusion();
    }

    private void UpdateChunkContents()
    {
      ChangeChunks();
      RemoveFromChunk();
      AddToChunk();

      void ChangeChunks()
      {
        if (asteroidToChangeChunk.Count == 0) return;
        foreach (var asteroid in asteroidToChangeChunk)
        {
          if (asteroid.Key == null) continue;
          asteroid.Key.ParentChunk.Asteroids.Remove(asteroid.Key);

          asteroid.Key.NewParentChunk(asteroid.Value);
          asteroid.Value.Asteroids.Add(asteroid.Key);
        }

        asteroidToChangeChunk.Clear();
      }

      void RemoveFromChunk()
      {
        if (asteoridToRemove.Count == 0) return;
        foreach (var asteroid in asteoridToRemove)
        {
          if (asteroid.Key == null) continue;
          asteroid.Value.Asteroids.Remove(asteroid.Key);

          Destroy(asteroid.Key.AttachedObject);

          if (lazyAsteroids.ContainsKey(asteroid.Key)) lazyAsteroids.Remove(asteroid.Key);
          if (activeAsteroids.ContainsKey(asteroid.Key)) activeAsteroids.Remove(asteroid.Key);
        }

        asteoridToRemove.Clear();
      }

      void AddToChunk()
      {
        if (asteroidToAdd.Count == 0) return;
        foreach (var asteroid in asteroidToAdd)
        {
          if (asteroid.Key == null) continue;
          if (asteroid.Key.ParentChunk == asteroid.Value) continue;
          asteroid.Key.ParentChunk.Asteroids.Remove(asteroid.Key);

          asteroid.Key.NewParentChunk(asteroid.Value);
          asteroid.Value.Asteroids.Add(asteroid.Key);
        }

        asteroidToAdd.Clear();
      }
    }

    private void ActiveStarOcclusion()
    {
      foreach (var activeStar in activeStars)
      {
        var starDistance = FastDistance(activeStar.Key.SpawnPoint, playerTransform.position);

        //Make object if null
        if (activeStar.Key.AttachedObject == null)
        {
          var starObject = instantiator.GetStar(activeStar.Key);
          starObject.GetComponent<StarController>().SetStarInfo(activeStar.Key);
          starObject.SetActive(false);
        }

        if (starDistance < starOcclusionRadius)
        {
          if (activeStar.Key.AttachedObject.activeSelf) continue;
          activeStar.Key.AttachedObject.SetActive(true);
        }
        else
        {
          //Dispose of object send back to pool
          if (!activeStar.Key.AttachedObject.activeSelf) continue;
          activeStar.Key.AttachedObject.SetActive(false);
        }
      }
    }

    private void LazyAsteroidOcclusion()
    {
      foreach (var lazyAsteroid in lazyAsteroids)
      {
        var asteroidDistance = FastDistance(lazyAsteroid.Key.CurrentPosition, playerTransform.position);

        //Make object if null
        if (lazyAsteroid.Key.AttachedObject == null)
        {
          var asteroidObject = instantiator.GetAsteroid(lazyAsteroid.Key);
          lazyAsteroid.Key.SetObject(asteroidObject);
          lazyAsteroid.Key.IsRendered(false);
        }
        else
        {
          lazyAsteroid.Key.UpdateCurrentPosition();
        }

        if (asteroidDistance < asteroidOcclusionDistance)
        {
          if (lazyAsteroid.Key.RendererStatus) continue;
          lazyAsteroid.Key.IsRendered(true);
        }
        else
        {
          //Dispose of object send back to pool
          if (!lazyAsteroid.Key.RendererStatus) continue;
          lazyAsteroid.Key.IsRendered(false);
        }
      }
    }

    private float FastDistance(Vector2 _point1, Vector2 _point2)
    {
      var x = _point1.x - _point2.x;
      var y = _point1.y - _point2.y;

      return Mathf.Sqrt(x * x + y * y);
    }
  } 
}