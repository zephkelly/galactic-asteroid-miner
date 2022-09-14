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

    //Array of sprites we randmly assign
    /*
    private Sprite[] asteroidPickupSprites = new Sprite[2];
    private Sprite[] smallAsteroidSprites = new Sprite[2];
    private Sprite[] mediumAsteroidSprites = new Sprite[2];
    private Sprite[] largeAsteroidSprites = new Sprite[2];
    private Sprite[] extraLargeAsteroidSprites = new Sprite[2];
    */

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

    public Tuple<AsteroidType, AsteroidSize, int> SetProperties(AsteroidType type, AsteroidSize size, GameObject newAsteroid, SpriteRenderer asteroidSpriteRenderer, Rigidbody2D asteroidRigid2D)
    {
      int health = 0;

      //int randomSpriteInt = UnityEngine.Random.Range(0, 1);

      switch (size)
      {
        case AsteroidSize.ExtraLarge:
          health = 5; 
          size = AsteroidSize.ExtraLarge;
          //asteroidSpriteRenderer.sprite = largeAsteroidSprites[randomSpriteInt];
          break;
        case AsteroidSize.Large:
          health = 4;
          size = AsteroidSize.Large;
          //asteroidSpriteRenderer.sprite = mediumAsteroidSprites[randomSpriteInt];
          break;
        case AsteroidSize.Medium:
          health = 3;
          size = AsteroidSize.Medium;
          //asteroidSpriteRenderer.sprite = smallAsteroidSprites[randomSpriteInt];
          break;
        case AsteroidSize.Small:
          health = 2;
          size = AsteroidSize.Small;
          //asteroidSpriteRenderer.sprite = asteroidPickupSprites[randomSpriteInt];
          break;
        case AsteroidSize.Pickup:
          health = 1;
          size = AsteroidSize.Pickup;
          //asteroidSpriteRenderer.sprite = asteroidPickupSprites[randomSpriteInt];
          break;
      }

      thisAsteroidProperties = new Tuple<AsteroidType, AsteroidSize, int>(type, size, health);
      return thisAsteroidProperties;
    }

    public int TakeDamage(AsteroidController controller, AsteroidType type, AsteroidSize size, int health, int damage, Vector2 hitVector)
    {
      int asteroidHealth = health;
      asteroidHealth -= damage;

      if (asteroidHealth <= 0)
      {
        SplitAsteroid(controller, type, size);
        return asteroidHealth;
      }

      CreateRubbleOnDamage(controller.gameObject, type, size, hitVector);
      return asteroidHealth;
    }

    private void SplitAsteroid(AsteroidController controller, AsteroidType parentType, AsteroidSize parentSize)
    {
      //If we're a pickup, ignore
      if (parentSize == AsteroidSize.Pickup) return;

      //Parent asteroid components
      Vector2 parentVelocity = controller.AsteroidRigid2D.velocity;
      Collider2D asteroidCollider = controller.AsteroidCollider;
      Bounds parentAsteroidBounds = asteroidCollider.bounds;
      Vector2 parentBoundsSize = parentAsteroidBounds.size;

      //New asteroid components
      AsteroidController newAsteroidController;
      GameObject newAsteroid;
      Bounds newAsteroidBounds = new Bounds();
      SpriteRenderer newSpriteRenderer;
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

        switch (parentSize)
        {
          case AsteroidSize.ExtraLarge:
            newAsteroid = Instantiate(largeAsteroidPrefab, newRandomBoundsPosition, Quaternion.identity);
            newAsteroidController = newAsteroid.GetComponent<AsteroidController>();
            newAsteroidController.Init(parentType, AsteroidSize.Large);
            SetNewAsteroidComponents();
            break;

          case AsteroidSize.Large:
            newAsteroid = Instantiate(mediumAsteroidPrefab, newRandomBoundsPosition, Quaternion.identity);
            newAsteroidController = newAsteroid.GetComponent<AsteroidController>();
            newAsteroidController.Init(parentType, AsteroidSize.Medium);
            SetNewAsteroidComponents();
            break;

          case AsteroidSize.Medium:
            newAsteroid = Instantiate(smallAsteroidPrefab, newRandomBoundsPosition, Quaternion.identity);
            newAsteroidController = newAsteroid.GetComponent<AsteroidController>();
            newAsteroidController.Init(parentType, AsteroidSize.Small);
            SetNewAsteroidComponents();
            break;

          case AsteroidSize.Small:
            newAsteroid = Instantiate(asteroidPickupPrefab, newRandomBoundsPosition, Quaternion.identity);
            newAsteroidController = newAsteroid.GetComponent<AsteroidController>();
            newAsteroidController.Init(parentType, AsteroidSize.Pickup);
            SetNewAsteroidComponents();
            break;
        }

        void SetNewAsteroidComponents()
        {
          newAsteroidBounds = newAsteroidController.AsteroidCollider.bounds;
          newSpriteRenderer = newAsteroidController.AsteroidSpriteRenderer;
          newRigid2D = newAsteroidController.AsteroidRigid2D;

          //newRigid2D.velocity = parentVelocity;

          newRigid2D.AddForce(new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)),
            ForceMode2D.Impulse);
          newRigid2D.AddTorque(UnityEngine.Random.Range(-1f, 1f), ForceMode2D.Impulse);
        }

        if (i == 1) return;   //Save some cpu cycles

        //Avoid clipping by getting new position if new asteroids intersect
        int giveUpCounter = 0;   //If we can't find a position, give up
        do
        {
          newRandomBoundsPosition = new Vector2(
            UnityEngine.Random.Range(parentAsteroidBounds.min.x, parentAsteroidBounds.max.x),
            UnityEngine.Random.Range(parentAsteroidBounds.min.y, parentAsteroidBounds.max.y));

          giveUpCounter++;
          if (giveUpCounter > 5) break;
        }
        while (Vector2.Distance(newRandomBoundsPosition, lastRandomBoundsPoistion) < newAsteroidBounds.size.x);
      }
    }

    private void CreateRubbleOnDamage(GameObject parentAsteroid, AsteroidType parentType, AsteroidSize parentSize, Vector2 hitVector)
    {
      //Create rubble based on size
      switch (parentSize)
      {
        case AsteroidSize.Small:
          SmallAsteroidRubble(parentAsteroid, parentType, hitVector);
          break;
        case AsteroidSize.Medium:
          MediumAsteroidRubble(parentAsteroid, parentType, hitVector);
          break;
        case AsteroidSize.Large:
          LargeAsteroidRubble(parentAsteroid, parentType, hitVector);
          break;
        case AsteroidSize.ExtraLarge:
          ExtraLargeAsteroidRubble(parentAsteroid, parentType, hitVector);
          break;
      }
    }

    private void SmallAsteroidRubble(GameObject parentAsteroid, AsteroidType parentType, Vector2 hitVector)
    {
      //Asteroid pickup chance
      int pickupChance = UnityEngine.Random.Range(0, 6);
      if(pickupChance == 0) return;

      //Number of pickups chance
      for (int i = 0; i < UnityEngine.Random.Range(1, 1); i++)
      {
        var pickup = Instantiate(asteroidPickupPrefab, hitVector, Quaternion.identity);
        pickup.GetComponent<AsteroidController>().Init(parentType, AsteroidSize.Pickup);
      }
    }

    private void MediumAsteroidRubble(GameObject parentAsteroid, AsteroidType parentType, Vector2 hitVector)
    {
      //Asteroid pickup chance
      int pickupChance = UnityEngine.Random.Range(0, 5);
      if (pickupChance == 0)
      {
        //Number of pickups chance
        for (int i = 0; i < UnityEngine.Random.Range(1, 1); i++)
        {
          var pickup = Instantiate(asteroidPickupPrefab, hitVector, Quaternion.identity);
          pickup.GetComponent<AsteroidController>().Init(parentType, AsteroidSize.Pickup);
        }
      }
    }

    private void LargeAsteroidRubble(GameObject parentAsteroid, AsteroidType parentType, Vector2 hitVector)
    {
      //Asteroid pickup chance
      int pickupChance = UnityEngine.Random.Range(0, 4);
      if (pickupChance == 0)
      {
        //Number of pickups chance
        for (int i = 0; i < UnityEngine.Random.Range(1, 3); i++)
        {
          var pickup = Instantiate(asteroidPickupPrefab, hitVector, Quaternion.identity);
          pickup.GetComponent<AsteroidController>().Init(parentType, AsteroidSize.Pickup);
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
          smallAsteroid.GetComponent<AsteroidController>().Init(parentType, AsteroidSize.Small);
        }
      }
    }

    private void ExtraLargeAsteroidRubble(GameObject parentAsteroid, AsteroidType parentType, Vector2 hitVector)
    {
      //Asteroid pickup chance
      int pickupChance = UnityEngine.Random.Range(0, 4);
      if (pickupChance == 0)
      {
        //Number of pickups chance
        for (int i = 0; i < UnityEngine.Random.Range(1, 3); i++)
        {
          var pickup = Instantiate(asteroidPickupPrefab, hitVector, Quaternion.identity);
          pickup.GetComponent<AsteroidController>().Init(parentType, AsteroidSize.Pickup);
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
          smallAsteroid.GetComponent<AsteroidController>().Init(parentType, AsteroidSize.Small);
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
          mediumAsteroid.GetComponent<AsteroidController>().Init(parentType, AsteroidSize.Medium);
        }
      }
    }
  }
}