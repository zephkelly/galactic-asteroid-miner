using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class PopulateChunk : MonoBehaviour
  {
    private GameObject asteroidSmallPrefab;
    private GameObject asteroidMediumPrefab;
    private GameObject asteroidLargePrefab;
    private GameObject asteroidExtraLargePrefab;
    private GameObject starOrangePrefab;
    private GameObject starWhitePrefab;

    private float generateStarChance = 3f;

    private float generateBinaryStarChance = 3f;
    
    private int minAsteroids;

    private int maxAsteroids;

    //----------------------------------------------------------------------------------------------

    private Bounds chunkBounds;

    private Vector2 chunkKey;

    private int chunkDiameter;

    private bool hasStar;

    //----------------------------------------------------------------------------------------------

    public void Awake()
    {
      //Grab our prefabs from resources folder
      asteroidSmallPrefab = Resources.Load("Prefabs/Asteroid-S") as GameObject;
      asteroidMediumPrefab = Resources.Load("Prefabs/Asteroid-M") as GameObject;
      asteroidLargePrefab = Resources.Load("Prefabs/Asteroid-L") as GameObject;
      asteroidExtraLargePrefab = Resources.Load("Prefabs/Asteroid-XL") as GameObject;

      starOrangePrefab = Resources.Load("Prefabs/StarOrange") as GameObject;
      starWhitePrefab = Resources.Load("Prefabs/StarWhite") as GameObject;
    }

    public void Populate(Vector2 key, int chunkSize)
    {
      chunkKey = key;
      chunkDiameter = chunkSize;

      GenerateAsteroids();

      GenerateStar();
    }

    private void GenerateAsteroids()
    {
      //Get the chunk's position in world space
      Vector2 chunkWorldPosition = chunkKey * chunkDiameter;

      //Get the chunk's bounds
      chunkBounds = new Bounds(chunkWorldPosition, Vector2.one * chunkDiameter);

      //Get the number of asteroids to generate
      minAsteroids = Random.Range(40, 60);
      maxAsteroids = Random.Range(90, 120);
      int totalAsteroidCount = Random.Range(minAsteroids, maxAsteroids);

      int smallAsteroidCount = totalAsteroidCount;
      int mediumAsteroidCount = totalAsteroidCount / 8;
      int extraLargeAsteroidCount = totalAsteroidCount / 14;
      int largeAsteroidCount = totalAsteroidCount / 16;

      //Small asteroids
      for (int i = 0; i < smallAsteroidCount; i++)
      {
        //Get a random position within the chunk
        Vector2 randomPosition = new Vector2(
          Random.Range(chunkBounds.min.x, chunkBounds.max.x),
          Random.Range(chunkBounds.min.y, chunkBounds.max.y)
        );

        var asteroid = Instantiate(asteroidSmallPrefab, randomPosition, Quaternion.identity, this.transform);
        asteroid.GetComponent<AsteroidBehaviour>().SetAsteroid(AsteroidType.Iron, AsteroidSize.Small);
      }

      //Medium asteroids
      for (int i = 0; i < mediumAsteroidCount; i++)
      {
        //Get a random position within the chunk
        Vector2 randomPosition = new Vector2(
          Random.Range(chunkBounds.min.x, chunkBounds.max.x),
          Random.Range(chunkBounds.min.y, chunkBounds.max.y)
        );

        var asteroid = Instantiate(asteroidMediumPrefab, randomPosition, Quaternion.identity, this.transform);
        asteroid.GetComponent<AsteroidBehaviour>().SetAsteroid(AsteroidType.Cobalt, AsteroidSize.Medium);
      }

      //Large asteroids
      for (int i = 0; i < largeAsteroidCount; i++)
      {
        //Get a random position within the chunk
        Vector2 randomPosition = new Vector2(
          Random.Range(chunkBounds.min.x, chunkBounds.max.x),
          Random.Range(chunkBounds.min.y, chunkBounds.max.y)
        );

        var asteroid = Instantiate(asteroidLargePrefab, randomPosition, Quaternion.identity, this.transform);
        asteroid.GetComponent<AsteroidBehaviour>().SetAsteroid(AsteroidType.Gold, AsteroidSize.Large);

      }

      //Extra Large asteroids
      for (int i = 0; i < extraLargeAsteroidCount; i++)
      {
        //Get a random position within the chunk
        Vector2 randomPosition = new Vector2(
          Random.Range(chunkBounds.min.x, chunkBounds.max.x),
          Random.Range(chunkBounds.min.y, chunkBounds.max.y)
        );

        var asteroid = Instantiate(asteroidExtraLargePrefab, randomPosition, Quaternion.identity, this.transform);
        asteroid.GetComponent<AsteroidBehaviour>().SetAsteroid(AsteroidType.Cobalt, AsteroidSize.ExtraLarge);
      }
    }

    private void GenerateStar()
    {
      hasStar = false;

      if(ChunkManager.Instance.StarCount < 2)
      {
        //Generate binary star
        if (Random.Range(0f, 100f) < generateStarChance)
        {
          Instantiate(starOrangePrefab, chunkBounds.center, Quaternion.identity, this.transform);

          hasStar = true;
          ChunkManager.Instance.StarCount++;
        }
        else if (Random.Range(0f, 100f) < generateBinaryStarChance)
        {
          Instantiate(starWhitePrefab, chunkBounds.center, Quaternion.identity, this.transform);

          hasStar = true;
          ChunkManager.Instance.StarCount++;
        }
      }
    }

    private void OnEnable()
    {
      if (hasStar)
      {
        ChunkManager.Instance.StarCount++;
      }
    }

    private void OnDisable()
    {
      if (hasStar) 
      {
        ChunkManager.Instance.StarCount--;
      }
    }
  }
}