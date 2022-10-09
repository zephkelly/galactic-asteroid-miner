using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class AsteroidBehaviour : MonoBehaviour
  {
    #region Asteroid sprites
    //Iron
    private Sprite asteroidIron1;
    private Sprite asteroidIron2;
    private Sprite asteroidIron3;

    //Platinum
    private Sprite asteroidPlatinum1;
    private Sprite asteroidPlatinum2;
    private Sprite asteroidPlatinum3;

    //Titanium
    private Sprite asteroidTitanium1;
    private Sprite asteroidTitanium2;
    private Sprite asteroidTitanium3;

    //Gold
    private Sprite asteroidGold1;
    private Sprite asteroidGold2;
    private Sprite asteroidGold3;

    //Palladium
    private Sprite asteroidPalladium1;
    private Sprite asteroidPalladium2;
    private Sprite asteroidPalladium3;

    //Cobalt
    private Sprite asteroidCobalt1;
    private Sprite asteroidCobalt2;
    private Sprite asteroidCobalt3;

    //Stellarite
    private Sprite asteroidStellarite1;
    private Sprite asteroidStellarite2;
    private Sprite asteroidStellarite3;

    //Darkore
    private Sprite asteroidDarkore1;
    private Sprite asteroidDarkore2;
    private Sprite asteroidDarkore3;
    #endregion

    //------------------------------------------------------------------------------

    private void Awake()
    {
      asteroidIron1 = Resources.Load<Sprite>("Sprites/Asteroids/Iron/Iron_Asteroid_1");
      asteroidIron2 = Resources.Load<Sprite>("Sprites/Asteroids/Iron/Iron_Asteroid_1_2");
      asteroidIron3 = Resources.Load<Sprite>("Sprites/Asteroids/Iron/Iron_Asteroid_1_3");

      asteroidPlatinum1 = Resources.Load<Sprite>("Sprites/Asteroids/Platinum/Platinum_Asteroid_1");
      asteroidPlatinum2 = Resources.Load<Sprite>("Sprites/Asteroids/Platinum/Platinum_Asteroid_1_2");
      asteroidPlatinum3 = Resources.Load<Sprite>("Sprites/Asteroids/Platinum/Platinum_Asteroid_1_3");

      asteroidTitanium1 = Resources.Load<Sprite>("Sprites/Asteroids/Titanium/Titanium_Asteroid_1");
      asteroidTitanium2 = Resources.Load<Sprite>("Sprites/Asteroids/Titanium/Titanium_Asteroid_1_2");
      asteroidTitanium3 = Resources.Load<Sprite>("Sprites/Asteroids/Titanium/Titanium_Asteroid_1_3");

      asteroidGold1 = Resources.Load<Sprite>("Sprites/Asteroids/Gold/Gold_Asteroid_1");
      asteroidGold2 = Resources.Load<Sprite>("Sprites/Asteroids/Gold/Gold_Asteroid_1_2");
      asteroidGold3 = Resources.Load<Sprite>("Sprites/Asteroids/Gold/Gold_Asteroid_1_3");

      asteroidPalladium1 = Resources.Load<Sprite>("Sprites/Asteroids/Palladium/Palladium_Asteroid_1");
      asteroidPalladium2 = Resources.Load<Sprite>("Sprites/Asteroids/Palladium/Palladium_Asteroid_1_2");
      asteroidPalladium3 = Resources.Load<Sprite>("Sprites/Asteroids/Palladium/Palladium_Asteroid_1_3");

      asteroidCobalt1 = Resources.Load<Sprite>("Sprites/Asteroids/Cobalt/Cobalt_Asteroid_1");
      asteroidCobalt2 = Resources.Load<Sprite>("Sprites/Asteroids/Cobalt/Cobalt_Asteroid_1_2");
      asteroidCobalt3 = Resources.Load<Sprite>("Sprites/Asteroids/Cobalt/Cobalt_Asteroid_1_3");

      asteroidStellarite1 = Resources.Load<Sprite>("Sprites/Asteroids/Stellarite/Stellarite_Asteroid_1");
      asteroidStellarite2 = Resources.Load<Sprite>("Sprites/Asteroids/Stellarite/Stellarite_Asteroid_1_2");
      asteroidStellarite3 = Resources.Load<Sprite>("Sprites/Asteroids/Stellarite/Stellarite_Asteroid_1_3");

      asteroidDarkore1 = Resources.Load<Sprite>("Sprites/Asteroids/Darkore/Darkore_Asteroid_1");
      asteroidDarkore2 = Resources.Load<Sprite>("Sprites/Asteroids/Darkore/Darkore_Asteroid_1_2");
      asteroidDarkore3 = Resources.Load<Sprite>("Sprites/Asteroids/Darkore/Darkore_Asteroid_1_3");
    }

    public int TakeDamage(Asteroid parentAsteroidInfo, int damage, Vector2 hitVector)
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

    private void SplitAsteroid(Asteroid parentAsteroidInfo)
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
      Asteroid newAsteroidInfo;
      AsteroidSize newSize;
      GameObject newAsteroid;
      Vector2 newRandomBoundsPosition;

      Chunk parentChunk = parentAsteroid.ParentChunk;
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

            newAsteroidInfo = CreateAsteroidInfo();
            newAsteroid = ChunkManager.Instance.Instantiator.GetAsteroid(newAsteroidInfo);
            newAsteroidInfo.SetObject(newAsteroid);
            break;

          case AsteroidSize.Large:
            newSize = AsteroidSize.Medium;

            newAsteroidInfo = CreateAsteroidInfo();
            newAsteroid = ChunkManager.Instance.Instantiator.GetAsteroid(newAsteroidInfo);
            newAsteroidInfo.SetObject(newAsteroid);
            break;

          case AsteroidSize.Medium:
            newSize = AsteroidSize.Small;

            newAsteroidInfo = CreateAsteroidInfo();
            newAsteroid = ChunkManager.Instance.Instantiator.GetAsteroid(newAsteroidInfo);
            newAsteroidInfo.SetObject(newAsteroid);
            break;

          case AsteroidSize.Small:
            newSize = AsteroidSize.Pickup;

            newAsteroidInfo = CreateAsteroidInfo();
            newAsteroid = ChunkManager.Instance.Instantiator.GetAsteroid(newAsteroidInfo);
            newAsteroidInfo.SetObject(newAsteroid);
            break;
        }

        Asteroid CreateAsteroidInfo()
        {
          var asteroidInfo = new Asteroid(
            parentChunk,
            newSize,
            rubbleType,
            newRandomBoundsPosition,
            GetHealth(newSize)
          );

          OcclusionManager.Instance.AddAsteroid.Add(asteroidInfo, parentChunk);
          return asteroidInfo;
        }
      }
    }

    private void CreateRubbleOnDamage(Asteroid parentAsteroidInfo, Vector2 hitVector)
    {
      //Parent asteroid components
      var parentAsteroid = parentAsteroidInfo;
      var parentChunk = parentAsteroid.ParentChunk;
      var rubbleType = parentAsteroid.Type;
      var rubbleSpawn = hitVector;

      //Set below
      GameObject newRubble;
      Asteroid newRubbleInfo;
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

          newRubbleInfo = CreateRubbleInfo();
          newRubble = ChunkManager.Instance.Instantiator.GetAsteroid(newRubbleInfo);
          newRubbleInfo.SetObject(newRubble);
        }
      }

      void MediumAsteroidRubble()
      {
        //Asteroid pickup chance
        int pickupChance = UnityEngine.Random.Range(0, 5);
        if(pickupChance == 0) return;

        //Number of pickups chance
        for (int i = 0; i < UnityEngine.Random.Range(1, 1); i++)
        {
          rubbleSize = AsteroidSize.Pickup;

          newRubbleInfo = CreateRubbleInfo();
          newRubble = ChunkManager.Instance.Instantiator.GetAsteroid(newRubbleInfo);
          newRubbleInfo.SetObject(newRubble);
        }
      }

      void LargeAsteroidRubble()
      {
        //Asteroid pickup chance
        int pickupChance = UnityEngine.Random.Range(0, 4);
        if(pickupChance == 0) return;

        //Number of pickups chance
        for (int i = 0; i < UnityEngine.Random.Range(1, 1); i++)
        {
          rubbleSize = AsteroidSize.Pickup;

          newRubbleInfo = CreateRubbleInfo();
          newRubble = ChunkManager.Instance.Instantiator.GetAsteroid(newRubbleInfo);
          newRubbleInfo.SetObject(newRubble);
        }

        //Small asteroid chance
        int smallAsteroidChance = UnityEngine.Random.Range(0, 6);
        if (smallAsteroidChance == 0)
        {
          //Number of small asteroids chance
          for (int i = 0; i < UnityEngine.Random.Range(1, 2); i++)
          {
            rubbleSize = AsteroidSize.Small;

            newRubbleInfo = CreateRubbleInfo();
            newRubble = ChunkManager.Instance.Instantiator.GetAsteroid(newRubbleInfo);
            newRubbleInfo.SetObject(newRubble);
          }
        }
      }

      void HugeAsteroidRubble()
      {
        //Asteroid pickup chance
        int pickupChance = UnityEngine.Random.Range(0, 4);
        if(pickupChance == 0) return;

        //Number of pickups chance
        for (int i = 0; i < UnityEngine.Random.Range(1, 1); i++)
        {
          rubbleSize = AsteroidSize.Pickup;

          newRubbleInfo = CreateRubbleInfo();
          newRubble = ChunkManager.Instance.Instantiator.GetAsteroid(newRubbleInfo);
          newRubbleInfo.SetObject(newRubble);
        }

        //Small asteroid chance
        int smallAsteroidChance = UnityEngine.Random.Range(0, 8);
        if (smallAsteroidChance == 0)
        {
          //Number of small asteroids chance
          for (int i = 0; i < UnityEngine.Random.Range(1, 2); i++)
          {
            rubbleSize = AsteroidSize.Small;

            newRubbleInfo = CreateRubbleInfo();
            newRubble = ChunkManager.Instance.Instantiator.GetAsteroid(newRubbleInfo);
            newRubbleInfo.SetObject(newRubble);
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

            newRubbleInfo = CreateRubbleInfo();
            newRubble = ChunkManager.Instance.Instantiator.GetAsteroid(newRubbleInfo);
            newRubbleInfo.SetObject(newRubble);
          }
        }
      }

      Asteroid CreateRubbleInfo()
      {
        var asteroidInfo = new Asteroid(
          parentChunk,
          rubbleSize,
          rubbleType,
          rubbleSpawn,
          GetHealth(rubbleSize)
        );

        OcclusionManager.Instance.AddAsteroid.Add(asteroidInfo, parentChunk);
        return asteroidInfo;
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