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
    Cobalt,
    Gold
  }

  public class AsteroidBehaviour : MonoBehaviour
  {
    public Rigidbody2D rigid2D;
    
    private GameObject asteroidPickup;
    private GameObject smallAsteroid;
    private GameObject mediumAsteroid;
    private GameObject largeAsteroid;
    private GameObject extraLargeAsteroid;

    private AsteroidSize asteroidSize;

    [SerializeField] AsteroidType asteroidType;
    [SerializeField] int health;

    public void Awake()
    {
      rigid2D = GetComponent<Rigidbody2D>();

      //Grab our prefabs from resources folder
      asteroidPickup = Resources.Load("Prefabs/AsteroidPickup") as GameObject;

      smallAsteroid = Resources.Load("Prefabs/Asteroid-S") as GameObject;
      mediumAsteroid = Resources.Load("Prefabs/Asteroid-M") as GameObject;
      largeAsteroid = Resources.Load("Prefabs/Asteroid-L") as GameObject;
      extraLargeAsteroid = Resources.Load("Prefabs/Asteroid-XL") as GameObject;
    }

    public void SetAsteroid(AsteroidType type, AsteroidSize size)
    {
      asteroidType = type;
      asteroidSize = size;

      //Set our health based on the size of asteroid
      switch (size)
      {
        case AsteroidSize.Small:
          health = 2;
          break; 
        case AsteroidSize.Medium:
          health = 4;
          break;
        case AsteroidSize.Large:
          health = 6;
          break;
        case AsteroidSize.ExtraLarge:
          health = 8;
          break;
      }

      switch (type)
      {
        case AsteroidType.Iron:
          gameObject.GetComponent<SpriteRenderer>().color = Color.white;
          break;
        case AsteroidType.Cobalt:
          gameObject.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.5f, 1f, 1f);
          break;
        case AsteroidType.Gold:
          gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0.8f, 0.3f, 1f);
          break;
      }
    }

    public void TakeDamage(int damage, Vector3 hitPoint)
    {
      health -= damage;

      if (health <= 0)
      {
        //Either make a pickup or split asteroid into two
        if (asteroidSize == AsteroidSize.Small)
        {
          var numberOfSmallerAsteroids = Random.Range(1, 4);

          for (int i = 0; i < numberOfSmallerAsteroids; i++)
          {
            CreateAsteroidPickup(asteroidType, hitPoint);
          }
          Destroy(gameObject);
        }
        else
        {
          //Make explosion first
          SplitAsteroidIntoSmaller(asteroidSize, hitPoint);
          Destroy(gameObject);
          return;
        }
      }
      else
      {
        //If we arent dead we do normal behaviour
        DamageBehaviourFromSize(hitPoint);
      }
    }

    //When the asteroid is hit we change behaviour based on size
    private void DamageBehaviourFromSize(Vector3 _hitPoint)
    {
      switch (asteroidSize)
      {
        case AsteroidSize.Small:
          var small = Random.Range(1, 6); 
          if (small == 1) CreateAsteroidPickup(asteroidType, _hitPoint);
          break;
        case AsteroidSize.Medium:
          var medium = Random.Range(1, 8);
          if (medium == 1) CreateAsteroidPickup(asteroidType, _hitPoint);
          break;
        case AsteroidSize.Large:
          var large = Random.Range(1, 10);
          if (large == 1) CreateSmallerAsteroid(AsteroidSize.Small, _hitPoint);
          break;
        case AsteroidSize.ExtraLarge:
          var extraLarge = Random.Range(1, 12);
          if (extraLarge == 1) CreateSmallerAsteroid(AsteroidSize.Medium, _hitPoint);
          break;
      }
    }

    private void CreateAsteroidPickup(AsteroidType _asteroidType, Vector2 _hitPoint)
    {
      //Instantiate asteroid pickup
      var pickup = Instantiate(asteroidPickup, _hitPoint, Quaternion.identity);
      var behaviour = pickup.GetComponent<AsteroidPickup>();
      
      behaviour.CreatePickup(_asteroidType);
      behaviour.rigid2D.AddForce(new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f)));
    }

    private void CreateSmallerAsteroid(AsteroidSize _asteroidSize, Vector2 _spawnPoint)
    {
      switch (_asteroidSize)
      {
        case AsteroidSize.ExtraLarge:
          InstantiateNewAsteroid(largeAsteroid, AsteroidSize.Medium, _spawnPoint);
          break;
        case AsteroidSize.Large:
          InstantiateNewAsteroid(mediumAsteroid, AsteroidSize.Small, _spawnPoint);
          break;
        case AsteroidSize.Medium:
          InstantiateNewAsteroid(smallAsteroid, AsteroidSize.Small, _spawnPoint);
          break;
        default:
          break;
      }
    }

    private void SplitAsteroidIntoSmaller(AsteroidSize _asteroidSize, Vector2 _spawnPoint)
    {
      switch (_asteroidSize)
      {
        case AsteroidSize.ExtraLarge:
          InstantiateNewAsteroid(largeAsteroid, AsteroidSize.Large, _spawnPoint);
          InstantiateNewAsteroid(largeAsteroid, AsteroidSize.Large, _spawnPoint);
          break;
        case AsteroidSize.Large:
          InstantiateNewAsteroid(mediumAsteroid, AsteroidSize.Medium, _spawnPoint);
          InstantiateNewAsteroid(mediumAsteroid, AsteroidSize.Medium, _spawnPoint);
          break;
        case AsteroidSize.Medium:
          InstantiateNewAsteroid(smallAsteroid, AsteroidSize.Small, _spawnPoint);
          InstantiateNewAsteroid(smallAsteroid, AsteroidSize.Small, _spawnPoint);
          break;
        default:
          break;
      }
    }

    public void InstantiateNewAsteroid(GameObject _type, AsteroidSize _size, Vector2 _hitPoint)
    {
      //Instantiate asteroid pickup
      var asteroid = Instantiate(_type, _hitPoint, Quaternion.identity);
      var behaviour = asteroid.GetComponent<AsteroidBehaviour>();

      behaviour.SetAsteroid(asteroidType, _size);
      behaviour.rigid2D.AddForce(new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f)));
    }
  }
}