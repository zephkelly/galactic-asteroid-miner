using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class OcclusionManager : MonoBehaviour
  {
    /* Defunct class Upgrade to 2.0 in progress
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

      foreach (var asteroid in deactivatedAsteroids)
      {
        Destroy(asteroid.Value.AsteroidObject);
      }

      deactivatedAsteroids.Clear();
    }

    //Checking occlusion of all asteroids in chunk
    public void CheckAsteroidOcclusion(Chunk currentChunk, Vector2 playerPosition)
    {
      foreach (var asteroid in currentChunk.Asteroids)
      {
        Asteroid currentAsteroid = asteroid.Value;
        Vector2 currentSpawn = currentAsteroid.SpawnPosition;
        Vector2 currentPosition = currentAsteroid.CurrentPosition;

        //Check if active
        if (activeAsteroids.ContainsKey(asteroid.Key))
        {
          if (Vector2.Distance(playerPosition, currentPosition) > occlusionDistance)
          {
            if(currentAsteroid.AsteroidObject == null)
            {
              Debug.LogError("Asteroid object is null");
              currentChunk.Asteroids.Remove(asteroid.Key);
              return;
            }
            
            var lazyKey = currentAsteroid.SetLazyKey();
            currentAsteroid.AsteroidObject.SetActive(false);

            activeAsteroids.Remove(asteroid.Key);
            lazyAsteroids.Add(lazyKey, currentAsteroid);
            deactivationTimer.Add(lazyKey, destructionTimer);
            return;
          }
        }

        //Check if lazy
        else if (lazyAsteroids.ContainsKey(currentAsteroid.LazyKey))
        {
          if (Vector2.Distance(playerPosition, currentPosition) < occlusionDistance)
          {
            currentAsteroid.AsteroidObject.SetActive(true);
            currentAsteroid.IsLazy = false;

            activeAsteroids.Add(asteroid.Key, currentAsteroid);
            lazyAsteroids.Remove(currentAsteroid.LazyKey);
            deactivationTimer.Remove(currentAsteroid.LazyKey);
          }
          else
          {
            deactivationTimer[currentAsteroid.LazyKey] -= Time.deltaTime;

            if (deactivationTimer[currentAsteroid.LazyKey] <= 0)
            {
              Destroy(currentAsteroid.AsteroidObject);

              deactivatedAsteroids.Add(currentAsteroid.LazyKey, currentAsteroid);
              lazyAsteroids.Remove(currentAsteroid.LazyKey);
              deactivationTimer.Remove(currentAsteroid.LazyKey);
            }
          }
        }

        //New asteroid
        else if (Vector2.Distance(playerPosition, asteroid.Value.SpawnPosition) < occlusionDistance)
        {
          switch (currentAsteroid.Size)
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
              .SetAsteroid(currentAsteroid, currentChunk);
            newAsteroid.transform.parent = currentChunk.ChunkObject.transform;  //Remove get component transform
            newAsteroid.transform.position = asteroid.Key;

            activeAsteroids.Add(asteroid.Key, asteroid.Value);
          }
        }
      }
    }

    public void RemoveAsteroid(Asteroid asteroid)
    {
      if (activeAsteroids.ContainsKey(asteroid.CurrentPosition))
      {
        activeAsteroids.Remove(asteroid.CurrentPosition);
      }
      else if (lazyAsteroids.ContainsKey(asteroid.CurrentPosition))
      {
        lazyAsteroids.Remove(asteroid.CurrentPosition);
      }

      asteroid.ParentChunk.Asteroids.Remove(asteroid.SpawnPosition);
      deactivatedAsteroids.Add(asteroid.CurrentPosition, asteroid);
      Destroy(asteroid.AsteroidObject);
    }

    */
  }
}