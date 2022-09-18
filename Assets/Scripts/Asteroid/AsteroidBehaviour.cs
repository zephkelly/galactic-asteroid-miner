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

    public Asteroid SetHealth(Asteroid initAsteroidInfo)
    {
      var newAsteroidInfo = initAsteroidInfo;

      switch (initAsteroidInfo.Size)
      {
        case AsteroidSize.ExtraLarge:
          newAsteroidInfo.Health = 5; 
          break;
        case AsteroidSize.Large:
          newAsteroidInfo.Health = 4;
          break;
        case AsteroidSize.Medium:
          newAsteroidInfo.Health = 3;
          break;
        case AsteroidSize.Small:
          newAsteroidInfo.Health = 2;
          break;
        case AsteroidSize.Pickup:
          newAsteroidInfo.Health = 1;
          break;
      }

      return newAsteroidInfo;
    }

    public int TakeDamage(Asteroid asteroidInfo, int damage, Vector2 hitVector)
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

    private void SplitAsteroid(Asteroid asteroidInfo)
    {
      //If we're a pickup, ignore
      if (asteroidInfo.Size == AsteroidSize.Pickup) return;

      //Parent asteroid components
      var parentAsteroid = asteroidInfo.Type;
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
            Random.Range(parentAsteroidBounds.min.x, parentAsteroidBounds.max.x),
            Random.Range(parentAsteroidBounds.min.y, parentAsteroidBounds.max.y));
            
        lastRandomBoundsPoistion = newRandomBoundsPosition;

        var newAsteroidInfo = new Asteroid();
        newAsteroidInfo.Type = parentAsteroid;
        newAsteroidInfo.SpawnPosition = newRandomBoundsPosition;

        switch (parentSize)
        {
          case AsteroidSize.ExtraLarge:
            newAsteroid = Instantiate(mediumAsteroidPrefab, newRandomBoundsPosition, Quaternion.identity);
            newAsteroidController = newAsteroid.GetComponent<AsteroidController>();

            newAsteroidInfo.Size = AsteroidSize.Medium;
            newAsteroidController.SetAsteroid(newAsteroidInfo, asteroidInfo.ParentChunk);

            SetRandomVelocity();
            break;

          case AsteroidSize.Large:
            newAsteroid = Instantiate(smallAsteroidPrefab, newRandomBoundsPosition, Quaternion.identity);
            newAsteroidController = newAsteroid.GetComponent<AsteroidController>();
            
            newAsteroidInfo.Size = AsteroidSize.Small; 
            newAsteroidController.SetAsteroid(newAsteroidInfo, asteroidInfo.ParentChunk);

            SetRandomVelocity();
            break;

          case AsteroidSize.Medium:
            newAsteroid = Instantiate(smallAsteroidPrefab, newRandomBoundsPosition, Quaternion.identity);
            newAsteroidController = newAsteroid.GetComponent<AsteroidController>();
            
            newAsteroidInfo.Size = AsteroidSize.Small; 
            newAsteroidController.SetAsteroid(newAsteroidInfo, asteroidInfo.ParentChunk);

            SetRandomVelocity();
            break;

          case AsteroidSize.Small:
            newAsteroid = Instantiate(asteroidPickupPrefab, newRandomBoundsPosition, Quaternion.identity);
            newAsteroidController = newAsteroid.GetComponent<AsteroidController>();
            
            newAsteroidInfo.Size = AsteroidSize.Pickup; 
            newAsteroidController.SetAsteroid(newAsteroidInfo, asteroidInfo.ParentChunk);

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

    private void CreateRubbleOnDamage(Asteroid parentAsteroidInfo, Vector2 hitVector)
    {
      //Create rubble based on size
      switch (parentAsteroidInfo.Size)
      {
        case AsteroidSize.Small:
          SmallAsteroidRubble(parentAsteroidInfo, hitVector);
          break;
        case AsteroidSize.Medium:
          MediumAsteroidRubble(parentAsteroidInfo, hitVector);
          break;
        case AsteroidSize.Large:
          LargeAsteroidRubble(parentAsteroidInfo, hitVector);
          break;
        case AsteroidSize.ExtraLarge:
          ExtraLargeAsteroidRubble(parentAsteroidInfo, hitVector);
          break;
      }
    }

    private void SmallAsteroidRubble(Asteroid parentAsteroid, Vector2 hitVector)
    {
      //Asteroid pickup chance
      int pickupChance = UnityEngine.Random.Range(0, 6);
      if(pickupChance == 0) return;

      //Number of pickups chance
      for (int i = 0; i < UnityEngine.Random.Range(1, 1); i++)
      {
        var pickup = Instantiate(asteroidPickupPrefab, hitVector, Quaternion.identity);

        var asteroidRubbleInfo = new Asteroid();
        asteroidRubbleInfo.Type = parentAsteroid.Type;
        asteroidRubbleInfo.Size = AsteroidSize.Medium;
        asteroidRubbleInfo.SpawnPosition = hitVector;

        pickup.GetComponent<AsteroidController>().SetAsteroid(asteroidRubbleInfo, parentAsteroid.ParentChunk);
      }
    }

    private void MediumAsteroidRubble(Asteroid parentAsteroid, Vector2 hitVector)
    {
      //Asteroid pickup chance
      int pickupChance = UnityEngine.Random.Range(0, 5);
      if (pickupChance == 0)
      {
        //Number of pickups chance
        for (int i = 0; i < UnityEngine.Random.Range(1, 1); i++)
        {
          var pickup = Instantiate(asteroidPickupPrefab, hitVector, Quaternion.identity);
          
          var asteroidRubbleInfo = new Asteroid();
          asteroidRubbleInfo.Type = parentAsteroid.Type;
          asteroidRubbleInfo.Size = AsteroidSize.Medium;
          asteroidRubbleInfo.SpawnPosition = hitVector;

          pickup.GetComponent<AsteroidController>().SetAsteroid(asteroidRubbleInfo, parentAsteroid.ParentChunk);
        }
      }
    }

    private void LargeAsteroidRubble(Asteroid parentAsteroid, Vector2 hitVector)
    {
      //Asteroid pickup chance
      int pickupChance = UnityEngine.Random.Range(0, 4);
      if (pickupChance == 0)
      {
        //Number of pickups chance
        for (int i = 0; i < UnityEngine.Random.Range(1, 3); i++)
        {
          var pickup = Instantiate(asteroidPickupPrefab, hitVector, Quaternion.identity);
          
          var asteroidRubbleInfo = new Asteroid();
          asteroidRubbleInfo.Type = parentAsteroid.Type;
          asteroidRubbleInfo.Size = AsteroidSize.Medium;
          asteroidRubbleInfo.SpawnPosition = hitVector;

          pickup.GetComponent<AsteroidController>().SetAsteroid(asteroidRubbleInfo, parentAsteroid.ParentChunk);
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
          
          var asteroidRubbleInfo = new Asteroid();
          asteroidRubbleInfo.Type = parentAsteroid.Type;
          asteroidRubbleInfo.Size = AsteroidSize.Small;
          asteroidRubbleInfo.SpawnPosition = hitVector;

          smallAsteroid.GetComponent<AsteroidController>().SetAsteroid(asteroidRubbleInfo, parentAsteroid.ParentChunk);
        }
      }
    }

    private void ExtraLargeAsteroidRubble(Asteroid parentAsteroid, Vector2 hitVector)
    {
      //Asteroid pickup chance
      int pickupChance = UnityEngine.Random.Range(0, 4);
      if (pickupChance == 0)
      {
        //Number of pickups chance
        for (int i = 0; i < UnityEngine.Random.Range(1, 3); i++)
        {
          var pickup = Instantiate(asteroidPickupPrefab, hitVector, Quaternion.identity);
          
          var asteroidRubbleInfo = new Asteroid();
          asteroidRubbleInfo.Type = parentAsteroid.Type;
          asteroidRubbleInfo.Size = AsteroidSize.Pickup;
          asteroidRubbleInfo.SpawnPosition = hitVector;

          pickup.GetComponent<AsteroidController>().SetAsteroid(asteroidRubbleInfo, parentAsteroid.ParentChunk);
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
          
          var asteroidRubbleInfo = new Asteroid();
          asteroidRubbleInfo.Type = parentAsteroid.Type;
          asteroidRubbleInfo.Size = AsteroidSize.Small;
          asteroidRubbleInfo.SpawnPosition = hitVector;

          smallAsteroid.GetComponent<AsteroidController>().SetAsteroid(asteroidRubbleInfo, parentAsteroid.ParentChunk);
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
          
          var asteroidRubbleInfo = new Asteroid();
          asteroidRubbleInfo.Type = parentAsteroid.Type;
          asteroidRubbleInfo.Size = AsteroidSize.Medium;
          asteroidRubbleInfo.SpawnPosition = hitVector;

          mediumAsteroid.GetComponent<AsteroidController>().SetAsteroid(asteroidRubbleInfo, parentAsteroid.ParentChunk);
        }
      }
    }
  }
}