using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace zephkelly
{
  public class PrefabInstantiator : MonoBehaviour
  {
    private GameObject asteroidPickup;
    private GameObject asteroidSmall;
    private GameObject asteroidMedium;
    private GameObject asteroidLarge;
    private GameObject asteroidHuge;

    private ObjectPool<GameObject> asteroidPickupPool;
    private ObjectPool<GameObject> asteroidSmallPool;
    private ObjectPool<GameObject> asteroidMediumPool;
    private ObjectPool<GameObject> asteroidLargePool;
    private ObjectPool<GameObject> asteroidHugePool;


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

    private GameObject depoStation;

    private GameObject whiteDwarf;
    private GameObject brownDwarf;
    private GameObject redDwarf;
    private GameObject yellowDwarf;
    private GameObject blueGiant;
    private GameObject orangeGiant;
    private GameObject redGiant;
    private GameObject blueSuperGiant;
    private GameObject redSuperGiant;
    private GameObject blueHyperGiant;
    private GameObject redHyperGiant;
    private GameObject neutronStar;
    private GameObject blackHole;

    //------------------------------------------------------------------------------

    #region Getters
    //Asteroids
    public GameObject AsteroidSmall { get => asteroidSmall; }
    public GameObject AsteroidMedium { get => asteroidMedium; }
    public GameObject AsteroidLarge { get => asteroidLarge; }
    public GameObject AsteroidHuge { get => asteroidHuge; }

    public GameObject Depo { get => depoStation; }

    //Stars
    public GameObject WhiteDwarf { get => whiteDwarf; }
    public GameObject BrownDwarf { get => brownDwarf; }
    public GameObject RedDwarf { get => redDwarf; }
    public GameObject YellowDwarf { get => yellowDwarf; }
    public GameObject BlueGiant { get => blueGiant; }
    public GameObject OrangeGiant { get => orangeGiant; }
    public GameObject RedGiant { get => redGiant; }
    public GameObject BlueSuperGiant { get => blueSuperGiant; }
    public GameObject RedSuperGiant { get => redSuperGiant; }
    public GameObject BlueHyperGiant { get => blueHyperGiant; }
    public GameObject RedHyperGiant { get => redHyperGiant; }
    public GameObject NeutronStar { get => neutronStar; }
    public GameObject BlackHole { get => blackHole; }
    #endregion

    //------------------------------------------------------------------------------

    private void Awake()
    {
      asteroidPickup = Resources.Load<GameObject>("Prefabs/Asteroids/AsteroidPickup");
      asteroidSmall = Resources.Load<GameObject>("Prefabs/Asteroids/Asteroid-S");
      asteroidMedium = Resources.Load<GameObject>("Prefabs/Asteroids/Asteroid-M");
      asteroidLarge = Resources.Load<GameObject>("Prefabs/Asteroids/Asteroid-L");
      asteroidHuge = Resources.Load<GameObject>("Prefabs/Asteroids/Asteroid-XL");

      #region Asteroid sprites
      //Iron
      asteroidIron1 = Resources.Load<Sprite>("Sprites/Asteroids/Iron/Iron_Asteroid_1");
      asteroidIron2 = Resources.Load<Sprite>("Sprites/Asteroids/Iron/Iron_Asteroid_1_2");
      asteroidIron3 = Resources.Load<Sprite>("Sprites/Asteroids/Iron/Iron_Asteroid_1_3");

      //Platinum
      asteroidPlatinum1 = Resources.Load<Sprite>("Sprites/Asteroids/Platinum/Platinum_Asteroid_1");
      asteroidPlatinum2 = Resources.Load<Sprite>("Sprites/Asteroids/Platinum/Platinum_Asteroid_1_2");
      asteroidPlatinum3 = Resources.Load<Sprite>("Sprites/Asteroids/Platinum/Platinum_Asteroid_1_3");

      //Titanium
      asteroidTitanium1 = Resources.Load<Sprite>("Sprites/Asteroids/Titanium/Titanium_Asteroid_1");
      asteroidTitanium2 = Resources.Load<Sprite>("Sprites/Asteroids/Titanium/Titanium_Asteroid_1_2");
      asteroidTitanium3 = Resources.Load<Sprite>("Sprites/Asteroids/Titanium/Titanium_Asteroid_1_3");

      //Gold
      asteroidGold1 = Resources.Load<Sprite>("Sprites/Asteroids/Gold/Gold_Asteroid_1");
      asteroidGold2 = Resources.Load<Sprite>("Sprites/Asteroids/Gold/Gold_Asteroid_1_2");
      asteroidGold3 = Resources.Load<Sprite>("Sprites/Asteroids/Gold/Gold_Asteroid_1_3");

      //Palladium
      asteroidPalladium1 = Resources.Load<Sprite>("Sprites/Asteroids/Palladium/Palladium_Asteroid_1");
      asteroidPalladium2 = Resources.Load<Sprite>("Sprites/Asteroids/Palladium/Palladium_Asteroid_1_2");
      asteroidPalladium3 = Resources.Load<Sprite>("Sprites/Asteroids/Palladium/Palladium_Asteroid_1_3");

      //Cobalt
      asteroidCobalt1 = Resources.Load<Sprite>("Sprites/Asteroids/Cobalt/Cobalt_Asteroid_1");
      asteroidCobalt2 = Resources.Load<Sprite>("Sprites/Asteroids/Cobalt/Cobalt_Asteroid_1_2");
      asteroidCobalt3 = Resources.Load<Sprite>("Sprites/Asteroids/Cobalt/Cobalt_Asteroid_1_3");

      //Stellarite
      asteroidStellarite1 = Resources.Load<Sprite>("Sprites/Asteroids/Stellarite/Stellarite_Asteroid_1");
      asteroidStellarite2 = Resources.Load<Sprite>("Sprites/Asteroids/Stellarite/Stellarite_Asteroid_1_2");
      asteroidStellarite3 = Resources.Load<Sprite>("Sprites/Asteroids/Stellarite/Stellarite_Asteroid_1_3");

      //Darkore
      asteroidDarkore1 = Resources.Load<Sprite>("Sprites/Asteroids/Darkore/Darkore_Asteroid_1");
      asteroidDarkore2 = Resources.Load<Sprite>("Sprites/Asteroids/Darkore/Darkore_Asteroid_1_2");
      asteroidDarkore3 = Resources.Load<Sprite>("Sprites/Asteroids/Darkore/Darkore_Asteroid_1_3");
      #endregion

      depoStation = Resources.Load<GameObject>("Prefabs/Stations/Depos/DepoStation");

      whiteDwarf = Resources.Load<GameObject>("Prefabs/Stars/WhiteDwarf");
      brownDwarf = Resources.Load<GameObject>("Prefabs/Stars/BrownDwarf");
      redDwarf = Resources.Load<GameObject>("Prefabs/Stars/RedDwarf");
      yellowDwarf = Resources.Load<GameObject>("Prefabs/Stars/YellowDwarf");
      blueGiant = Resources.Load<GameObject>("Prefabs/Stars/BlueGiant");
      orangeGiant = Resources.Load<GameObject>("Prefabs/Stars/OrangeGiant");
      redGiant = Resources.Load<GameObject>("Prefabs/Stars/RedGiant");
      blueSuperGiant = Resources.Load<GameObject>("Prefabs/Stars/BlueSuperGiant");
      redSuperGiant = Resources.Load<GameObject>("Prefabs/Stars/RedSuperGiant");
      blueHyperGiant = Resources.Load<GameObject>("Prefabs/Stars/BlueHyperGiant");
      redHyperGiant = Resources.Load<GameObject>("Prefabs/Stars/RedHyperGiant");
      neutronStar = Resources.Load<GameObject>("Prefabs/Stars/NeutronStar");
      blackHole = Resources.Load<GameObject>("Prefabs/Stars/BlackHole");
    }

    private void Start()
    {
      asteroidPickupPool = new ObjectPool<GameObject>(() =>
      {
        return Instantiate(asteroidPickup);
      }, asteroid =>
      {
        asteroid.SetActive(true);
        asteroid.GetComponent<Collider2D>().isTrigger = false;
      }, asteroid =>
      {
        asteroid.SetActive(false);
        asteroid.transform.position = Vector3.zero;
      }, asteroid =>
      {
        Destroy(asteroid);
      }, true, 100, 200);

      asteroidSmallPool = new ObjectPool<GameObject>(() =>
      {
        return Instantiate(asteroidSmall);
      }, asteroid =>
      {
        asteroid.SetActive(true);
      }, asteroid =>
      {
        asteroid.SetActive(false);
        asteroid.transform.position = Vector3.zero;
      }, asteroid =>
      {
        Destroy(asteroid);
      }, true, 100, 200);

      asteroidMediumPool = new ObjectPool<GameObject>(() =>
      {
        return Instantiate(asteroidMedium);
      }, asteroid =>
      {
        asteroid.SetActive(true);
      }, asteroid =>
      {
        asteroid.SetActive(false);
        asteroid.transform.position = Vector3.zero;
      }, asteroid =>
      {
        Destroy(asteroid);
      }, true, 100, 200);

      asteroidLargePool = new ObjectPool<GameObject>(() =>
      {
        return Instantiate(asteroidLarge);
      }, asteroid =>
      {
        asteroid.SetActive(true);
      }, asteroid =>
      {
        asteroid.SetActive(false);
        asteroid.transform.position = Vector3.zero;
      }, asteroid =>
      {
        Destroy(asteroid);
      }, true, 100, 200);

      asteroidHugePool = new ObjectPool<GameObject>(() =>
      {
        return Instantiate(asteroidHuge);
      }, asteroid =>
      {
        asteroid.SetActive(true);
      }, asteroid =>
      {
        asteroid.SetActive(false);
        asteroid.transform.position = Vector3.zero;
      }, asteroid =>
      {
        Destroy(asteroid);
      }, true, 100, 200);
    }

    public GameObject GetDepo(Depo _depoInfo)
    {
      //Switch statement for depo type
      var depo = GameObject.Instantiate(Depo);
      depo.transform.position = _depoInfo.SpawnPoint;
      return depo;
    }

    public GameObject GetStar(Star _starInfo)
    {
      Star starInfo = _starInfo;

      switch (starInfo.Type)
      {
        case StarType.WhiteDwarf:
          return GameObject.Instantiate(WhiteDwarf);
        case StarType.BrownDwarf:
          return GameObject.Instantiate(BrownDwarf);
        case StarType.RedDwarf:
          return GameObject.Instantiate(RedDwarf);
        case StarType.YellowDwarf:
          return GameObject.Instantiate(YellowDwarf);
        case StarType.BlueGiant:
          return GameObject.Instantiate(BlueGiant);
        case StarType.OrangeGiant:
          return GameObject.Instantiate(OrangeGiant);
        case StarType.RedGiant:
          return GameObject.Instantiate(RedGiant);
        case StarType.BlueSuperGiant:
          return GameObject.Instantiate(BlueSuperGiant);
        case StarType.RedSuperGiant:
          return GameObject.Instantiate(RedSuperGiant);
        case StarType.BlueHyperGiant:
          return GameObject.Instantiate(BlueHyperGiant);
        case StarType.RedHyperGiant:
          return GameObject.Instantiate(RedHyperGiant);
        case StarType.NeutronStar:
          return GameObject.Instantiate(NeutronStar);
        case StarType.BlackHole:
          return GameObject.Instantiate(BlackHole);
        default:
          Debug.LogError("Star type not found");
          return null;
      }
    }

    public GameObject GetAsteroid(Asteroid _asteroidInfo)
    {
      GameObject newAsteroid = null;

      switch (_asteroidInfo.Size)
      {
        case AsteroidSize.Pickup:
          newAsteroid = asteroidPickupPool.Get();
          break;
        case AsteroidSize.Small:
          newAsteroid = asteroidSmallPool.Get();
          break;
        case AsteroidSize.Medium:
          newAsteroid = asteroidMediumPool.Get();
          break;
        case AsteroidSize.Large:
          newAsteroid = asteroidLargePool.Get();
          break;
        case AsteroidSize.Huge:
          newAsteroid = asteroidHugePool.Get();
          break;
        default:
          Debug.LogError("Asteroid size not found");
          break;
      }

      var spriteRenderer = newAsteroid.GetComponentInChildren<SpriteRenderer>();
      spriteRenderer.sprite = GetAsteroidSprite(_asteroidInfo.Type);
      spriteRenderer.enabled = true;

      return newAsteroid;
    }

    public void ReturnAsteroid(Asteroid _asteroidInfo)
    {
      switch(_asteroidInfo.Size)
      {
        case AsteroidSize.Pickup:
          if (_asteroidInfo.AttachedObject == null) break;
          asteroidPickupPool.Release(_asteroidInfo.AttachedObject);
          break;
        case AsteroidSize.Small:
          asteroidSmallPool.Release(_asteroidInfo.AttachedObject);
          break;
        case AsteroidSize.Medium:
          asteroidMediumPool.Release(_asteroidInfo.AttachedObject);
          break;
        case AsteroidSize.Large:
          asteroidLargePool.Release(_asteroidInfo.AttachedObject);
          break;
        case AsteroidSize.Huge:
          asteroidHugePool.Release(_asteroidInfo.AttachedObject);
          break;
        default:
          Debug.LogError("Asteroid size not found");
          break;
      }
    }

    private Sprite GetAsteroidSprite(AsteroidType type)
    {
      switch (type)
      {
        case AsteroidType.Iron:
          return GetRandomAsteroidSprite(asteroidIron1, asteroidIron2, asteroidIron3);
        case AsteroidType.Titanium:
          return GetRandomAsteroidSprite(asteroidTitanium1, asteroidTitanium2, asteroidTitanium3);
        case AsteroidType.Platinum:
          return GetRandomAsteroidSprite(asteroidPlatinum1, asteroidPlatinum2, asteroidPlatinum3);
        case AsteroidType.Gold:
          return GetRandomAsteroidSprite(asteroidGold1, asteroidGold2, asteroidGold3);
        case AsteroidType.Palladium:
          return GetRandomAsteroidSprite(asteroidPalladium1, asteroidPalladium2, asteroidPalladium3);
        case AsteroidType.Cobalt:
          return GetRandomAsteroidSprite(asteroidCobalt1, asteroidCobalt2, asteroidCobalt3);
        case AsteroidType.Stellarite:
          return GetRandomAsteroidSprite(asteroidStellarite1, asteroidStellarite2, asteroidStellarite3);
        case AsteroidType.Darkore:
          return GetRandomAsteroidSprite(asteroidDarkore1, asteroidDarkore2, asteroidDarkore3);
        default:
        Debug.LogError("Asteroid type not found");
          return null;
      }
    }

    private Sprite GetRandomAsteroidSprite(Sprite sprite1, Sprite sprite2, Sprite sprite3)
    {
      int random = Random.Range(0, 3);

      switch (random)
      {
        case 0:
          return sprite1;
        case 1:
          return sprite2;
        case 2:
          return sprite3;
        default:
          Debug.LogError("Asteroid sprite not found");
          return null;
      }
    }
  }
}