using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class OcclusionManager : MonoBehaviour
  {
    private ChunkManager chunkManager;
    private ChunkPopulator chunkPopulator;

    private Transform playerTransform;

    [SerializeField] GameObject asteroidPickupPrefab;
    [SerializeField] GameObject asteroidSmallPrefab;
    [SerializeField] GameObject asteroidMediumPrefab;
    [SerializeField] GameObject asteroidLargePrefab;
    [SerializeField] GameObject asteroidExtraLPrefab;

    [SerializeField] int occlusionDistance = 50;
    [SerializeField] float destructionTimer = 10f;

    //------------------------------------------------------------------------------

    private Dictionary<Vector2, Asteroid> activeAsteroids = 
      new Dictionary<Vector2, Asteroid>();

    private Dictionary<Vector2, Asteroid> lazyAsteroids = 
      new Dictionary<Vector2, Asteroid>();

    private Dictionary<Vector2, float> deactivationTimer =
      new Dictionary<Vector2, float>();

    private Dictionary<Vector2, Asteroid> deactivatedAsteroids =
      new Dictionary<Vector2, Asteroid>();

    private void Start()
    {
      chunkManager = ChunkManager.Instance;
      playerTransform = chunkManager.PlayerTransform;
    }

    private void Update()
    {
      foreach (var chunk in chunkManager.ActiveChunks)
      {
        CheckAsteroidOcclusion(chunk.Value, playerTransform.position);
      }

      if (deactivatedAsteroids.Count == 0) return;
      foreach (var asteroid in deactivatedAsteroids)
      {
        var asteroidInfo = asteroid.Value;

        asteroidInfo.RemoveParentChunk();
        chunkManager.AddAsteroidToChunk(asteroidInfo);

        Destroy(asteroid.Value.AsteroidObject); //Pool
      }

      deactivatedAsteroids.Clear();
    }

    //Checking occlusion of all asteroids in chunk
    public void CheckAsteroidOcclusion(Chunk currentChunk, Vector2 playerPosition)
    {
      foreach (var asteroid in currentChunk.Asteroids)
      {
        Asteroid asteroidInfo = asteroid.Value;
        Vector2 currentSpawn = asteroidInfo.SpawnPosition;
        Vector2 currentPosition = asteroidInfo.CurrentPosition;

        //Check if active
        if (activeAsteroids.ContainsKey(asteroid.Key))
        {
          if (Vector2.Distance(playerPosition, currentPosition) > occlusionDistance)
          {
            if(asteroidInfo.AsteroidObject == null)
            {
              Debug.LogError("Asteroid object is null");
              currentChunk.Asteroids.Remove(asteroid.Key);
              return;
            }
            
            var lazyKey = asteroidInfo.SetLazyKey();
            asteroidInfo.AsteroidObject.SetActive(false);
            //Destroy(asteroidInfo.AsteroidObject); //Pool instead

            activeAsteroids.Remove(asteroid.Key);
            lazyAsteroids.Add(lazyKey, asteroidInfo);
            deactivationTimer.Add(lazyKey, destructionTimer);
            return;
          }
        }

        //Check if lazy
        else if (lazyAsteroids.ContainsKey(asteroidInfo.LazyKey))
        {
          if (Vector2.Distance(playerPosition, currentPosition) < occlusionDistance)
          {
            asteroidInfo.AsteroidObject.SetActive(true);

            asteroidInfo.SetNewSpawn(asteroidInfo.CurrentPosition);
            activeAsteroids.Add(asteroidInfo.SpawnPosition, asteroidInfo);

            lazyAsteroids.Remove(asteroidInfo.LazyKey);
            deactivationTimer.Remove(asteroidInfo.LazyKey);
          }
          else
          {
            deactivationTimer[asteroidInfo.LazyKey] -= Time.deltaTime;

            if (deactivationTimer[asteroidInfo.LazyKey] <= 0)
            {
              Destroy(asteroidInfo.AsteroidObject); //Pool instead

              deactivatedAsteroids.Add(asteroidInfo.LazyKey, asteroidInfo);

              lazyAsteroids.Remove(asteroidInfo.LazyKey);
              deactivationTimer.Remove(asteroidInfo.LazyKey);
            }
          }
        }

        //New asteroid
        else if (Vector2.Distance(playerPosition, asteroid.Value.SpawnPosition) < occlusionDistance)
        {
          switch (asteroidInfo.Size)
          {
            case AsteroidSize.Pickup:
              var pickupAsteroid = Instantiate(asteroidPickupPrefab);
              SetAsteroid(pickupAsteroid);
              break;

            case AsteroidSize.Small:
              var smallAsteroid = Instantiate(asteroidSmallPrefab);
              SetAsteroid(smallAsteroid);
              break;

            case AsteroidSize.Medium:
              var mediumAsteroid = Instantiate(asteroidMediumPrefab);
              SetAsteroid(mediumAsteroid);
              break;

            case AsteroidSize.Large:
              var largeAsteroid = Instantiate(asteroidLargePrefab);
              SetAsteroid(largeAsteroid);
              break;

            case AsteroidSize.ExtraLarge:
              var extraLargeAsteroid = Instantiate(asteroidExtraLPrefab);
              SetAsteroid(extraLargeAsteroid);
              break;
          }

          void SetAsteroid(GameObject newAsteroid)
          {
            newAsteroid.GetComponent<AsteroidController>()
              .SetAsteroid(asteroidInfo, currentChunk);
            newAsteroid.transform.parent = currentChunk.ChunkTransform;
            newAsteroid.transform.position = asteroid.Key;

            activeAsteroids.Add(asteroid.Key, asteroid.Value);
          }
        }
      }
    }

    public void RemoveAsteroid(Asteroid asteroid)
    {
      if (activeAsteroids.ContainsKey(asteroid.SpawnPosition))
      {
        activeAsteroids.Remove(asteroid.SpawnPosition);
      }
      else if (lazyAsteroids.ContainsKey(asteroid.LazyKey))
      {
        lazyAsteroids.Remove(asteroid.LazyKey);
      }
      else if (deactivatedAsteroids.ContainsKey(asteroid.LazyKey) == false)
      {
        deactivatedAsteroids.Add(asteroid.LazyKey, asteroid);
      }

      asteroid.ParentChunk.Asteroids.Remove(asteroid.SpawnPosition);

      if (asteroid.AsteroidObject == null) return;
      Destroy(asteroid.AsteroidObject); //Pool instead
    }
  }
}
  