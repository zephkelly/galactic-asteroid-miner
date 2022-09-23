using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class ChunkPopulator2
  {
    //Stars
    private static int starMinimumSeparation = 400;
    //Minimum distances before a star can spawn
    private static int starMinDistance1 = 300;   //WhiteDwarf - BrownDwarf
    private static int starMinDistance2 = 600;   //RedDwarf - YellowDwarf
    private static int starMinDistance3 = 900;   //BlueGiant - OrangeGiant
    private static int starMinDistance4 = 1200;   //RedGiant - BlueSuperGiant
    private static int starMinDistance5 = 1500;   //RedSuperGiant - BlueHyperGiant
    private static int starMinDistance6 = 1800;   //RedHyperGiant
    private static int starMinDistance7 = 2100;   //NeutronStar
    private static int starMinDistance8 = 2400;   //BlackHole

    //Asteroids
    private static int minAsteroids = 40;
    private static int maxAsteroids = 70;
    //Minimum distances before asteroids can spawn
    private static int asteroidMinDistance1 = 0;   //Iron - Platinum
    private static int asteroidMinDistance2 = 300;   //Gold
    private static int asteroidMinDistance3 = 600;   //Palladium
    private static int asteroidMinDistance4 = 900;   //Cobalt
    private static int asteroidMinDistance5 = 1200;   //Stellarite
    private static int asteroidMinDistance6 = 1500;   //Darkore

    //------------------------------------------------------------------------------

    public bool PopulateLargeBodies(Chunk2 lazyChunk)
    {
      var hasStar = GenerateStars(lazyChunk, ChunkManager2.Instance.LazyChunks);

      if (!hasStar) return false;

      GenerateAsteroids(lazyChunk, true);
      return true;
    }

    public void PopulateSmallBodies(Chunk2 activeChunk)
    {
      if (activeChunk.HasBeenPopulated) return;   //here we could include repopulating code? special spawns?

      GenerateAsteroids(activeChunk, hasStar: false);

      activeChunk.SetPopulated();
    }

    private bool GenerateStars(Chunk2 thisChunk, Dictionary<Vector2Int,Chunk2> chunks)
    {
      if (thisChunk.Key == Vector2.zero) return false;

      //Stars are always generated in the center of a chunk
      List<float> starDistances = new List<float>();
      
      GetStarDistances();

      if (CanGenerateStar()) 
      {
        return TryGenerateStar();   //returns true if star was generated
      } else {
        return false; 
      }

      //------------------------------------------------------------------------------

      void GetStarDistances()
      {
        foreach (var chunk in chunks)
        {
          if (chunk.Key == thisChunk.Key) continue;   //ignore ourselves

          if (chunk.Value.HasStar)
          {
            var distanceToStar = Vector2.Distance (
              thisChunk.Position,
              chunk.Value.Position
            );

            starDistances.Add(distanceToStar);
          }
          else continue;
        }
      }

      bool CanGenerateStar()
      {
        if (starDistances.Count == 0) return true;   //if there are no stars

        for (int i = 0; i < starDistances.Count; i++)
        {
          if (starDistances[i] < starMinimumSeparation) return false;
        }

        return true;
      }

      bool TryGenerateStar()
      {
        int shouldGenerateStar = Random.Range(0, 100);
        if (shouldGenerateStar > 10) return false;   //6% chance

        int starTypeGenerator = Random.Range(0, 1000);

        float originDistance = Vector2.Distance(thisChunk.Position, Vector2.zero);

        //WhiteDwarf - BrownDwarf
        if (originDistance > starMinDistance1)
        {
          if (starTypeGenerator < 100) {
            var whiteDwarf = new Star(thisChunk, StarType.WhiteDwarf);
            thisChunk.SetStar(whiteDwarf);
            return true;
          }
          else if (starTypeGenerator < 200) {
            var brownDwarf = new Star(thisChunk, StarType.BrownDwarf);
            thisChunk.SetStar(brownDwarf);
            return true;
          }
        }

        //RedDwarf - YellowDwarf
        if (originDistance > starMinDistance2)
        {
          if (starTypeGenerator < 300) {
            var redDwarf = new Star(thisChunk, StarType.RedDwarf);
            thisChunk.SetStar(redDwarf);
            return true;
          }
          else if (starTypeGenerator < 400) {
            var yellowDwarf = new Star(thisChunk, StarType.YellowDwarf);
            thisChunk.SetStar(yellowDwarf);
            return true;
          }
        }

        //BlueGiant - OrangeGiant
        if (originDistance > starMinDistance3)
        {
          if (starTypeGenerator < 500) {
            var blueGiant = new Star(thisChunk, StarType.BlueGiant);
            thisChunk.SetStar(blueGiant);
            return true;
          }
          else if (starTypeGenerator < 600) {
            var orangeGiant = new Star(thisChunk, StarType.OrangeGiant);
            thisChunk.SetStar(orangeGiant);
            return true;
          }
        }

        //RedGiant - BlueSuperGiant
        if (originDistance > starMinDistance4)
        {
          if (starTypeGenerator < 700) {
            var redGiant = new Star(thisChunk, StarType.RedGiant);
            thisChunk.SetStar(redGiant);
            return true;
          }
          else if (starTypeGenerator < 800) {
            var blueSuperGiant = new Star(thisChunk, StarType.BlueSuperGiant);
            thisChunk.SetStar(blueSuperGiant);
            return true;
          }
        }

        //RedSuperGiant - BlueHyperGiant
        if (originDistance > starMinDistance5)
        {
          if (starTypeGenerator < 900) {
            var redSuperGiant = new Star(thisChunk, StarType.RedSuperGiant);
            thisChunk.SetStar(redSuperGiant);
            return true;
          }
          else if (starTypeGenerator < 950) {
            var blueHyperGiant = new Star(thisChunk, StarType.BlueHyperGiant);
            thisChunk.SetStar(blueHyperGiant);
            return true;
          }
        }

        //RedHyperGiant
        if (originDistance > starMinDistance6)
        {
          if (starTypeGenerator < 990) {
            var redHyperGiant = new Star(thisChunk, StarType.RedHyperGiant);
            thisChunk.SetStar(redHyperGiant);
            return true;
          }
        }

        //NeutroStar
        if (originDistance > starMinDistance7)
        {
          if (starTypeGenerator < 995) {
            var neutronStar = new Star(thisChunk, StarType.NeutronStar);
            thisChunk.SetStar(neutronStar);
            return true;
          }
        }

        //BlackHole
        if (originDistance > starMinDistance8)
        {
          if (starTypeGenerator > 995) {
            var blackHole = new Star(thisChunk, StarType.BlackHole);
            thisChunk.SetStar(blackHole);
            return true;
          }
        }

        return false;
      }
    }

    private void GenerateAsteroids(Chunk2 chunk, bool hasStar)
    {
      int minimum = Random.Range(minAsteroids - 10, minAsteroids + 10);
      int maximum = Random.Range(maxAsteroids - 10, maxAsteroids + 10);
      int asteroidCount = Random.Range(minimum, maximum);

      var spawnPoint = Vector2.zero;

      if (hasStar == true)   //Generate around stars
      {
        for (int i = 0; i < asteroidCount; i++)
        {
          spawnPoint = GetPositionAroundStar(chunk.ChunkStar);
          
          if (chunk.Asteroids.ContainsKey(spawnPoint)) return;

          chunk.AddAsteroid(CreateAsteroid(spawnPoint), spawnPoint);
        }
      }
      else   //Generate asteroids randomly
      {
        for (int i = 0; i < asteroidCount; i++)
        {
          spawnPoint = GetRandomPosition(chunk.ChunkBounds);

          if (chunk.Asteroids.ContainsKey(spawnPoint)) return;

          chunk.AddAsteroid(CreateAsteroid(spawnPoint), spawnPoint);
        }
      }

      //------------------------------------------------------------------------------

      Asteroid2 CreateAsteroid(Vector2 _spawnPoint)
      {
        var randomSize = GetRandomSize();
        var randomType = GetRandomType();
        var health = GetAsteroidHealth(randomSize);

        return new Asteroid2(
          chunk,
          randomSize,
          randomType,
          _spawnPoint,
          health
        );
      }

      AsteroidSize GetRandomSize()
      {
        int randomSize = Random.Range(0, 100);

        if (randomSize <= 60) return AsteroidSize.Small;
        else if (randomSize <= 85) return AsteroidSize.Medium;
        else if (randomSize <= 99) return AsteroidSize.Large;
        else return AsteroidSize.Huge;
      }

      AsteroidType GetRandomType()
      {
        int gen = Random.Range(0, 100);
        float originDistance = Vector2.Distance(chunk.Position, Vector2.zero);

        if (originDistance > asteroidMinDistance1)
        {
          if (gen <= 80) return AsteroidType.Iron;
          else return AsteroidType.Platinum;
        }

        else if (originDistance > asteroidMinDistance2)
        {
          if (gen <= 70) return AsteroidType.Iron;
          else if (gen <= 90) return AsteroidType.Platinum;
          else return AsteroidType.Gold;
        }

        else if (originDistance > asteroidMinDistance3)
        {
          if (gen <= 55) return AsteroidType.Iron;
          else if (gen <= 75) return AsteroidType.Platinum;
          else if (gen <= 90) return AsteroidType.Gold;
          else return AsteroidType.Palladium;
        }

        else if (originDistance > asteroidMinDistance4)
        {
          if (gen <= 40) return AsteroidType.Iron;
          else if (gen <= 60) return AsteroidType.Platinum;
          else if (gen <= 80) return AsteroidType.Gold;
          else if (gen <= 90) return AsteroidType.Palladium;
          else return AsteroidType.Cobalt;
        }

        else if (originDistance > asteroidMinDistance5)
        {
          if (gen <= 30) return AsteroidType.Iron;
          else if (gen <= 50) return AsteroidType.Platinum;
          else if (gen <= 70) return AsteroidType.Gold;
          else if (gen <= 80) return AsteroidType.Palladium;
          else if (gen <= 90) return AsteroidType.Cobalt;
          else return AsteroidType.Stellarite;
        }

        else if (originDistance > asteroidMinDistance6)
        {
          if (gen <= 20) return AsteroidType.Iron;
          else if (gen <= 40) return AsteroidType.Platinum;
          else if (gen <= 60) return AsteroidType.Gold;
          else if (gen <= 70) return AsteroidType.Palladium;
          else if (gen <= 80) return AsteroidType.Cobalt;
          else if (gen <= 90) return AsteroidType.Stellarite;
          else return AsteroidType.Darkore;
        }

        else return AsteroidType.Iron;
      }

      Vector2 GetRandomPosition(Bounds chunkBounds)
      {
        return new Vector2(
            Random.Range(chunkBounds.min.x, chunkBounds.max.x),
            Random.Range(chunkBounds.min.y, chunkBounds.max.y)
          );
      }

      Vector2 GetPositionAroundStar(Star orbitingStar)
      {
        var randomAngle = Random.Range(0f, (2 * Mathf.PI));

        float orbitRadius = Random.Range(
          orbitingStar.AsteroidBeltRadius.x,
          orbitingStar.AsteroidBeltRadius.y
        );
        
        var positionX = orbitRadius * Mathf.Cos(randomAngle);
        var positionY = orbitRadius * Mathf.Sin(randomAngle);

        return new Vector2(positionX, positionY);
      }

      int GetAsteroidHealth(AsteroidSize _size)
      {
        //health based on AsteroidSize
        int health = 0;

        switch (_size)
        {
          case AsteroidSize.Small:
            health = Random.Range(2, 3);
            break;
          case AsteroidSize.Medium:
            health = Random.Range(4, 5);
            break;
          case AsteroidSize.Large:
            health = Random.Range(6, 7);
            break;
          case AsteroidSize.Huge:
            health = Random.Range(8, 9);
            break;
        }

        return health;
      }
    }
  }
}