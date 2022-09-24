using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  [CreateAssetMenu(fileName = "AsteroidBehaviour", menuName = "ScriptableObjects/AsteroidBehaviour")]
  public class AsteroidBehaviour : ScriptableObject
  {
    //Prefabs with variants
    private GameObject asteroidPickupPrefab;
    private GameObject smallAsteroidPrefab;
    private GameObject mediumAsteroidPrefab;
    private GameObject largeAsteroidPrefab;
    private GameObject xLargeAsteroidPrefab;

    //------------------------------------------------------------------------------

    private void Awake()
    {
      //Grab our prefabs from resources folder
      smallAsteroidPrefab = Resources.Load<GameObject>("Prefabs/Asteroids/Asteroid-S");
      mediumAsteroidPrefab = Resources.Load<GameObject>("Prefabs/Asteroids/Asteroid-M");
      largeAsteroidPrefab = Resources.Load<GameObject>("Prefabs/Asteroids/Asteroid-L");
      xLargeAsteroidPrefab = Resources.Load<GameObject>("Prefabs/Asteroids/Asteroid-XL"); 
      asteroidPickupPrefab = Resources.Load<GameObject>("Prefabs/Asteroids/AsteroidPickup");
    }

    public int TakeDamage(Asteroid2 parentAsteroidInfo, int damage, Vector2 hitVector)
    {
      var health = parentAsteroidInfo.Health - damage;

      if (health <= 0)
      {
        SplitAsteroid(parentAsteroidInfo);
        return health;
      }

      CreateRubbleOnDamage(parentAsteroidInfo, hitVector);
      return health;
    }

    private void SplitAsteroid(Asteroid2 parentAsteroidInfo)
    {
      //If we're a pickup, ignore
      if (parentAsteroidInfo.Size == AsteroidSize.Pickup) return;

      //Parent asteroid components
      var parentAsteroid = parentAsteroidInfo;
      var parentSize = parentAsteroidInfo.Size;
      Vector2 parentVelocity = parentAsteroidInfo.AttachedRigid.velocity;
      Collider2D asteroidCollider = parentAsteroidInfo.AttachedCollider;
      Bounds parentAsteroidBounds = asteroidCollider.bounds;
      Vector2 parentBoundsSize = parentAsteroidBounds.size;

      //New asteroid components
      Asteroid2 newAsteroidInfo;
      AsteroidSize newSize;
      GameObject newAsteroid;
      Vector2 newRandomBoundsPosition;

      Chunk2 parentChunk = parentAsteroid.ParentChunk;
      AsteroidType rubbleType = parentAsteroid.Type;

      //Spawn 2 children
      for (int i = 0; i < 2; i++)
      {
        //Get a random position in the parent asteroid bounds
        newRandomBoundsPosition = new Vector2(
            Random.Range(parentAsteroidBounds.min.x, parentAsteroidBounds.max.x),
            Random.Range(parentAsteroidBounds.min.y, parentAsteroidBounds.max.y));

        switch (parentSize)
        {
          case AsteroidSize.ExtraLarge:
            newSize = AsteroidSize.Large;

            newAsteroid = Instantiate(largeAsteroidPrefab, newRandomBoundsPosition, Quaternion.identity);
            newAsteroidInfo = CreateAsteroidInfo();

            newAsteroidInfo.SetObject(newAsteroid, newRandomBoundsPosition);

            newAsteroid.GetComponent<AsteroidController>().SetAsteroidInfo(newAsteroidInfo);
            break;

          case AsteroidSize.Large:
            newSize = AsteroidSize.Medium;

            newAsteroid = Instantiate(mediumAsteroidPrefab, newRandomBoundsPosition, Quaternion.identity);
            newAsteroidInfo = CreateAsteroidInfo();

            newAsteroidInfo.SetObject(newAsteroid, newRandomBoundsPosition);

            newAsteroid.GetComponent<AsteroidController>().SetAsteroidInfo(newAsteroidInfo);
            break;

          case AsteroidSize.Medium:
            newSize = AsteroidSize.Small;

            newAsteroid = Instantiate(smallAsteroidPrefab, newRandomBoundsPosition, Quaternion.identity);
            newAsteroidInfo = CreateAsteroidInfo();

            newAsteroidInfo.SetObject(newAsteroid, newRandomBoundsPosition);

            newAsteroid.GetComponent<AsteroidController>().SetAsteroidInfo(newAsteroidInfo);
            break;

          case AsteroidSize.Small:
            newSize = AsteroidSize.Pickup;

            newAsteroid = Instantiate(asteroidPickupPrefab, newRandomBoundsPosition, Quaternion.identity);
            newAsteroidInfo = CreateAsteroidInfo();

            newAsteroidInfo.SetObject(newAsteroid, newRandomBoundsPosition);

            newAsteroid.GetComponent<AsteroidController>().SetAsteroidInfo(newAsteroidInfo);
            break;
        }

        Asteroid2 CreateAsteroidInfo()
        {
          return new Asteroid2(
            parentChunk,
            newSize,
            rubbleType,
            newRandomBoundsPosition,
            GetHealth(newSize)
          );
        }
      }
    }

    private void CreateRubbleOnDamage(Asteroid2 parentAsteroidInfo, Vector2 hitVector)
    {
      //Parent asteroid components
      var parentAsteroid = parentAsteroidInfo;
      var parentChunk = parentAsteroid.ParentChunk;
      var rubbleType = parentAsteroid.Type;
      var rubbleSpawn = hitVector;

      //Set below
      GameObject newRubble;
      Asteroid2 newRubbleInfo;
      AsteroidSize rubbleSize;

      //Create rubble based on size
      switch (parentAsteroidInfo.Size)
      {
        case AsteroidSize.Small:
          SmallAsteroidRubble();
          break;
        case AsteroidSize.Medium:
          MediumAsteroidRubble();
          break;
        case AsteroidSize.Large:
          LargeAsteroidRubble();
          break;
        case AsteroidSize.Huge:
          HugeAsteroidRubble();
          break;
      }

      //--------------------------------------------------------------------------------

      void SmallAsteroidRubble()
      {
        //Asteroid pickup chance
        int pickupChance = UnityEngine.Random.Range(0, 6);
        if(pickupChance == 0) return;

        //Number of pickups chance
        for (int i = 0; i < UnityEngine.Random.Range(1, 1); i++)
        {
          rubbleSize = AsteroidSize.Pickup;

          newRubble = Instantiate(asteroidPickupPrefab, hitVector, Quaternion.identity);
          newRubbleInfo = CreateRubbleInfo();

          newRubbleInfo.SetObject(newRubble, hitVector);
          newRubble.GetComponent<AsteroidController>().SetAsteroidInfo(newRubbleInfo);
        }
      }

      void MediumAsteroidRubble()
      {
        //Asteroid pickup chance
        int pickupChance = UnityEngine.Random.Range(0, 5);
        if(pickupChance == 0) return;

        //Number of pickups chance
        for (int i = 0; i < UnityEngine.Random.Range(1, 2); i++)
        {
          rubbleSize = AsteroidSize.Pickup;

          newRubble = Instantiate(asteroidPickupPrefab, hitVector, Quaternion.identity);
          newRubbleInfo = CreateRubbleInfo();

          newRubbleInfo.SetObject(newRubble, hitVector);
          newRubble.GetComponent<AsteroidController>().SetAsteroidInfo(newRubbleInfo);
        }
      }

      void LargeAsteroidRubble()
      {
        //Asteroid pickup chance
        int pickupChance = UnityEngine.Random.Range(0, 4);
        if(pickupChance == 0) return;

        //Number of pickups chance
        for (int i = 0; i < UnityEngine.Random.Range(1, 3); i++)
        {
          rubbleSize = AsteroidSize.Pickup;

          newRubble = Instantiate(asteroidPickupPrefab, hitVector, Quaternion.identity);
          newRubbleInfo = CreateRubbleInfo();

          newRubbleInfo.SetObject(newRubble, hitVector);
          newRubble.GetComponent<AsteroidController>().SetAsteroidInfo(newRubbleInfo);
        }

        //Small asteroid chance
        int smallAsteroidChance = UnityEngine.Random.Range(0, 6);
        if (smallAsteroidChance == 0)
        {
          //Number of small asteroids chance
          for (int i = 0; i < UnityEngine.Random.Range(1, 2); i++)
          {
            rubbleSize = AsteroidSize.Small;

            newRubble = Instantiate(smallAsteroidPrefab, hitVector, Quaternion.identity);
            newRubbleInfo = CreateRubbleInfo();

            newRubbleInfo.SetObject(newRubble, hitVector);
            newRubble.GetComponent<AsteroidController>().SetAsteroidInfo(newRubbleInfo);
          }
        }
      }

      void HugeAsteroidRubble()
      {
        //Asteroid pickup chance
        int pickupChance = UnityEngine.Random.Range(0, 4);
        if(pickupChance == 0) return;

        //Number of pickups chance
        for (int i = 0; i < UnityEngine.Random.Range(1, 3); i++)
        {
          rubbleSize = AsteroidSize.Pickup;

          newRubble = Instantiate(asteroidPickupPrefab, hitVector, Quaternion.identity);
          newRubbleInfo = CreateRubbleInfo();

          newRubbleInfo.SetObject(newRubble, hitVector);
          newRubble.GetComponent<AsteroidController>().SetAsteroidInfo(newRubbleInfo);
        }

        //Small asteroid chance
        int smallAsteroidChance = UnityEngine.Random.Range(0, 8);
        if (smallAsteroidChance == 0)
        {
          //Number of small asteroids chance
          for (int i = 0; i < UnityEngine.Random.Range(1, 2); i++)
          {
            rubbleSize = AsteroidSize.Small;

            newRubble = Instantiate(smallAsteroidPrefab, hitVector, Quaternion.identity);
            newRubbleInfo = CreateRubbleInfo();

            newRubbleInfo.SetObject(newRubble, hitVector);
            newRubble.GetComponent<AsteroidController>().SetAsteroidInfo(newRubbleInfo);
          }
        }

        //Medium asteroid chance
        int mediumAsteroidChance = UnityEngine.Random.Range(0, 8);
        if (mediumAsteroidChance == 0)
        {
          //Number of medium asteroids chance
          for (int i = 0; i < UnityEngine.Random.Range(1, 2); i++)
          {
            rubbleSize = AsteroidSize.Medium;

            newRubble = Instantiate(mediumAsteroidPrefab, hitVector, Quaternion.identity);
            newRubbleInfo = CreateRubbleInfo();

            newRubbleInfo.SetObject(newRubble, hitVector);
            newRubble.GetComponent<AsteroidController>().SetAsteroidInfo(newRubbleInfo);
          }
        }
      }

      Asteroid2 CreateRubbleInfo()
      {
        return new Asteroid2(
          parentChunk,
          rubbleSize,
          rubbleType,
          rubbleSpawn,
          GetHealth(rubbleSize)
        );
      }
    }

    private int GetHealth(AsteroidSize _size)
    {
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