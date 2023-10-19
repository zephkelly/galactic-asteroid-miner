using System;
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
    private const int starOcclusionRadius = 300;
    private const int asteroidOcclusionDistance = 90;
    private const int starOcclusionRadiusSqr = starOcclusionRadius * starOcclusionRadius;
    private const int asteroidOcclusionDistanceSqr = asteroidOcclusionDistance * asteroidOcclusionDistance;

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

      UpdateChunkContents();
      ActiveDepoOcclusion();
      ActiveStarOcclusion();
      LazyAsteroidOcclusion();
      ActiveAsteroidOcclusion();

      Debug.Log("Active: " + activeAsteroids.Count);
      Debug.Log("Lazy: " + lazyAsteroids.Count);
    }

    private void UpdateDictionary<TKey>(
      Dictionary<TKey, Chunk> activeDict, 
      Dictionary<Vector2Int, Chunk> chunks, 
      Func<Chunk, bool> hasKeyPredicate, 
      Func<Chunk, TKey> keySelector)
    { 
      foreach (var chunk in chunks)
      {
        if (!hasKeyPredicate(chunk.Value)) continue;
        var key = keySelector(chunk.Value);
        if (!activeDict.ContainsKey(key))
          activeDict.Add(key, chunk.Value);
      }
    }

    private void GetActiveDepos()
    {
      UpdateDictionary(activeDepos, currentLazyChunks, 
        chunk => chunk.HasDepo, chunk => chunk.ChunkDepo);
      UpdateDictionary(activeDepos, currentActiveChunks, 
        chunk => chunk.HasDepo, chunk => chunk.ChunkDepo);
    }

    private void GetActiveStars()
    {
      UpdateDictionary(activeStars, currentLazyChunks, 
        chunk => chunk.HasStar, chunk => chunk.ChunkStar);
      UpdateDictionary(activeStars, currentActiveChunks, 
        chunk => chunk.HasStar, chunk => chunk.ChunkStar);
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
      foreach (var activeChunk in currentActiveChunks)
      {
        // if (activeChunk.Value.HasStar) {
          // Vector2Int gridKey = new Vector2Int(activeChunk.Key.x -1, activeChunk.Key.y -1);

          // for (int y = 0; y < 3; y++)
          // {
          //   for (int x = 0; x < 3; x++)
          //   {
          //     if (currentLazyChunks.ContainsKey(gridKey))
          //     {
          //       if (currentLazyChunks[gridKey].Asteroids.Count == 0) continue;

          //       foreach (var lazyAsteroid in currentLazyChunks[gridKey].Asteroids)
          //       {
          //         if (activeAsteroids.ContainsKey(lazyAsteroid)) continue;
          //         activeAsteroids.Add(lazyAsteroid, currentLazyChunks[gridKey]);
          //       }
          //     }

          //     if (currentActiveChunks.ContainsKey(gridKey))
          //     {
          //       if (currentActiveChunks[gridKey].Asteroids.Count == 0) continue;

          //       foreach (var activeAsteroid in currentActiveChunks[gridKey].Asteroids)
          //       {
          //         if (activeAsteroids.ContainsKey(activeAsteroid)) continue;
          //         activeAsteroids.Add(activeAsteroid, currentActiveChunks[gridKey]);
          //       }
          //     }

          //     gridKey.x++;
          //   }

          //   gridKey.x -= 3;
          //   gridKey.y++;
          // }
    
          // continue;
        // }

        if (activeChunk.Value.Asteroids.Count == 0) continue;

        foreach (var activeAsteroid in activeChunk.Value.Asteroids)
        {
          // if (lazyAsteroids.ContainsKey(activeAsteroid))
          // {
          //   lazyAsteroids.Remove(activeAsteroid);
          //   continue;
          // }

          if (activeAsteroids.ContainsKey(activeAsteroid)) continue;
          activeAsteroids.Add(activeAsteroid, activeChunk.Value);
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

        if (starDistance < starOcclusionRadiusSqr)
        { 
          if (activeStar.Key.AttachedObject != null) continue;

          var starObject = instantiator.GetStar(activeStar.Key);
          activeStar.Key.SetStarObject(starObject, starObject.GetComponent<SpriteRenderer>());

          starObject.GetComponent<StarController>().SetStarInfo(activeStar.Key);

          activeStar.Key.AttachedObject.SetActive(true);
        }
        else
        {
          if (activeStar.Key.AttachedObject == null) continue;

          activeStar.Key.DisposeObject();

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
        if (lazyAsteroid.Key.AttachedObject == null)
        {
          var asteroidObject = instantiator.GetAsteroid(lazyAsteroid.Key);
          lazyAsteroid.Key.SetObject(asteroidObject);
          lazyAsteroid.Key.IsRendered(false);
        }
        else {
          lazyAsteroid.Key.UpdateCurrentPosition();
        }

        var asteroidDistance = FastDistance(lazyAsteroid.Key.CurrentPosition, playerTransform.position);

        if (asteroidDistance < asteroidOcclusionDistanceSqr)
        {
          if (lazyAsteroid.Key.RendererStatus) continue;
          lazyAsteroid.Key.IsRendered(true);
          Debug.LogWarning("Star asteroid displayed!");
        }
        else
        {
          if (lazyAsteroid.Key.RendererStatus == false) continue;
          lazyAsteroid.Key.IsRendered(false);
          Debug.LogWarning("Star asteroid hidden!");
        }
      }
    }

    private void ActiveAsteroidOcclusion()
    {
      List<Asteroid> disposableAsteroids = new List<Asteroid>();

      foreach (var activeAsteroid in activeAsteroids)
      {
        var asteroidDistance = FastDistance(activeAsteroid.Key.CurrentPosition, playerTransform.position);

        if (asteroidDistance < asteroidOcclusionDistanceSqr)
        {
          //Make object if null
          if (activeAsteroid.Key.AttachedObject == null)
          {
            var asteroidObject = instantiator.GetAsteroid(activeAsteroid.Key);
            activeAsteroid.Key.SetObject(asteroidObject);
          } 
          else {
            activeAsteroid.Key.IsRendered(true);
            activeAsteroid.Key.UpdateCurrentPosition();    
          }
        }
        else
        {
          disposableAsteroids.Add(activeAsteroid.Key);

          if (activeAsteroid.Key.AttachedObject == null) continue;

          activeAsteroid.Key.UpdateCurrentPosition();
          instantiator.ReturnAsteroid(activeAsteroid.Key);
          activeAsteroid.Key.NullObject();
        }
      }

      foreach (var asteroid in disposableAsteroids)
      {
        activeAsteroids.Remove(asteroid);
      }
    }

    //This returns the fast distance squared between two points, need to compare against the distance squared
    private float FastDistance(Vector2 _point1, Vector2 _point2)
    {
      var x = _point1.x - _point2.x;
      var y = _point1.y - _point2.y;

      return x * x + y * y;
    }
  }
}