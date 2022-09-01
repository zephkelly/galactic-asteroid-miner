using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class PopulateChunk : MonoBehaviour
  {
    [SerializeField] GameObject asteroidPrefab;

    [SerializeField] GameObject starPrefab;

    private int generateStarChance = 10; 
    
    private int minAsteroids;

    private int maxAsteroids;

    //----------------------------------------------------------------------------------------------

    private Bounds chunkBounds;

    private Vector2 chunkKey;

    private int chunkDiameter;

    //----------------------------------------------------------------------------------------------

    public void Awake()
    {
      //Grab our prefabs from resources folder
      asteroidPrefab = Resources.Load("Prefabs/Asteroid-M") as GameObject;

      starPrefab = Resources.Load("Prefabs/BinaryStar") as GameObject;
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
      minAsteroids = Random.Range(15, 30);
      maxAsteroids = Random.Range(100, 150);

      int asteroidCount = Random.Range(minAsteroids, maxAsteroids);

      //Generate asteroids
      for (int i = 0; i < asteroidCount; i++)
      {
        //Get a random position within the chunk
        Vector2 randomPosition = new Vector2(
          Random.Range(chunkBounds.min.x, chunkBounds.max.x),
          Random.Range(chunkBounds.min.y, chunkBounds.max.y)
        );

        //Instantiate the asteroid
        Instantiate(asteroidPrefab, randomPosition, Quaternion.identity, this.transform);
      }
    }

    private void GenerateStar()
    {
      //Generate star
      if (Random.Range(0, 100) < generateStarChance)
      {
        //Get a random position within the chunk
        Vector2 randomPosition = new Vector2(
          Random.Range(chunkBounds.min.x, chunkBounds.max.x),
          Random.Range(chunkBounds.min.y, chunkBounds.max.y)
        );

        //Instantiate the star
        Instantiate(starPrefab, randomPosition, Quaternion.identity, this.transform);
      }
    }
  }
}