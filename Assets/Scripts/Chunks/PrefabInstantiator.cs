using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class PrefabInstantiator : MonoBehaviour
  {
    private GameObject asteroidSmall;
    private GameObject asteroidMedium;
    private GameObject asteroidLarge;
    private GameObject asteroidHuge;

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

    //Asteroids
    public GameObject AsteroidSmall { get => asteroidSmall; }
    public GameObject AsteroidMedium { get => asteroidMedium; }
    public GameObject AsteroidLarge { get => asteroidLarge; }
    public GameObject AsteroidHuge { get => asteroidHuge; }

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

    //------------------------------------------------------------------------------

    private void Awake()
    {
      asteroidSmall = Resources.Load<GameObject>("Prefabs/Asteroids/Asteroid-S");
      asteroidMedium = Resources.Load<GameObject>("Prefabs/Asteroids/Asteroid-M");
      asteroidLarge = Resources.Load<GameObject>("Prefabs/Asteroids/Asteroid-L");
      asteroidHuge = Resources.Load<GameObject>("Prefabs/Asteroids/Asteroid-XL");

      whiteDwarf = Resources.Load<GameObject>("Prefabs/Stars/StarOrange");
      brownDwarf = Resources.Load<GameObject>("Prefabs/Stars/StarOrange");
      redDwarf = Resources.Load<GameObject>("Prefabs/Stars/StarOrange");
      yellowDwarf = Resources.Load<GameObject>("Prefabs/Stars/StarOrange");
      blueGiant = Resources.Load<GameObject>("Prefabs/Stars/StarOrange");
      orangeGiant = Resources.Load<GameObject>("Prefabs/Stars/StarOrange");
      redGiant = Resources.Load<GameObject>("Prefabs/Stars/StarOrange");
      blueSuperGiant = Resources.Load<GameObject>("Prefabs/Stars/StarOrange");
      redSuperGiant = Resources.Load<GameObject>("Prefabs/Stars/StarOrange");
      blueHyperGiant = Resources.Load<GameObject>("Prefabs/Stars/StarOrange");
      redHyperGiant = Resources.Load<GameObject>("Prefabs/Stars/StarOrange");
      neutronStar = Resources.Load<GameObject>("Prefabs/Stars/StarOrange");
      blackHole = Resources.Load<GameObject>("Prefabs/Stars/StarOrange");
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
          return null;
      }
    }

    public GameObject GetAsteroid(Asteroid _asteroidInfo)
    {
      switch (_asteroidInfo.Size)
      {
        case AsteroidSize.Small:
          return GameObject.Instantiate(AsteroidSmall);
        case AsteroidSize.Medium:
          return GameObject.Instantiate(AsteroidMedium);
        case AsteroidSize.Large:
          return GameObject.Instantiate(AsteroidLarge);
        case AsteroidSize.Huge:
          return GameObject.Instantiate(AsteroidHuge);
        default:
          return null;
      }
    }
  }
}