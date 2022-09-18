using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class OcclusionManager : MonoBehaviour
  {
    private ChunkManager chunkManager;
    private ChunkPopulator chunkPopulator;

    [SerializeField] GameObject asteroidPickupPrefab;
    [SerializeField] GameObject asteroidSmallPrefab;
    [SerializeField] GameObject asteroidMediumPrefab;
    [SerializeField] GameObject asteroidLargePrefab;
    [SerializeField] GameObject asteroidExtraLPrefab;

    [SerializeField] int occlusionDistance = 50;

    //------------------------------------------------------------------------------

    private Dictionary<Vector2, Asteroid> activeAsteroids = 
      new Dictionary<Vector2, Asteroid>();

    private Dictionary<Vector2, Asteroid> lazyAsteroids = 
      new Dictionary<Vector2, Asteroid>();

    private Dictionary<Vector2, Asteroid> deactivatedAsteroids =
      new Dictionary<Vector2, Asteroid>();

    private void Start()
    {
      chunkManager = ChunkManager.Instance;
    }

    private void Update()
    {
      foreach (var chunk in chunkManager.ActiveChunks)
      {
        CheckAsteroidOcclusion(chunk.Value, chunkManager.PlayerTransform.position);
      }
    }

    //Checking occlusion of all asteroids in chunk
    public void CheckAsteroidOcclusion(Chunk currentChunk, Vector2 playerPosition)
    {
      foreach (var asteroid in currentChunk.Asteroids)
      {
        Asteroid currentAsteroid = asteroid.Value;

        if (activeAsteroids.ContainsKey(asteroid.Key))
        {
          if (Vector2.Distance(playerPosition, currentAsteroid.CurrentPosition) > occlusionDistance)
          { 
            if(currentAsteroid.AsteroidObject == null)
            {
              currentChunk.Asteroids.Remove(asteroid.Key);
              return;
            }
            
            currentAsteroid.AsteroidObject.SetActive(false);

            activeAsteroids.Remove(asteroid.Key);
            lazyAsteroids.Add(asteroid.Key, currentAsteroid);

            return;
            //Destroy(asteroid.Value.AsteroidObject);   //need to pool object here
          }
        }
        else if (lazyAsteroids.ContainsKey(asteroid.Key))
        {
          if (Vector2.Distance(playerPosition, currentAsteroid.SpawnPosition) < occlusionDistance)
          {
            currentAsteroid.AsteroidObject.SetActive(true);

            lazyAsteroids.Remove(asteroid.Key);
            activeAsteroids.Add(asteroid.Key, currentAsteroid);
          }
        }
        else if (Vector2.Distance(playerPosition, asteroid.Value.SpawnPosition) < occlusionDistance)
        {
          if (asteroid.Value.HasBeenActive == true)
          {
            //reactivate from pool with the new spawn position as the key
          }
          else
          {
            switch (currentAsteroid.Size)
            {
              case AsteroidSize.Pickup:
                
                var _asteroidP = Instantiate(asteroidPickupPrefab);
                _asteroidP.GetComponent<AsteroidController>()
                  .SetAsteroid(currentAsteroid, currentChunk);
                _asteroidP.transform.parent = currentChunk.ChunkObject.transform;
                _asteroidP.transform.position = asteroid.Key;

                activeAsteroids.Add(asteroid.Key, asteroid.Value);
                break;

              case AsteroidSize.Small:
                if (activeAsteroids.ContainsKey(asteroid.Key) == true) return;

                var _asteroidS = Instantiate(asteroidSmallPrefab);
                _asteroidS.GetComponent<AsteroidController>()
                  .SetAsteroid(currentAsteroid, currentChunk);
                _asteroidS.transform.parent = currentChunk.ChunkObject.transform;
                _asteroidS.transform.position = asteroid.Key;

                activeAsteroids.Add(asteroid.Key, asteroid.Value);
                break;

              case AsteroidSize.Medium:
                if (activeAsteroids.ContainsKey(asteroid.Key) == true) return;

                var _asteroidM = Instantiate(asteroidMediumPrefab);
                _asteroidM.GetComponent<AsteroidController>()
                  .SetAsteroid(currentAsteroid, currentChunk);
                _asteroidM.transform.parent = currentChunk.ChunkObject.transform;
                _asteroidM.transform.position = asteroid.Key;

                activeAsteroids.Add(asteroid.Key, asteroid.Value);
                break;

              case AsteroidSize.Large:
                if (activeAsteroids.ContainsKey(asteroid.Key) == true) return;

                var _asteroidL = Instantiate(asteroidLargePrefab);
                _asteroidL.GetComponent<AsteroidController>()
                  .SetAsteroid(currentAsteroid, currentChunk);
                _asteroidL.transform.parent = currentChunk.ChunkObject.transform;
                _asteroidL.transform.position = asteroid.Key;

                activeAsteroids.Add(asteroid.Key, asteroid.Value);
                break;

              case AsteroidSize.ExtraLarge:
                if (activeAsteroids.ContainsKey(asteroid.Key) == true) return;

                var _asteroidXL = Instantiate(asteroidLargePrefab);
                _asteroidXL.GetComponent<AsteroidController>()
                  .SetAsteroid(currentAsteroid, currentChunk);
                _asteroidXL.transform.parent = currentChunk.ChunkObject.transform;
                _asteroidXL.transform.position = asteroid.Key;

                activeAsteroids.Add(asteroid.Key, asteroid.Value);
                break;
            }
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
  }
}
  