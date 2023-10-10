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
    private const int starOcclusionRadius = 350;   //Should make it the largest star radius + players viewport size
    private const int asteroidOcclusionDistance = 80;

    Dictionary<Vector2Int, Chunk> currentActiveChunks = new Dictionary<Vector2Int, Chunk>();
    Dictionary<Vector2Int, Chunk> currentLazyChunks = new Dictionary<Vector2Int, Chunk>();

    private Dictionary<Asteroid, Chunk> activeAsteroids = new Dictionary<Asteroid, Chunk>();
    private Dictionary<Asteroid, Chunk> lazyAsteroids = new Dictionary<Asteroid, Chunk>();
    //private Dictionary<Asteroid, Chunk> inactiveAsteroids = new Dictionary<Asteroid, Chunk>();

    private Dictionary<Asteroid, Chunk> asteroidToChangeChunk = new Dictionary<Asteroid, Chunk>();
    private Dictionary<Asteroid, Chunk> asteoridToRemove = new Dictionary<Asteroid, Chunk>();
    private Dictionary<Asteroid, Chunk> asteroidToAdd = new Dictionary<Asteroid, Chunk>();

    private Dictionary<Star, Chunk> activeStars = new Dictionary<Star, Chunk>();
    //private Dictionary<Star, Chunk> inactiveStars = new Dictionary<Star, Chunk>();

    private Dictionary<Depo, Chunk> activeDepos = new Dictionary<Depo, Chunk>();

    //------------------------------------------------------------------------------

    public Dictionary<Asteroid, Chunk> ChangeAsteroidChunk { get => activeAsteroids; set => activeAsteroids = value; }
    public Dictionary<Asteroid, Chunk> RemoveAsteroid { get => asteoridToRemove; set => asteoridToRemove = value; }
    public Dictionary<Asteroid, Chunk> AddAsteroid { get => asteroidToAdd; set => asteroidToAdd = value; }

    private void Awake()
    {
      if (Instance == null) {
        Instance = this;
      } else {
        Destroy(this);
      }
    }

    public void UpdatePlayerTransform(Transform newPlayer) => playerTransform = newPlayer;

    private void Start()
    {
      chunkManager = ChunkManager.Instance;
      instantiator = ChunkManager.Instance.Instantiator;
      playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
      if (playerTransform == null) return;

      UpdateChunkContents();
      ActiveDepoOcclusion();
      ActiveStarOcclusion();
      LazyAsteroidOcclusion();
      ActiveAsteroidOcclusion();
    }

    public void UpdateChunks(Dictionary<Vector2Int, Chunk> activeChunks, Dictionary<Vector2Int, Chunk> lazyChunks)
    {
      currentActiveChunks = activeChunks;
      currentLazyChunks = lazyChunks;

      activeStars.Clear();
      lazyAsteroids.Clear();

      GetActiveStars();
      GetActiveDepos();
      GetLazyAsteroids();
      GetActiveAsteroids();
    }

    private void GetActiveDepos()
    {
      foreach (var lazyChunk in currentLazyChunks)
      {
        if (!lazyChunk.Value.HasDepo) continue;
        if (activeDepos.ContainsKey(lazyChunk.Value.ChunkDepo)) continue;
        activeDepos.Add(lazyChunk.Value.ChunkDepo, lazyChunk.Value);
      }

      foreach (var activeChunk in currentActiveChunks)
      {
        if (!activeChunk.Value.HasDepo) continue;
        if (activeDepos.ContainsKey(activeChunk.Value.ChunkDepo)) continue;
        activeDepos.Add(activeChunk.Value.ChunkDepo, activeChunk.Value);
      }
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
        if (activeStar.Value.Asteroids.Count == 0) continue;

        foreach (var asteroid in activeStar.Value.Asteroids)
        {
          if (lazyAsteroids.ContainsKey(asteroid)) continue;
          lazyAsteroids.Add(asteroid, activeStar.Value);
        }
      }
    }

    private void GetActiveAsteroids()
    {
      List<Chunk> starChunks = new List<Chunk>();

      foreach (var activeChunk in currentActiveChunks)
      {
        if (activeChunk.Value.HasStar) {
          starChunks.Add(activeChunk.Value);
          continue;
        }
        if (activeChunk.Value.Asteroids.Count == 0) continue;

        foreach (var activeAsteroid in activeChunk.Value.Asteroids)
        {
          if (activeAsteroids.ContainsKey(activeAsteroid)) continue;
          activeAsteroids.Add(activeAsteroid, activeChunk.Value);
        }
      }

      foreach (var starChunk in starChunks)
      {
        Vector2Int gridKey = new Vector2Int(starChunk.Key.x -5, starChunk.Key.y -5);

        for (int y = 0; y < 5; y++)
        {
          for (int x = 0; x < 5; x++)
          {
            if (currentLazyChunks.ContainsKey(gridKey))
            {
              if (currentLazyChunks[gridKey].Asteroids.Count == 0) continue;

              foreach (var lazyAsteroid in currentLazyChunks[gridKey].Asteroids)
              {
                if (activeAsteroids.ContainsKey(lazyAsteroid)) continue;
                activeAsteroids.Add(lazyAsteroid, currentLazyChunks[gridKey]);
              }
            }

            if (currentActiveChunks.ContainsKey(gridKey))
            {
              if (currentActiveChunks[gridKey].Asteroids.Count == 0) continue;

              foreach (var activeAsteroid in currentActiveChunks[gridKey].Asteroids)
              {
                if (activeAsteroids.ContainsKey(activeAsteroid)) continue;
                activeAsteroids.Add(activeAsteroid, currentActiveChunks[gridKey]);
              }
            }

            gridKey.x++;
          }

          gridKey.x -= 5;
          gridKey.y++;
        }
      }
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
          asteroid.Key.UpdateCurrentPosition();
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
          if (asteroid.Key.AttachedObject != null) asteroid.Key.UpdateCurrentPosition();
          asteroid.Value.Asteroids.Remove(asteroid.Key);

          chunkManager.Instantiator.ReturnAsteroid(asteroid.Key);
          asteroid.Key.NullObject();

          if (activeAsteroids.ContainsKey(asteroid.Key)) activeAsteroids.Remove(asteroid.Key);
          if (lazyAsteroids.ContainsKey(asteroid.Key)) lazyAsteroids.Remove(asteroid.Key);

          GetActiveAsteroids();
          GetLazyAsteroids();
        }

        asteoridToRemove.Clear();
      }

      void AddToChunk()
      {
        foreach (var asteroid in asteroidToAdd)
        {
          if (asteroid.Key == null) continue;
          asteroid.Key.UpdateCurrentPosition();
          asteroid.Value.Asteroids.Add(asteroid.Key);

          GetActiveAsteroids();
          GetLazyAsteroids();
        }

        asteroidToAdd.Clear();
      }
    }

    private void ActiveDepoOcclusion() 
    {
      foreach (var activeDepo in activeDepos)
      {
        if (activeDepo.Key.AttachedObject == null)
        {
          var depoObject = instantiator.GetDepo(activeDepo.Key);
          activeDepo.Key.SetDepoObject(depoObject);
        }
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
        }

        if (starDistance < starOcclusionRadius)
        {
          if (activeStar.Key.AttachedObject.activeSelf) continue;
          activeStar.Key.AttachedObject.SetActive(true);
        }
        else
        {
          Destroy(activeStar.Key.AttachedObject);

          foreach (var asteroid in activeStar.Value.Asteroids)
          {
            if (lazyAsteroids.ContainsKey(asteroid))
            {
              lazyAsteroids.Remove(asteroid);

              if (asteroid.AttachedObject == null) continue;

              //Send back to pool
              asteroid.UpdateCurrentPosition();
              chunkManager.Instantiator.ReturnAsteroid(asteroid);

              asteroid.NullObject();
            }
          }
        }
      }
    }

    private void LazyAsteroidOcclusion()
    {
      foreach (var lazyAsteroid in lazyAsteroids)
      {
        //Make object if null
        if (lazyAsteroid.Key.AttachedObject == null )
        {
          var asteroidObject = instantiator.GetAsteroid(lazyAsteroid.Key);
          lazyAsteroid.Key.SetObject(asteroidObject);
        }
        else {
          lazyAsteroid.Key.UpdateCurrentPosition();
        }

        var asteroidDistance = FastDistance(lazyAsteroid.Key.CurrentPosition, playerTransform.position);

        if (asteroidDistance < asteroidOcclusionDistance)
        {
          if (lazyAsteroid.Key.AttachedObject == null) continue;
          if (lazyAsteroid.Key.RendererStatus) continue;
          lazyAsteroid.Key.IsRendered(true);
          lazyAsteroid.Key.UpdateCurrentPosition();
        }
        else
        {
          if (lazyAsteroid.Key.AttachedObject == null) continue;
          if (!lazyAsteroid.Key.RendererStatus) continue;
          lazyAsteroid.Key.IsRendered(false);
          lazyAsteroid.Key.UpdateCurrentPosition();
        }
      }
    }

    private void ActiveAsteroidOcclusion()
    {
      foreach (var activeAsteroid in activeAsteroids)
      {
        var asteroidDistance = FastDistance(activeAsteroid.Key.CurrentPosition, playerTransform.position);

        if (asteroidDistance < asteroidOcclusionDistance)
        {
          //Make object if null
          if (activeAsteroid.Key.AttachedObject == null)
          {
            var asteroidObject = instantiator.GetAsteroid(activeAsteroid.Key);
            activeAsteroid.Key.SetObject(asteroidObject);
          } 
          else {
            activeAsteroid.Key.UpdateCurrentPosition();
          }
        }
        else
        {
          //Dispose of object and update spawn point
          if (activeAsteroid.Key.AttachedObject == null) continue;

          activeAsteroid.Key.UpdateCurrentPosition();
          instantiator.ReturnAsteroid(activeAsteroid.Key);

          activeAsteroid.Key.NullObject();
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