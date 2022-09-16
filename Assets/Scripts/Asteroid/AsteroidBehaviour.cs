using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public enum AsteroidSize
  {
    Pickup,
    Small,
    Medium,
    Large,
    ExtraLarge
  }

  public enum AsteroidType
  {
    Iron,
    Platinum,
    Gold,
    Palladium,
    Stellarite,
    Darkore,
    Cobalt
  }

  [CreateAssetMenu(fileName = "AsteroidBehaviour", menuName = "ScriptableObjects/AsteroidBehaviour", order = 1)]
  public class AsteroidBehaviour : ScriptableObject
  {
    private Tuple<AsteroidType, AsteroidSize, int> thisAsteroidProperties;

    //----------------------------------------------------------------------------------------------
  
    //Prefabs with variants
    private GameObject asteroidPickupPrefab;
    private GameObject smallAsteroidPrefab;
    private GameObject mediumAsteroidPrefab;
    private GameObject largeAsteroidPrefab;
    private GameObject extraLargeAsteroidPrefab;

    //----------------------------------------------------------------------------------------------

    private void Awake()
    {
      //Grab our prefabs from resources folder
      smallAsteroidPrefab = Resources.Load("Prefabs/Asteroid-S") as GameObject;
      mediumAsteroidPrefab = Resources.Load("Prefabs/Asteroid-M") as GameObject;
      largeAsteroidPrefab = Resources.Load("Prefabs/Asteroid-L") as GameObject;
      extraLargeAsteroidPrefab = Resources.Load("Prefabs/Asteroid-XL") as GameObject; 
      asteroidPickupPrefab = Resources.Load("Prefabs/AsteroidPickup") as GameObject;
    }

    public AsteroidInformation SetHealth(AsteroidInformation initAsteroidInformation)
    {
      var newAsteroidInformation = initAsteroidInformation;

      switch (initAsteroidInformation.Size)
      {
        case AsteroidSize.ExtraLarge:
          newAsteroidInformation.Health = 5; 
          break;
        case AsteroidSize.Large:
          newAsteroidInformation.Health = 4;
          break;
        case AsteroidSize.Medium:
          newAsteroidInformation.Health = 3;
          break;
        case AsteroidSize.Small:
          newAsteroidInformation.Health = 2;
          break;
        case AsteroidSize.Pickup:
          newAsteroidInformation.Health = 1;
          break;
      }

      return newAsteroidInformation;
    }

    public int TakeDamage(AsteroidInformation asteroidInfo, int damage, Vector2 hitVector)
    {
      var health = asteroidInfo.Health - damage;

      if (health <= 0)
      {
        SplitAsteroid(asteroidInfo);
        return health;
      }

      CreateRubbleOnDamage(asteroidInfo, hitVector);
      return health;
    }

    private void SplitAsteroid(AsteroidInformation asteroidInfo)
    {
      //If we're a pickup, ignore
      if (asteroidInfo.Size == AsteroidSize.Pickup) return;

      //Parent asteroid components
      var parentType = asteroidInfo.Type;
      var parentSize = asteroidInfo.Size;
      Vector2 parentVelocity = asteroidInfo.Rigid2D.velocity;
      Collider2D asteroidCollider = asteroidInfo.Collider;
      Bounds parentAsteroidBounds = asteroidInfo.Collider.bounds;
      Vector2 parentBoundsSize = parentAsteroidBounds.size;

      //New asteroid components
      AsteroidController newAsteroidController;
      GameObject newAsteroid;
      Rigidbody2D newRigid2D;

      Vector2 lastRandomBoundsPoistion;
      Vector2 newRandomBoundsPosition;

      //Spawn 2 children
      for (int i = 0; i < 2; i++)
      {
        //Get a random position in the parent asteroid bounds
        newRandomBoundsPosition = new Vector2(
            UnityEngine.Random.Range(parentAsteroidBounds.min.x, parentAsteroidBounds.max.x),
            UnityEngine.Random.Range(parentAsteroidBounds.min.y, parentAsteroidBounds.max.y));
            
        lastRandomBoundsPoistion = newRandomBoundsPosition;

        var newAsteroidInformation = new AsteroidInformation();
        newAsteroidInformation.Type = parentType;
        newAsteroidInformation.Position = newRandomBoundsPosition;

        switch (parentSize)
        {
          case AsteroidSize.ExtraLarge:
            newAsteroid = Instantiate(largeAsteroidPrefab, newRandomBoundsPosition, Quaternion.identity);
            newAsteroidController = newAsteroid.GetComponent<AsteroidController>();

            newAsteroidInformation.Size = AsteroidSize.Large; 
            newAsteroidController.Init(newAsteroidInformation);

            SetRandomVelocity();
            break;

          case AsteroidSize.Large:
            newAsteroid = Instantiate(mediumAsteroidPrefab, newRandomBoundsPosition, Quaternion.identity);
            newAsteroidController = newAsteroid.GetComponent<AsteroidController>();
            
            newAsteroidInformation.Size = AsteroidSize.Medium; 
            newAsteroidController.Init(newAsteroidInformation);

            SetRandomVelocity();
            break;

          case AsteroidSize.Medium:
            newAsteroid = Instantiate(smallAsteroidPrefab, newRandomBoundsPosition, Quaternion.identity);
            newAsteroidController = newAsteroid.GetComponent<AsteroidController>();
            
            newAsteroidInformation.Size = AsteroidSize.Small; 
            newAsteroidController.Init(newAsteroidInformation);

            SetRandomVelocity();
            break;

          case AsteroidSize.Small:
            newAsteroid = Instantiate(asteroidPickupPrefab, newRandomBoundsPosition, Quaternion.identity);
            newAsteroidController = newAsteroid.GetComponent<AsteroidController>();
            
            newAsteroidInformation.Size = AsteroidSize.Pickup; 
            newAsteroidController.Init(newAsteroidInformation);

            SetRandomVelocity();
            break;
        }

        void SetRandomVelocity()
        {
          newRigid2D = newAsteroidController.AsteroidRigid2D;

          newRigid2D.velocity = parentVelocity;

          newRigid2D.AddForce(new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)),
            ForceMode2D.Impulse);

          newRigid2D.AddTorque(UnityEngine.Random.Range(-1f, 1f), ForceMode2D.Impulse);
        }
      }
    }

    private void CreateRubbleOnDamage(AsteroidInformation parentAsteroidInfo, Vector2 hitVector)
    {
      //Create rubble based on size
      switch (parentAsteroidInfo.Size)
      {
        case AsteroidSize.Small:
          SmallAsteroidRubble(parentAsteroidInfo.Type, hitVector);
          break;
        case AsteroidSize.Medium:
          MediumAsteroidRubble(parentAsteroidInfo.Type, hitVector);
          break;
        case AsteroidSize.Large:
          LargeAsteroidRubble(parentAsteroidInfo.Type, hitVector);
          break;
        case AsteroidSize.ExtraLarge:
          ExtraLargeAsteroidRubble(parentAsteroidInfo.Type, hitVector);
          break;
      }
    }

    private void SmallAsteroidRubble(AsteroidType parentType, Vector2 hitVector)
    {
      //Asteroid pickup chance
      int pickupChance = UnityEngine.Random.Range(0, 6);
      if(pickupChance == 0) return;

      //Number of pickups chance
      for (int i = 0; i < UnityEngine.Random.Range(1, 1); i++)
      {
        var pickup = Instantiate(asteroidPickupPrefab, hitVector, Quaternion.identity);

        var asteroidRubbleInfo = new AsteroidInformation();
        asteroidRubbleInfo.Type = parentType;
        asteroidRubbleInfo.Size = AsteroidSize.Medium;
        asteroidRubbleInfo.Position = hitVector;

        pickup.GetComponent<AsteroidController>().Init(asteroidRubbleInfo);
      }
    }

    private void MediumAsteroidRubble(AsteroidType parentType, Vector2 hitVector)
    {
      //Asteroid pickup chance
      int pickupChance = UnityEngine.Random.Range(0, 5);
      if (pickupChance == 0)
      {
        //Number of pickups chance
        for (int i = 0; i < UnityEngine.Random.Range(1, 1); i++)
        {
          var pickup = Instantiate(asteroidPickupPrefab, hitVector, Quaternion.identity);
          
          var asteroidRubbleInfo = new AsteroidInformation();
          asteroidRubbleInfo.Type = parentType;
          asteroidRubbleInfo.Size = AsteroidSize.Medium;
          asteroidRubbleInfo.Position = hitVector;

          pickup.GetComponent<AsteroidController>().Init(asteroidRubbleInfo);
        }
      }
    }

    private void LargeAsteroidRubble(AsteroidType parentType, Vector2 hitVector)
    {
      //Asteroid pickup chance
      int pickupChance = UnityEngine.Random.Range(0, 4);
      if (pickupChance == 0)
      {
        //Number of pickups chance
        for (int i = 0; i < UnityEngine.Random.Range(1, 3); i++)
        {
          var pickup = Instantiate(asteroidPickupPrefab, hitVector, Quaternion.identity);
          
          var asteroidRubbleInfo = new AsteroidInformation();
          asteroidRubbleInfo.Type = parentType;
          asteroidRubbleInfo.Size = AsteroidSize.Medium;
          asteroidRubbleInfo.Position = hitVector;

          pickup.GetComponent<AsteroidController>().Init(asteroidRubbleInfo);
        }
      }

      //Small asteroids chance
      int smallAsteroidChance = UnityEngine.Random.Range(0, 6);
      if (smallAsteroidChance == 0)
      {
        //Number of small asteroids chance
        for (int i = 0; i < UnityEngine.Random.Range(1, 2); i++)
        {
          var smallAsteroid = Instantiate(smallAsteroidPrefab, hitVector, Quaternion.identity);
          
          var asteroidRubbleInfo = new AsteroidInformation();
          asteroidRubbleInfo.Type = parentType;
          asteroidRubbleInfo.Size = AsteroidSize.Small;
          asteroidRubbleInfo.Position = hitVector;

          smallAsteroid.GetComponent<AsteroidController>().Init(asteroidRubbleInfo);
        }
      }
    }

    private void ExtraLargeAsteroidRubble(AsteroidType parentType, Vector2 hitVector)
    {
      //Asteroid pickup chance
      int pickupChance = UnityEngine.Random.Range(0, 4);
      if (pickupChance == 0)
      {
        //Number of pickups chance
        for (int i = 0; i < UnityEngine.Random.Range(1, 3); i++)
        {
          var pickup = Instantiate(asteroidPickupPrefab, hitVector, Quaternion.identity);
          
          var asteroidRubbleInfo = new AsteroidInformation();
          asteroidRubbleInfo.Type = parentType;
          asteroidRubbleInfo.Size = AsteroidSize.Pickup;
          asteroidRubbleInfo.Position = hitVector;

          pickup.GetComponent<AsteroidController>().Init(asteroidRubbleInfo);
        }
      }

      //Small asteroids chance
      int smallAsteroidChance = UnityEngine.Random.Range(0, 5);
      if (smallAsteroidChance == 0)
      {
        //Number of small asteroids chance
        for (int i = 0; i < UnityEngine.Random.Range(1, 2); i++)
        {
          var smallAsteroid = Instantiate(smallAsteroidPrefab, hitVector, Quaternion.identity);
          
          var asteroidRubbleInfo = new AsteroidInformation();
          asteroidRubbleInfo.Type = parentType;
          asteroidRubbleInfo.Size = AsteroidSize.Small;
          asteroidRubbleInfo.Position = hitVector;

          smallAsteroid.GetComponent<AsteroidController>().Init(asteroidRubbleInfo);
        }
      }

      //Medium asteroids chance
      int mediumAsteroidChance = UnityEngine.Random.Range(0, 6);
      if (mediumAsteroidChance == 0)
      {
        //Number of medium asteroids chance
        for (int i = 0; i < UnityEngine.Random.Range(1, 2); i++)
        {
          var mediumAsteroid = Instantiate(mediumAsteroidPrefab, hitVector, Quaternion.identity);
          
          var asteroidRubbleInfo = new AsteroidInformation();
          asteroidRubbleInfo.Type = parentType;
          asteroidRubbleInfo.Size = AsteroidSize.Medium;
          asteroidRubbleInfo.Position = hitVector;

          mediumAsteroid.GetComponent<AsteroidController>().Init(asteroidRubbleInfo);
        }
      }
    }
  }
}