using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class OcclusionManager
  {
    private ChunkManager chunkManager;
    private PrefabInstantiator instantiator;
    private Transform playerTransform;

    private const int starOcclusionRadius = 200;   //Should make it the largest star radius + players viewport size
    private const int asteroidOcclusionDistance = 100;

    //Asteroids
    private Dictionary<Vector2, Asteroid> activeAsteroids =
      new Dictionary<Vector2, Asteroid>();

    private Dictionary<Vector2, Asteroid> inactiveAsteroids =
      new Dictionary<Vector2, Asteroid>();

    //Stars
    private Dictionary<Vector2, Star> activeStars =
      new Dictionary<Vector2, Star>();

    private Dictionary<Vector2, Star> inactiveStars =
      new Dictionary<Vector2, Star>();

    private Dictionary<Vector2, Asteroid> starAsteroids =
      new Dictionary<Vector2, Asteroid>();

    //------------------------------------------------------------------------------

    public Dictionary<Vector2, Star> ActiveStars { get => activeStars; }

    public OcclusionManager(Transform player, ChunkManager _chunkManager) {
      chunkManager = _chunkManager;
      instantiator = chunkManager.Instantiator;
      playerTransform = player;
    }

    public void UpdateOcclusion(Dictionary<Vector2Int, Chunk> activeChunks,
      Dictionary<Vector2Int, Chunk> lazyChunks)
    {
      foreach (var activeChunk in activeChunks)
      {
        if (activeChunk.Value.HasStar)
        {
          CheckStarOcclusion(activeChunk.Value, playerTransform.position);
        }
        else
        {
          CheckAsteroidOcclusion(activeChunk.Value, playerTransform.position);
        }
      }

      foreach (var lazyChunk in lazyChunks)
      {
        if (lazyChunk.Value.HasStar == false) continue;
        CheckStarOcclusion(lazyChunk.Value, playerTransform.position);
      }

      DisposeInactiveObjects();
    }

    private void CheckAsteroidOcclusion(Chunk currentChunk, Vector2 playerPosition)
    {
      foreach (var asteroid in currentChunk.Asteroids)
      {
        var asteroidInfo = asteroid.Value;
        var asteroidSpawnPoint = asteroidInfo.SpawnPoint;

        //var distance = Vector2.Distance(playerPosition, asteroidSpawnPoint);
        var distance = FastDistance(asteroidSpawnPoint, playerPosition);

        //Occlusion check
        if (activeAsteroids.ContainsKey(asteroidSpawnPoint))
        {
          if (distance > asteroidOcclusionDistance)
          {
            activeAsteroids.Remove(asteroidSpawnPoint);
            inactiveAsteroids.Add(asteroidSpawnPoint, asteroidInfo);
            continue;
          }
        }
        else if (starAsteroids.ContainsKey(asteroidSpawnPoint))
        {
          if (distance > asteroidOcclusionDistance)
          {
            if (asteroidInfo.RendererStatus == false) return;
            asteroidInfo.IsRendered(false);
          }
          else
          {
            if (asteroidInfo.RendererStatus == true) return;
            asteroidInfo.IsRendered(true);
          }

          continue;
        }
        else if (distance < asteroidOcclusionDistance)   //Should we spawn asteroid?
        {
          if (asteroidInfo.AttachedObject == null)
          {
            var newAsteroid = instantiator.GetAsteroid(asteroidInfo);

            asteroidInfo.SetObject(
              newAsteroid,
              asteroidSpawnPoint
            );

            newAsteroid.GetComponent<AsteroidController>().SetAsteroidInfo(asteroidInfo);   
          }

          activeAsteroids.Add(asteroidSpawnPoint, asteroidInfo);
          continue;
        }
      }
    }

    private void CheckStarOcclusion(Chunk currentChunk, Vector2 playerPosition)
    {
      StarOcclusion();
      CheckStarAsteroids();

      void StarOcclusion()
      {
        var currentStar = currentChunk.ChunkStar;
        var starSpawn = currentStar.SpawnPoint;
        var starDistance = FastDistance(starSpawn, playerPosition);

        //Check the distance to the star
        if (activeStars.ContainsKey(starSpawn))
        {
          if (starDistance < starOcclusionRadius) return;

          activeStars.Remove(starSpawn);
          inactiveStars.Add(starSpawn, currentStar);
          return;
        }
        else if (starDistance < starOcclusionRadius)
        {
          currentStar.SetStarObject(instantiator.GetStar(currentStar));
          activeStars.Add(starSpawn, currentStar);

          foreach (var asteroid in currentChunk.Asteroids)
          {
            var asteroidInfo = asteroid.Value;
            var asteroidSpawnPoint = asteroidInfo.SpawnPoint;

            if (starAsteroids.ContainsKey(asteroidSpawnPoint)) 
            {
              continue;
            }
            else if (activeAsteroids.ContainsKey(asteroidSpawnPoint))
            {
              activeAsteroids.Remove(asteroidSpawnPoint);
              starAsteroids.Add(asteroidSpawnPoint, asteroidInfo);
            }
            else
            {
              var starAsteroid = instantiator.GetAsteroid(asteroidInfo);

              asteroidInfo.SetObject(
                starAsteroid,
                asteroidSpawnPoint
              );

              starAsteroid.GetComponent<AsteroidController>().SetAsteroidInfo(asteroidInfo);

              starAsteroids.Add(asteroidSpawnPoint, asteroidInfo);
            }
          }
        }
      }

      void CheckStarAsteroids()
      {
        foreach (var asteroid in currentChunk.Asteroids)
        {
          var asteroidInfo = asteroid.Value;
          var asteroidSpawnPoint = asteroidInfo.Position;

          var asteroidDistance = FastDistance(asteroidInfo.Position, playerPosition);

          if (starAsteroids.ContainsKey(asteroidSpawnPoint))
          {
            asteroidInfo.UpdateSpawnPoint();
          }
        }
      }
    }

    private void DisposeInactiveObjects()
    {
      foreach (var star in inactiveStars)
      {
        var starInfo = star.Value;

        foreach (var asteroid in starInfo.ParentChunk.Asteroids)
        {
          var asteroidInfo = asteroid.Value;

          if (starAsteroids.ContainsKey(asteroidInfo.SpawnPoint))
          {
            starAsteroids.Remove(asteroidInfo.SpawnPoint);
            inactiveAsteroids.Add(asteroidInfo.SpawnPoint, asteroidInfo);
          }
        }

        Object.Destroy(starInfo.AttachedObject); //Pool instead

        starInfo.DisposeObject();
      }

      foreach (var asteroid in inactiveAsteroids)
      {
        var asteroidInfo = asteroid.Value;

        Object.Destroy(asteroidInfo.AttachedObject); //Pool instead

        asteroidInfo.DisposeObject();
      }

      inactiveStars.Clear();
      inactiveAsteroids.Clear();
    }

    private float FastDistance(Vector2 _point1, Vector2 _point2)
    {
      var x = _point1.x - _point2.x;
      var y = _point1.y - _point2.y;

      return Mathf.Sqrt(x * x + y * y);
    }
  }
}