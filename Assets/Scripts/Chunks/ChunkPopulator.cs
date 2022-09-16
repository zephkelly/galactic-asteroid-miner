using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  [CreateAssetMenu(fileName = "ChunkPopulator",menuName = "ScriptableObjects/ChunkPopulator")]
  public class ChunkPopulator : ScriptableObject
  {
    [SerializeField] GameObject asteroidSmallPrefab;
    [SerializeField] GameObject asteroidMediumPrefab;
    [SerializeField] GameObject asteroidLargePrefab;
    [SerializeField] GameObject asteroidExtraLPrefab;
    [SerializeField] GameObject starOrangePrefab;
    [SerializeField] GameObject starWhitePrefab;

    private List<GameObject> pickupAsteroidPool = new List<GameObject>();
    private List<GameObject> smallAsteroidPool = new List<GameObject>();
    private List<GameObject> mediumAsteroidPool = new List<GameObject>();
    private List<GameObject> largeAsteroidPool = new List<GameObject>();
    private List<GameObject> xLargeAsteroidPool = new List<GameObject>();
    private List<GameObject> starOrangePool = new List<GameObject>();
    private List<GameObject> starWhitePool = new List<GameObject>();

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

    private void Start()
    {
      //Create asteroid pools
      for (int i = 0; i < 100; i++)
      {
        var smallAsteroid = Instantiate(asteroidSmallPrefab);
        smallAsteroid.SetActive(false);
        pickupAsteroidPool.Add(smallAsteroid);
      }
      for (int i = 0; i < 20; i++)
      {
        var mediumAsteroid = Instantiate(asteroidMediumPrefab);
        mediumAsteroid.SetActive(false);
        mediumAsteroidPool.Add(mediumAsteroid);
      }
      for (int i = 0; i < 15; i++)
      {
        var largeAsteroid = Instantiate(asteroidLargePrefab);
        largeAsteroid.SetActive(false);
        largeAsteroidPool.Add(largeAsteroid);
      }
      for (int i = 0; i < 5; i++)
      {
        var xLargeAsteroid = Instantiate(asteroidExtraLPrefab);
        xLargeAsteroid.SetActive(false);
        largeAsteroidPool.Add(xLargeAsteroid);
      }

      //Create star pools
      for (int i = 0; i < 2; i++)
      {
        starOrangePool.Add(Instantiate(starOrangePrefab));
        starOrangePool[i].SetActive(false);
        starWhitePool.Add(Instantiate(starWhitePrefab));
        starWhitePool[i].SetActive(false);
      }
    }

    public void AddToAsteroidPool(GameObject asteroid, AsteroidSize size)
    {
      switch (size)
      {
        case AsteroidSize.Pickup:
          pickupAsteroidPool.Add(asteroid);
          break;
        case AsteroidSize.Small:
          smallAsteroidPool.Add(asteroid);
          break;
        case AsteroidSize.Medium:
          mediumAsteroidPool.Add(asteroid);
          break;
        case AsteroidSize.Large:
          largeAsteroidPool.Add(asteroid);
          break;
        case AsteroidSize.ExtraLarge:
          xLargeAsteroidPool.Add(asteroid);
          break;
        default: Debug.Log("Error: size not recognised");
          break;
      }
    }

    public void Populate(Vector2 key, int chunkSize, Chunk newChunkInfo)
    {
      chunkKey = key;
      chunkDiameter = chunkSize;

      GenerateAsteroids(newChunkInfo);

      GenerateStar(newChunkInfo);
    }

    private void GenerateAsteroids(Chunk chunkInfo)
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

        var asteroidInformation = new Asteroid();
        asteroidInformation.Type = AsteroidType.Iron;
        asteroidInformation.Size = AsteroidSize.Small;
        asteroidInformation.Position = randomPosition;

        chunkInfo.AddAsteroid(randomPosition, asteroidInformation);
      }

      //Medium asteroids
      for (int i = 0; i < mediumAsteroidCount; i++)
      {
        //Get a random position within the chunk
        Vector2 randomPosition = new Vector2(
          Random.Range(chunkBounds.min.x, chunkBounds.max.x),
          Random.Range(chunkBounds.min.y, chunkBounds.max.y)
        );
        
        var asteroidInformation = new Asteroid();
        asteroidInformation.Type = AsteroidType.Cobalt;
        asteroidInformation.Size = AsteroidSize.Medium;
        asteroidInformation.Position = randomPosition;

        chunkInfo.AddAsteroid(randomPosition, asteroidInformation);
      }

      //Large asteroids
      for (int i = 0; i < largeAsteroidCount; i++)
      {
        //Get a random position within the chunk
        Vector2 randomPosition = new Vector2(
          Random.Range(chunkBounds.min.x, chunkBounds.max.x),
          Random.Range(chunkBounds.min.y, chunkBounds.max.y)
        );

        var asteroidInformation = new Asteroid();
        asteroidInformation.Type = AsteroidType.Iron;
        asteroidInformation.Size = AsteroidSize.Large;
        asteroidInformation.Position = randomPosition;

        chunkInfo.AddAsteroid(randomPosition, asteroidInformation);
      }

      //Extra Large asteroids
      for (int i = 0; i < extraLargeAsteroidCount; i++)
      {
        //Get a random position within the chunk
        Vector2 randomPosition = new Vector2(
          Random.Range(chunkBounds.min.x, chunkBounds.max.x),
          Random.Range(chunkBounds.min.y, chunkBounds.max.y)
        );

        var asteroidInformation = new Asteroid();
        asteroidInformation.Type = AsteroidType.Gold;
        asteroidInformation.Size = AsteroidSize.ExtraLarge;
        asteroidInformation.Position = randomPosition;

        chunkInfo.AddAsteroid(randomPosition, asteroidInformation);
      }
    }

    private void GenerateStar(Chunk chunkInfo)
    {
      hasStar = false;
      var chunkTransform = chunkInfo.ChunkObject.transform;

      if(ChunkManager.Instance.StarCount < 2)
      {
        if(chunkKey == Vector2.zero) return;

        //Generate binary star
        if (Random.Range(0f, 100f) < generateStarChance)
        {
          var star = Instantiate(
            starOrangePrefab, chunkBounds.center, Quaternion.identity, chunkTransform);

          hasStar = true;
          ChunkManager.Instance.StarCount++;
        }
        else if (Random.Range(0f, 100f) < generateBinaryStarChance)
        {
          var star = Instantiate(
            starWhitePrefab, chunkBounds.center, Quaternion.identity, chunkTransform);

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