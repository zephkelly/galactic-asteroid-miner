using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class PrefabInstantiator
  {
    //Asteroids
    public GameObject AsteroidSmall { get; private set; }
    public GameObject AsteroidMedium { get; private set; }
    public GameObject AsteroidLarge { get; private set; }
    public GameObject AsteroidHuge { get; private set; }

    //Stars
    public GameObject WhiteDwarf { get; private set; }
    public GameObject BrownDwarf { get; private set; }
    public GameObject RedDwarf { get; private set; }
    public GameObject YellowDwarf { get; private set; }
    public GameObject BlueGiant { get; private set; }
    public GameObject OrangeGiant { get; private set; }
    public GameObject RedGiant { get; private set; }
    public GameObject BlueSuperGiant { get; private set; }
    public GameObject RedSuperGiant { get; private set; }
    public GameObject BlueHyperGiant { get; private set; }
    public GameObject RedHyperGiant { get; private set; }
    public GameObject NeutronStar { get; private set; }
    public GameObject BlackHole { get; private set; }

    //------------------------------------------------------------------------------

    private void Start()
    {
      AsteroidSmall = Resources.Load("Prefabs/Asteroids/Asteroid-S") as GameObject;
      AsteroidMedium = Resources.Load("Prefabs/Asteroids/Asteroid-M") as GameObject;
      AsteroidLarge = Resources.Load("Prefabs/Asteroids/Asteroid-L") as GameObject;
      AsteroidHuge = Resources.Load("Prefabs/Asteroids/Asteroid-XL") as GameObject;

      WhiteDwarf = Resources.Load<GameObject>("Prefabs/Stars/StarOrange");
      BrownDwarf = Resources.Load<GameObject>("Prefabs/Stars/StarOrange");
      RedDwarf = Resources.Load<GameObject>("Prefabs/Stars/StarOrange");
      YellowDwarf = Resources.Load<GameObject>("Prefabs/Stars/StarOrange");
      BlueGiant = Resources.Load<GameObject>("Prefabs/Stars/StarOrange");
      OrangeGiant = Resources.Load<GameObject>("Prefabs/Stars/StarOrange");
      RedGiant = Resources.Load<GameObject>("Prefabs/Stars/StarOrange");
      BlueSuperGiant = Resources.Load<GameObject>("Prefabs/Stars/StarOrange");
      RedSuperGiant = Resources.Load<GameObject>("Prefabs/Stars/StarOrange");
      BlueHyperGiant = Resources.Load<GameObject>("Prefabs/Stars/StarOrange");
      RedHyperGiant = Resources.Load<GameObject>("Prefabs/Stars/StarOrange");
      NeutronStar = Resources.Load<GameObject>("Prefabs/Stars/StarOrange");
      BlackHole = Resources.Load<GameObject>("Prefabs/Stars/StarOrange");
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

    public GameObject GetAsteroid(AsteroidSize _asteroidSize)
    {
      switch (_asteroidSize)
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