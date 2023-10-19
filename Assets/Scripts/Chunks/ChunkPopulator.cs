using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class ChunkPopulator
  {
    //Depo
    private static int depoSpawnChance = 32;

    //Stars
    //private static int starMinimumSeparation = 2400;
    //Minimum distances before a star can spawn
    private static int starSpawnChance = 98;
    private static int starMinDistance1 = 200;   //WhiteDwarf - BrownDwarf
    private static int starMinDistance2 = 500;   //RedDwarf - YellowDwarf
    private static int starMinDistance3 = 1000;   //BlueGiant - OrangeGiant
    private static int starMinDistance4 = 1000;   //RedGiant - BlueSuperGiant
    private static int starMinDistance5 = 1500;   //RedSuperGiant - BlueHyperGiant
    private static int starMinDistance6 = 2000;   //RedHyperGiant
    private static int starMinDistance7 = 2500;   //NeutronStar
    private static int starMinDistance8 = 3000;   //BlackHole

    //Asteroids
    private static int minAsteroids = 16;
    private static int maxAsteroids = 50;

    //Minimum distances before asteroids can spawn
    private static int asteroidMinDistance1 = 0;   //Iron - Platinum
    private static int asteroidMinDistance2 = 350;   //Titanium
    private static int asteroidMinDistance3 = 750;   //Gold
    private static int asteroidMinDistance4 = 1000;   //Palladium
    private static int asteroidMinDistance5 = 1600;   //Cobalt
    private static int asteroidMinDistance6 = 2000;   //Stellarite
    private static int asteroidMinDistance7 = 4000;   //Darkore

    //------------------------------------------------------------------------------

    public void PopulateLargeBodies(Chunk lazyChunk)
    {
      var hasStar = GenerateStars(lazyChunk);

      if (hasStar)
      {
        GenerateAsteroids(lazyChunk, hasStar:true);
        return;
      }
    
      var hasDepo = GenerateDepos(lazyChunk);

      if (hasDepo) 
      {
        GenerateAsteroids(lazyChunk, hasDepo:true);
        return;
      }
    }

    public void PopulateSmallBodies(Chunk activeChunk)
    {
      if (activeChunk.HasBeenPopulated) return;   //here we could include repopulating code? special spawns?

      GenerateAsteroids(activeChunk);

      activeChunk.SetPopulated();
    }

    private bool GenerateDepos(Chunk thisChunk)
    {
      if (thisChunk.Key == Vector2.zero) return false;

      bool starNearby = GetStarDistances();

      if (starNearby) return false;
      return TryGenerateDepo();

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

      bool TryGenerateDepo()
      {
        int shouldGenerateDepo = Random.Range(0, 1000);
        if (shouldGenerateDepo > depoSpawnChance) return false;

        float originDistance = FastDistance(thisChunk.Position, Vector2.zero);

        thisChunk.SetDepo(new Depo(thisChunk, DepoType.Standard));
        return true;
      }
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
              if (starCheckKey != thisChunk.Key)              {
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
        int shouldGenerateStar = Random.Range(0, 1000);
        if (shouldGenerateStar > starSpawnChance) return false;   //8% chance

        int starTypeGenerator = Random.Range(0, 1000);

        float originDistance = FastDistance(thisChunk.Position, Vector2.zero);

        //WhiteDwarf - BrownDwarf
        if (originDistance >= starMinDistance1)
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

    private Vector2 lastRandPosition = Vector2.zero;
    private Vector2 lastOrbitPosition = Vector2.zero;
    private void GenerateAsteroids(Chunk chunk, bool hasStar = false, bool hasDepo = false)
    {
      //if (chunk.Key == Vector2.zero) return;

      int minimum = Random.Range(minAsteroids - Random.Range(0, 25), minAsteroids + Random.Range(0, 10));
      int maximum = Random.Range(maxAsteroids - Random.Range(5, 30), maxAsteroids + Random.Range(0, 15));
      int asteroidCount = Random.Range(minimum, maximum);

      var spawnPoint = Vector2.zero;

      if (hasStar == true)   //Generate around star or depo
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
        int randomSize = Random.Range(0, 1000);

        if (randomSize <= 450) return AsteroidSize.Small;
        else if (randomSize <= 750) return AsteroidSize.Medium;
        else if (randomSize <= 990) return AsteroidSize.Large;
        else return AsteroidSize.Huge;
      }

      AsteroidType GetRandomType(Vector2 _position)
      {
        int gen = Random.Range(0, 100);

        int originDistance = (int)FastDistance(_position, Vector2.zero);

        if (originDistance >= asteroidMinDistance1 && originDistance <= asteroidMinDistance2)
        {
          if (gen <= 50) return AsteroidType.Iron;
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
          if (gen <= 20) return AsteroidType.Iron;
          else if (gen <= 35) return AsteroidType.Platinum;
          else if (gen <= 62) return AsteroidType.Titanium;
          else if (gen <= 74) return AsteroidType.Gold;
          else if (gen <= 80) return AsteroidType.Palladium;
          else if (gen <= 85) return AsteroidType.Cobalt;
          else if (gen <= 95) return AsteroidType.Stellarite;
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
        var randomPosition = new Vector2(
          Random.Range(chunkBounds.min.x, chunkBounds.max.x),
          Random.Range(chunkBounds.min.y, chunkBounds.max.y)
        );

        int randomNum = Random.Range(0, 7);

        if (randomNum == 0) 
        {
          return randomPosition;
        }
        else if (randomNum <= 3)
        {
          randomPosition.x = (lastRandPosition.x + randomPosition.x) / 2;
          randomPosition.y = (lastRandPosition.y + randomPosition.y) / 2;
        } 
        else if(randomNum <= 6) 
        {
          randomPosition.x = lastRandPosition.x + Random.Range(-40, 40);
          randomPosition.y = lastRandPosition.y + Random.Range(-40, 40);
        }

        lastRandPosition = randomPosition;
        return randomPosition;
      }

      Vector2 GetPositionAroundStar(Star orbitingStar)
      {
        var randomAngle = Random.Range(0f, 2 * Mathf.PI);
        Vector2 starPosition = orbitingStar.SpawnPoint;
        float orbitRadius;

        float asteroidBeltProbability = 0.88f; // 20% chance for an asteroid to be in the belt
        if (Random.value < asteroidBeltProbability)
        {
            // Place asteroid in the asteroid belt
            orbitRadius = (orbitingStar.AsteroidBeltRadius.x + orbitingStar.AsteroidBeltRadius.y) / Random.Range(1.5f, 2.2f); // Midpoint of the defined asteroid belt range
        }
        else
        {
            // Place asteroid at a random distance
            orbitRadius = Random.Range(
              orbitingStar.AsteroidBeltRadius.x,
              orbitingStar.AsteroidBeltRadius.y
            );
        }

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