using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class ChunkPopulator
  {
    //Stars
    //private static int starMinimumSeparation = 2400;
    //Minimum distances before a star can spawn
    private static int starSpawnChance = 15;
    private static int starMinDistance1 = 200;   //WhiteDwarf - BrownDwarf
    private static int starMinDistance2 = 1000;   //RedDwarf - YellowDwarf
    private static int starMinDistance3 = 1500;   //BlueGiant - OrangeGiant
    private static int starMinDistance4 = 2000;   //RedGiant - BlueSuperGiant
    private static int starMinDistance5 = 2500;   //RedSuperGiant - BlueHyperGiant
    private static int starMinDistance6 = 3000;   //RedHyperGiant
    private static int starMinDistance7 = 3500;   //NeutronStar
    private static int starMinDistance8 = 4000;   //BlackHole

    //Asteroids
    private static int minAsteroids = 40;
    private static int maxAsteroids = 70;
    //Minimum distances before asteroids can spawn
    private static int asteroidMinDistance1 = 0;   //Iron - Platinum
    private static int asteroidMinDistance2 = 150;   //Titanium
    private static int asteroidMinDistance3 = 250;   //Gold
    private static int asteroidMinDistance4 = 400;   //Palladium
    private static int asteroidMinDistance5 = 800;   //Cobalt
    private static int asteroidMinDistance6 = 1300;   //Stellarite
    private static int asteroidMinDistance7 = 1600;   //Darkore

    //------------------------------------------------------------------------------

    public void PopulateLargeBodies(Chunk lazyChunk)
    {
      var hasStar = GenerateStars(lazyChunk);

      if (!hasStar) return;

      GenerateAsteroids(lazyChunk, true);
    }

    public void PopulateSmallBodies(Chunk activeChunk)
    {
      if (activeChunk.HasBeenPopulated) return;   //here we could include repopulating code? special spawns?

      GenerateAsteroids(activeChunk);

      activeChunk.SetPopulated();
    }

    private bool GenerateStars(Chunk thisChunk)
    {
      if (thisChunk.Key == Vector2.zero) return false;

      bool starNearby = GetStarDistances();

      if (starNearby) return false;
      return TryGenerateStar();   //true if star was generated

      //------------------------------------------------------------------------------

      bool GetStarDistances()
      {
        Vector2Int starCheckKey = new Vector2Int(thisChunk.Key.x - 2, thisChunk.Key.y - 2);

        for (int y = 0; y < 5; y++)
        {
          for (int x = 0; x < 5; x++)
          {
            if (ChunkManager.Instance.AllChunks.ContainsKey(starCheckKey))
            {
              if (starCheckKey != thisChunk.Key)
              {
                var chunk = ChunkManager.Instance.AllChunks[starCheckKey];

                if (chunk.HasStar)
                {
                  return true;
                }
              }
            }

            starCheckKey.x++;
          }

          starCheckKey.y++;
          starCheckKey.x -= 5;
        }

        return false;
      }

      bool TryGenerateStar()
      {
        int shouldGenerateStar = Random.Range(0, 100);
        if (shouldGenerateStar > starSpawnChance) return false;   //8% chance

        int starTypeGenerator = Random.Range(0, 1000);

        float originDistance = FastDistance(thisChunk.Position, Vector2.zero);

        //WhiteDwarf - BrownDwarf
        if (originDistance >= starMinDistance1 && originDistance <= starMinDistance2)
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
        if (originDistance > starMinDistance2 && originDistance <= starMinDistance3)
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
        if (originDistance > starMinDistance3 && originDistance <= starMinDistance4)
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
        if (originDistance > starMinDistance4 && originDistance <= starMinDistance5)
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
        if (originDistance > starMinDistance5 && originDistance <= starMinDistance6)
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
        if (originDistance > starMinDistance6 && originDistance <= starMinDistance7)
        {
          if (starTypeGenerator < 990) {
            var redHyperGiant = new Star(thisChunk, StarType.RedHyperGiant);
            thisChunk.SetStar(redHyperGiant);
            return true;
          }
        }

        //NeutroStar
        if (originDistance > starMinDistance7 && originDistance <= starMinDistance8)
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

    private void GenerateAsteroids(Chunk chunk, bool hasStar = false)
    {
      //if (chunk.Key == Vector2.zero) return;

      int minimum = Random.Range(minAsteroids - 10, minAsteroids + 10);
      int maximum = Random.Range(maxAsteroids - 10, maxAsteroids + 10);
      int asteroidCount = Random.Range(minimum, maximum);

      var spawnPoint = Vector2.zero;

      if (hasStar == true)   //Generate around stars
      {
        for (int i = 0; i < asteroidCount; i++)
        {
          spawnPoint = GetPositionAroundStar(chunk.ChunkStar);
          
          chunk.PopulateAsteroid(CreateAsteroid(spawnPoint), spawnPoint);
        }
      }
      else   //Generate asteroids randomly
      {
        for (int i = 0; i < asteroidCount; i++)
        {
          spawnPoint = GetRandomPosition(chunk.ChunkBounds);

          chunk.PopulateAsteroid(CreateAsteroid(spawnPoint), spawnPoint);
        }
      }

      //------------------------------------------------------------------------------

      Asteroid CreateAsteroid(Vector2 _spawnPoint)
      {
        var randomSize = GetRandomSize();
        var randomType = GetRandomType(_spawnPoint);
        var health = GetAsteroidHealth(randomSize);

        return new Asteroid(
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
        else if (randomSize <= 80) return AsteroidSize.Medium;
        else if (randomSize <= 99) return AsteroidSize.Large;
        else return AsteroidSize.Huge;
      }

      AsteroidType GetRandomType(Vector2 _position)
      {
        int gen = Random.Range(0, 100);

        int originDistance = (int)FastDistance(_position, Vector2.zero);

        if (originDistance >= asteroidMinDistance1 && originDistance <= asteroidMinDistance2)
        {
          if (gen <= 60) return AsteroidType.Iron;
          else return AsteroidType.Platinum;
        }
        else if (originDistance > asteroidMinDistance2 && originDistance <= asteroidMinDistance3)
        {
          if (gen <= 60) return AsteroidType.Iron;
          else if (gen <= 90) return AsteroidType.Platinum;
          else return AsteroidType.Titanium;
        }

        else if (originDistance > asteroidMinDistance3 && originDistance <= asteroidMinDistance4)
        {
          if (gen <= 50) return AsteroidType.Iron;
          else if (gen <= 75) return AsteroidType.Platinum;
          else if (gen <= 90) return AsteroidType.Titanium;
          else return AsteroidType.Gold;
        }

        else if (originDistance > asteroidMinDistance4 && originDistance <= asteroidMinDistance5)
        {
          if (gen <= 45) return AsteroidType.Iron;
          else if (gen <= 70) return AsteroidType.Platinum;
          else if (gen <= 87) return AsteroidType.Titanium;
          else if (gen <= 97) return AsteroidType.Gold;
          else return AsteroidType.Palladium;
        }

        else if (originDistance > asteroidMinDistance5 && originDistance <= asteroidMinDistance6)
        {
          if (gen <= 40) return AsteroidType.Iron;
          else if (gen <= 50) return AsteroidType.Platinum;
          else if (gen <= 65) return AsteroidType.Titanium;
          else if (gen <= 85) return AsteroidType.Gold;
          else if (gen <= 97) return AsteroidType.Palladium;
          else return AsteroidType.Cobalt;
        }

        else if (originDistance > asteroidMinDistance6 && originDistance <= asteroidMinDistance7)
        {
          if (gen <= 35) return AsteroidType.Iron;
          else if (gen <= 50) return AsteroidType.Platinum;
          else if (gen <= 66) return AsteroidType.Titanium;
          else if (gen <= 75) return AsteroidType.Gold;
          else if (gen <= 84) return AsteroidType.Palladium;
          else if (gen <= 97) return AsteroidType.Cobalt;
          else return AsteroidType.Stellarite;
        }

        else if (originDistance > asteroidMinDistance7)
        {
          if (gen <= 28) return AsteroidType.Iron;
          else if (gen <= 45) return AsteroidType.Platinum;
          else if (gen <= 67) return AsteroidType.Titanium;
          else if (gen <= 76) return AsteroidType.Gold;
          else if (gen <= 85) return AsteroidType.Palladium;
          else if (gen <= 91) return AsteroidType.Cobalt;
          else if (gen <= 97) return AsteroidType.Stellarite;
          else return AsteroidType.Darkore;
        }
        else
        {
          Debug.LogWarning("Random index out of range " + originDistance);
          return AsteroidType.Iron;
        }
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

        Vector2 starPosition = orbitingStar.SpawnPoint;

        float orbitRadius = Random.Range(
          orbitingStar.AsteroidBeltRadius.x,
          orbitingStar.AsteroidBeltRadius.y
        );
        
        var positionX = orbitRadius * Mathf.Cos(randomAngle);
        var positionY = orbitRadius * Mathf.Sin(randomAngle);

        return new Vector2(positionX + starPosition.x, positionY + starPosition.y);
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

    private float FastDistance(Vector2 _point1, Vector2 _point2)
    {
      var x = _point1.x - _point2.x;
      var y = _point1.y - _point2.y;

      return Mathf.Sqrt(x * x + y * y);
    }
  }
}