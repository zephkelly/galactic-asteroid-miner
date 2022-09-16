using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  [CreateAssetMenu(fileName = "PopulateChunkManager",menuName = "ScriptableObjects/PopulateChunkManager", order = 2)]
  public class PopulateChunk : ScriptableObject
  {
    [SerializeField] GameObject asteroidSmallPrefab;
    [SerializeField] GameObject asteroidMediumPrefab;
    [SerializeField] GameObject asteroidLargePrefab;
    [SerializeField] GameObject asteroidExtraLPrefab;
    [SerializeField] GameObject starOrangePrefab;
    [SerializeField] GameObject starWhitePrefab;

    //------------------------------------------------------------------------------

    private Dictionary<Vector2, AsteroidInformation> asteroidSpawnPoints = 
      new Dictionary<Vector2, AsteroidInformation>();

    private Vector2 starSpawnPoint;

    //------------------------------------------------------------------------------
    
    private float generateStarChance = 3f;
    private float generateBinaryStarChance = 3f;
    private int minAsteroids;
    private int maxAsteroids;

    //------------------------------------------------------------------------------

    private Bounds chunkBounds;
    private Vector2 chunkKey;
    private int chunkDiameter;
    private bool hasStar;

    //------------------------------------------------------------------------------

    public void Populate(Vector2 key, int chunkSize, Transform chunkTransform)
    {
      chunkKey = key;
      chunkDiameter = chunkSize;

      GenerateAsteroids(chunkTransform);

      GenerateStar(chunkTransform);
    }

    private void GenerateAsteroids(Transform chunkTransform)
    {
      //Get the chunk's position in world space
      Vector2 chunkWorldPosition = chunkKey * chunkDiameter;

      //Get the chunk's bounds
      chunkBounds = new Bounds(chunkWorldPosition, Vector2.one * chunkDiameter);

      //Get the number of asteroids to generate
      minAsteroids = Random.Range(40, 60);
      maxAsteroids = Random.Range(90, 120);
      int asteroidBaseCount = Random.Range(minAsteroids, maxAsteroids);

      int smallAsteroidCount = asteroidBaseCount;
      int mediumAsteroidCount = asteroidBaseCount / 8;
      int extraLargeAsteroidCount = asteroidBaseCount / 14;
      int largeAsteroidCount = asteroidBaseCount / 16;

      var totalAsteroidCount = 
        smallAsteroidCount + 
        mediumAsteroidCount + 
        largeAsteroidCount + 
        extraLargeAsteroidCount;

      //Small asteroids
      for (int i = 0; i < smallAsteroidCount; i++)
      {
        //Get a random position within the chunk
        Vector2 randomPosition = new Vector2(
          Random.Range(chunkBounds.min.x, chunkBounds.max.x),
          Random.Range(chunkBounds.min.y, chunkBounds.max.y)
        );

        var asteroid = Instantiate(
          asteroidSmallPrefab, randomPosition, Quaternion.identity, chunkTransform);

        var asteroidInformation = new AsteroidInformation();
        asteroidInformation.Type = AsteroidType.Iron;
        asteroidInformation.Size = AsteroidSize.Small;
        asteroidInformation.Position = randomPosition;

        asteroid.GetComponent<AsteroidController>().Init(asteroidInformation);
        asteroidSpawnPoints.Add(randomPosition, asteroidInformation);
      }

      //Medium asteroids
      for (int i = 0; i < mediumAsteroidCount; i++)
      {
        //Get a random position within the chunk
        Vector2 randomPosition = new Vector2(
          Random.Range(chunkBounds.min.x, chunkBounds.max.x),
          Random.Range(chunkBounds.min.y, chunkBounds.max.y)
        );

        var asteroid = Instantiate(
          asteroidMediumPrefab, randomPosition, Quaternion.identity, chunkTransform);
        
        var asteroidInformation = new AsteroidInformation();
        asteroidInformation.Type = AsteroidType.Cobalt;
        asteroidInformation.Size = AsteroidSize.Medium;
        asteroidInformation.Position = randomPosition;

        asteroid.GetComponent<AsteroidController>().Init(asteroidInformation);
        asteroidSpawnPoints.Add(randomPosition, asteroidInformation);
      }

      //Large asteroids
      for (int i = 0; i < largeAsteroidCount; i++)
      {
        //Get a random position within the chunk
        Vector2 randomPosition = new Vector2(
          Random.Range(chunkBounds.min.x, chunkBounds.max.x),
          Random.Range(chunkBounds.min.y, chunkBounds.max.y)
        );

        var asteroid = Instantiate(
          asteroidLargePrefab, randomPosition, Quaternion.identity, chunkTransform);
        
        var asteroidInformation = new AsteroidInformation();
        asteroidInformation.Type = AsteroidType.Iron;
        asteroidInformation.Size = AsteroidSize.Large;
        asteroidInformation.Position = randomPosition;

        asteroid.GetComponent<AsteroidController>().Init(asteroidInformation);
        asteroidSpawnPoints.Add(randomPosition, asteroidInformation);
      }

      //Extra Large asteroids
      for (int i = 0; i < extraLargeAsteroidCount; i++)
      {
        //Get a random position within the chunk
        Vector2 randomPosition = new Vector2(
          Random.Range(chunkBounds.min.x, chunkBounds.max.x),
          Random.Range(chunkBounds.min.y, chunkBounds.max.y)
        );

        var asteroid = Instantiate(
          asteroidExtraLPrefab, randomPosition, Quaternion.identity, chunkTransform);
        
        var asteroidInformation = new AsteroidInformation();
        asteroidInformation.Type = AsteroidType.Gold;
        asteroidInformation.Size = AsteroidSize.ExtraLarge;
        asteroidInformation.Position = randomPosition;

        asteroid.GetComponent<AsteroidController>().Init(asteroidInformation);
        asteroidSpawnPoints.Add(randomPosition, asteroidInformation);
      }
    }

    private void GenerateStar(Transform chunkTransform)
    {
      hasStar = false;

      if(ChunkManager.Instance.StarCount < 2)
      {
        if(chunkKey == Vector2.zero) return;

        //Generate binary star
        if (Random.Range(0f, 100f) < generateStarChance)
        {
          var star = Instantiate(
            starOrangePrefab, chunkBounds.center, Quaternion.identity, chunkTransform);
          starSpawnPoint = star.transform.position;

          hasStar = true;
          ChunkManager.Instance.StarCount++;
        }
        else if (Random.Range(0f, 100f) < generateBinaryStarChance)
        {
          var star = Instantiate(
            starWhitePrefab, chunkBounds.center, Quaternion.identity, chunkTransform);
          starSpawnPoint = star.transform.position;

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