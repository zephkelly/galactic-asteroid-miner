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
    private Sprite[] asteroidPickupSprites = new Sprite[2];
    private Sprite[] smallAsteroidSprites = new Sprite[2];
    private Sprite[] mediumAsteroidSprites = new Sprite[2];
    private Sprite[] largeAsteroidSprites = new Sprite[2];
    private Sprite[] extraLargeAsteroidSprites = new Sprite[2];

    //----------------------------------------------------------------------------------------------

    private void Awake()
    {
      //Grab our prefabs from resources folder
      smallAsteroidPrefab = Resources.Load("Prefabs/Asteroid-S") as GameObject;
      smallAsteroidSprites[0] = Resources.Load("Sprites/Asteroid-S-Sprite1") as Sprite;
      smallAsteroidSprites[1] = Resources.Load("Sprites/Asteroid-S-Sprite1") as Sprite;

      mediumAsteroidPrefab = Resources.Load("Prefabs/Asteroid-M") as GameObject;
      mediumAsteroidSprites[0] = Resources.Load("Sprites/Asteroid-S-Sprite1") as Sprite;
      mediumAsteroidSprites[1] = Resources.Load("Sprites/Asteroid-S-Sprite1") as Sprite;

      largeAsteroidPrefab = Resources.Load("Prefabs/Asteroid-L") as GameObject;
      largeAsteroidSprites[0] = Resources.Load("Sprites/Asteroid-S-Sprite1") as Sprite;
      largeAsteroidSprites[1] = Resources.Load("Sprites/Asteroid-S-Sprite1") as Sprite;

      extraLargeAsteroidPrefab = Resources.Load("Prefabs/Asteroid-XL") as GameObject; 
      extraLargeAsteroidSprites[0] = Resources.Load("Sprites/Asteroid-S-Sprite1") as Sprite;
      extraLargeAsteroidSprites[1] = Resources.Load("Sprites/Asteroid-S-Sprite1") as Sprite;

      asteroidPickupPrefab = Resources.Load("Prefabs/AsteroidPickup") as GameObject;
      asteroidPickupSprites[0] = Resources.Load("Sprites/Asteroid-S-Sprite1") as Sprite;
      asteroidPickupSprites[1] = Resources.Load("Sprites/Asteroid-S-Sprite1") as Sprite;
    }

    public Tuple<AsteroidType, AsteroidSize, int> SetProperties(AsteroidType type, AsteroidSize size, GameObject newAsteroid, SpriteRenderer asteroidSpriteRenderer, Rigidbody2D asteroidRigid2D)
    {
      int health = 0;

      //int randomSpriteInt = UnityEngine.Random.Range(0, 2);

      switch (size)
      {
        case AsteroidSize.ExtraLarge:
          health = 5; 
          size = AsteroidSize.ExtraLarge;
          //thisSpriteRenderer.sprite = largeAsteroidSprites[randomSpriteInt];
          break;
        case AsteroidSize.Large:
          health = 4;
          size = AsteroidSize.Large;
          //thisSpriteRenderer.sprite = mediumAsteroidSprites[randomSpriteInt];
          break;
        case AsteroidSize.Medium:
          health = 3;
          size = AsteroidSize.Medium;
          //thisSpriteRenderer.sprite = smallAsteroidSprites[randomSpriteInt];
          break;
        case AsteroidSize.Small:
          health = 2;
          size = AsteroidSize.Small;
          //thisSpriteRenderer.sprite = asteroidPickupSprites[randomSpriteInt];
          break;
        case AsteroidSize.Pickup:
          health = 1;
          size = AsteroidSize.Pickup;
          //thisSpriteRenderer.sprite = asteroidPickupSprites[randomSpriteInt];
          break;
      }

      thisAsteroidProperties = new Tuple<AsteroidType, AsteroidSize, int>(type, size, health);
      return thisAsteroidProperties;
    }

    public int TakeDamage(GameObject asteroidObject, AsteroidType type, AsteroidSize size, int health, int damage, Vector2 hitVector)
    {
      int asteroidHealth = health;
      asteroidHealth -= damage;

      if (asteroidHealth <= 0)
      {
        SplitAsteroid(asteroidObject, type, size);
        return asteroidHealth;
      }

      CreateRubbleOnDamage(asteroidObject, type, size, hitVector);
      return asteroidHealth;
    }

    private void SplitAsteroid(GameObject parentAsteroid, AsteroidType parentType, AsteroidSize parentSize)
    {
      //If we're a pickup, ignore
      if (parentSize == AsteroidSize.Pickup) return;

      AsteroidController newAsteroidController;
      GameObject newAsteroid;
      SpriteRenderer spriteRenderer;
      Rigidbody2D rigid2D;

      //Spawn 2 children
      for (int i = 0; i < 2; i++)
      {
        switch (parentSize)
        {
          case AsteroidSize.ExtraLarge:
            newAsteroid = Instantiate(largeAsteroidPrefab, parentAsteroid.transform.position, Quaternion.identity);
            newAsteroidController = newAsteroid.GetComponent<AsteroidController>();
            newAsteroidController.Init(parentType, AsteroidSize.Large);

            rigid2D = newAsteroidController.AsteroidRigid2D;
            spriteRenderer = newAsteroidController.AsteroidSpriteRenderer;

            rigid2D.AddForce(new Vector2(
              UnityEngine.Random.Range(-1f, 1f),
              UnityEngine.Random.Range(-1f, 1f)),
              ForceMode2D.Impulse);

            rigid2D.AddTorque(UnityEngine.Random.Range(-1f, 1f), ForceMode2D.Impulse);
            break;

          case AsteroidSize.Large:
            newAsteroid = Instantiate(mediumAsteroidPrefab, parentAsteroid.transform.position, Quaternion.identity);
            newAsteroidController = newAsteroid.GetComponent<AsteroidController>();
            newAsteroidController.Init(parentType, AsteroidSize.Medium);

            rigid2D = newAsteroidController.AsteroidRigid2D;
            spriteRenderer = newAsteroidController.AsteroidSpriteRenderer;

            rigid2D.AddForce(new Vector2(
              UnityEngine.Random.Range(-1f, 1f),
              UnityEngine.Random.Range(-1f, 1f)),
              ForceMode2D.Impulse);

            rigid2D.AddTorque(UnityEngine.Random.Range(-1f, 1f), ForceMode2D.Impulse);
            break;

          case AsteroidSize.Medium:
            newAsteroid = Instantiate(smallAsteroidPrefab, parentAsteroid.transform.position, Quaternion.identity);
            newAsteroidController = newAsteroid.GetComponent<AsteroidController>();
            newAsteroidController.Init(parentType, AsteroidSize.Small);

            rigid2D = newAsteroidController.AsteroidRigid2D;
            spriteRenderer = newAsteroidController.AsteroidSpriteRenderer;

            rigid2D.AddForce(new Vector2(
              UnityEngine.Random.Range(-1f, 1f),
              UnityEngine.Random.Range(-1f, 1f)),
              ForceMode2D.Impulse);

            rigid2D.AddTorque(UnityEngine.Random.Range(-1f, 1f), ForceMode2D.Impulse);
            break;

          case AsteroidSize.Small:
            newAsteroid = Instantiate(asteroidPickupPrefab, parentAsteroid.transform.position, Quaternion.identity);
            newAsteroidController = newAsteroid.GetComponent<AsteroidController>();
            newAsteroidController.Init(parentType, AsteroidSize.Pickup);

            rigid2D = newAsteroidController.AsteroidRigid2D;
            spriteRenderer = newAsteroidController.AsteroidSpriteRenderer;

            rigid2D.AddForce(new Vector2(
              UnityEngine.Random.Range(-1f, 1f),
              UnityEngine.Random.Range(-1f, 1f)),
              ForceMode2D.Impulse);

            rigid2D.AddTorque(UnityEngine.Random.Range(-1f, 1f), ForceMode2D.Impulse);
            break;
        }
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
      for (int i = 0; i < UnityEngine.Random.Range(0, 3); i++)
      {
        var pickup = Instantiate(asteroidPickupPrefab, hitVector, Quaternion.identity);
        pickup.GetComponent<AsteroidController>().Init(parentType, AsteroidSize.Pickup);
      }
    }

    private void MediumAsteroidRubble(GameObject parentAsteroid, AsteroidType parentType, Vector2 hitVector)
    {
      //Asteroid pickup chance
      int pickupChance = UnityEngine.Random.Range(0, 6);
      if (pickupChance == 0)
      {
        //Number of pickups chance
        for (int i = 0; i < UnityEngine.Random.Range(0, 2); i++)
        {
          var pickup = Instantiate(asteroidPickupPrefab, hitVector, Quaternion.identity);
          pickup.GetComponent<AsteroidController>().Init(parentType, AsteroidSize.Pickup);
        }
      }
    }

    private void LargeAsteroidRubble(GameObject parentAsteroid, AsteroidType parentType, Vector2 hitVector)
    {
      //Asteroid pickup chance
      int pickupChance = UnityEngine.Random.Range(0, 6);
      if (pickupChance == 0)
      {
        //Number of pickups chance
        for (int i = 0; i < UnityEngine.Random.Range(0, 2); i++)
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
        for (int i = 0; i < UnityEngine.Random.Range(0, 2); i++)
        {
          var smallAsteroid = Instantiate(smallAsteroidPrefab, hitVector, Quaternion.identity);
          smallAsteroid.GetComponent<AsteroidController>().Init(parentType, AsteroidSize.Small);
        }
      }
    }

    private void ExtraLargeAsteroidRubble(GameObject parentAsteroid, AsteroidType parentType, Vector2 hitVector)
    {
      //Asteroid pickup chance
      int pickupChance = UnityEngine.Random.Range(0, 6);
      if (pickupChance == 0)
      {
        //Number of pickups chance
        for (int i = 0; i < UnityEngine.Random.Range(0, 2); i++)
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
        for (int i = 0; i < UnityEngine.Random.Range(0, 2); i++)
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
        for (int i = 0; i < UnityEngine.Random.Range(0, 2); i++)
        {
          var mediumAsteroid = Instantiate(mediumAsteroidPrefab, hitVector, Quaternion.identity);
          mediumAsteroid.GetComponent<AsteroidController>().Init(parentType, AsteroidSize.Medium);
        }
      }
    }
  }
} 