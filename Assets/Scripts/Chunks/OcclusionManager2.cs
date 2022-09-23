using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class OcclusionManager2
  {
    private ChunkManager2 chunkManager;
    private PrefabInstantiator instantiator;
    private Transform playerTransform;

    private const int starOcclusionRadius = 200;   //Should make it the largest star radius + players viewport size
    private const int asteroidOcclusionDistance = 100;

    private Dictionary<Vector2, Asteroid2> activeAsteroids =
      new Dictionary<Vector2, Asteroid2>();

    private Dictionary<Vector2, Asteroid2> lazyAsteroids =
      new Dictionary<Vector2, Asteroid2>();

    private Dictionary<Vector2, Asteroid2> inactiveAsteroids =
      new Dictionary<Vector2, Asteroid2>();

    //------------------------------------------------------------------------------

    public OcclusionManager2(Transform player, ChunkManager2 _chunkManager) {
      chunkManager = _chunkManager;
      instantiator = chunkManager.Instantiator;
      playerTransform = player;
    }

    public void UpdateOcclusion(Dictionary<Vector2Int, Chunk2> activeChunks)
    {
      foreach (var chunk in activeChunks)
      {
        //CheckStarDistances();
        CheckAsteroidOcclusion(chunk.Value, playerTransform.position);
      }

      DisposeInactiveObjects();
    }

    private void CheckAsteroidOcclusion(Chunk2 currentChunk, Vector2 playerPosition)
    {
      foreach (var asteroid in currentChunk.Asteroids)
      {
        var asteroidInfo = asteroid.Value;
        var asteroidSpawnPoint = asteroidInfo.SpawnPoint;

        var distance = Vector2.Distance(playerPosition, asteroidSpawnPoint);
        //var distance = FastDistance(asteroidSpawnPoint, playerPosition);

        //Check if in active asteroids
        if (activeAsteroids.ContainsKey(asteroidSpawnPoint))
        {
          //Check if in occlusion radius
          if (distance > asteroidOcclusionDistance)
          {
            activeAsteroids.Remove(asteroidSpawnPoint);
            inactiveAsteroids.Add(asteroidSpawnPoint, asteroidInfo);
            continue;
          }
        }
        else if (distance < asteroidOcclusionDistance)
        {
          asteroidInfo.SetObject(
            instantiator.GetAsteroid(asteroidInfo.Size),
            asteroidSpawnPoint
          );

          activeAsteroids.Add(asteroidSpawnPoint, asteroidInfo);
        }
      }
    }

    private void DisposeInactiveObjects()
    {
      if (inactiveAsteroids.Count == 0) return;

      foreach (var asteroid in inactiveAsteroids)
      {
        var asteroidInfo = asteroid.Value;

        asteroidInfo.DisposeObject();
        Object.Destroy(asteroidInfo.AttachedObject); //Pool instead
      }

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