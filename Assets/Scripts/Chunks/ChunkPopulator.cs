using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  [CreateAssetMenu(fileName = "ChunkPopulator",menuName = "ScriptableObjects/ChunkPopulator")]
  public class ChunkPopulator : ScriptableObject
  {
    [SerializeField] GameObject starOrangePrefab;
    [SerializeField] GameObject starWhitePrefab;
    
    [SerializeField] int generateStarChance = 3;
    [SerializeField] int generateBinaryStarChance = 3;

    private Bounds chunkBounds;
    private Vector2 chunkKey;

    private int chunkDiameter;

    //------------------------------------------------------------------------------

    public void Populate(Vector2 key, int chunkSize, Chunk newChunkInfo)
    {
      chunkKey = key;
      chunkDiameter = chunkSize;

      GenerateAsteroidInformation(newChunkInfo);
      GenerateStar(newChunkInfo);
    }

    private void GenerateAsteroidInformation(Chunk chunkInfo)
    {
      //Get the chunk's position in world space
      Vector2 chunkWorldPosition = chunkKey * chunkDiameter;

      chunkBounds = new Bounds(chunkWorldPosition, Vector2.one * chunkDiameter);

      //Get the number of asteroids to generate
      int minAsteroids = Random.Range(40, 60);
      int maxAsteroids = Random.Range(90, 120);

      int asteroidBaseCount = Random.Range(minAsteroids, maxAsteroids);
      int smallAsteroidCount = asteroidBaseCount;
      int mediumAsteroidCount = asteroidBaseCount / 8;
      int extraLargeAsteroidCount = asteroidBaseCount / 14;
      int largeAsteroidCount = asteroidBaseCount / 16;

      //Small asteroids
      for (int i = 0; i < smallAsteroidCount; i++)
      {
        Asteroid smallAsteroid = CreateAsteroid();
        smallAsteroid.Size = AsteroidSize.Small;
        chunkInfo.AddAsteroid(smallAsteroid);
      }

      //Medium asteroids
      for (int i = 0; i < mediumAsteroidCount; i++)
      {
        Asteroid mediumAsteroid = CreateAsteroid();
        mediumAsteroid.Size = AsteroidSize.Medium;
        chunkInfo.AddAsteroid(mediumAsteroid);
      }

      //Large asteroids
      for (int i = 0; i < largeAsteroidCount; i++)
      {
        Asteroid largeAsteroid = CreateAsteroid();
        largeAsteroid.Size = AsteroidSize.Large;
        chunkInfo.AddAsteroid(largeAsteroid);
      }

      //Extra Large asteroids
      for (int i = 0; i < extraLargeAsteroidCount; i++)
      {
        Asteroid extraLargeAsteroid = CreateAsteroid();
        extraLargeAsteroid.Size = AsteroidSize.ExtraLarge;
        chunkInfo.AddAsteroid(extraLargeAsteroid);
      }
    }

    private void GenerateStar(Chunk chunkInfo)
    {
      var chunkTransform = chunkInfo.ChunkObject.transform;

      if(ChunkManager.Instance.StarCount < 2)
      {
        if(chunkKey == Vector2.zero) return; //Dont insta kill player

        //Generate binary star
        if (Random.Range(0f, 100f) < generateStarChance)
        {
          var star = Instantiate(
          starOrangePrefab, chunkBounds.center, Quaternion.identity, chunkTransform);

          ChunkManager.Instance.StarCount++;
        }
        else if (Random.Range(0f, 100f) < generateBinaryStarChance)
        {
          var star = Instantiate(
          starWhitePrefab, chunkBounds.center, Quaternion.identity, chunkTransform);

          ChunkManager.Instance.StarCount++;
        }
      }
    }

    private Asteroid CreateAsteroid()
    {
      var asteroid = new Asteroid();
      asteroid.Type = GetRandomType();
      asteroid.SetNewSpawn(GetRandomPosition());

      return asteroid;
    }

    private Vector2 GetRandomPosition()
    {
      return new Vector2(
          Random.Range(chunkBounds.min.x, chunkBounds.max.x),
          Random.Range(chunkBounds.min.y, chunkBounds.max.y)
        );
    }

    private AsteroidType GetRandomType()
    {
      int randomType = Random.Range(0, 100);
      //To be balanced later...
      if  (randomType >= 0 && randomType < 40) return AsteroidType.Iron;
      else if (randomType >= 40 && randomType < 55) return AsteroidType.Platinum;
      else if (randomType >= 55 && randomType < 70) return AsteroidType.Gold;
      else if (randomType >= 70 && randomType < 80) return AsteroidType.Palladium;
      else if (randomType >= 80 && randomType < 90) return AsteroidType.Cobalt;
      else if (randomType >= 90 && randomType < 96) return AsteroidType.Stellarite;
      else if (randomType >= 96 && randomType <= 100) return AsteroidType.Darkore;
      else 
      {
        Debug.LogError("Random type out of range " + randomType);
        return AsteroidType.Iron;
      }
    }
  }
}