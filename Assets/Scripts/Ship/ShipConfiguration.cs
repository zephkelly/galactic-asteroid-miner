using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace zephkelly
{
  public enum ShipHull
  {
    SteelHull,
    TitaniumHull,
    CobaltHull,
    StellariteHull,
    DarkoreHull
  }

  public enum ShipRadiator
  {
    SteelRadiator,
    TitaniumRadiator,
    CobaltRadiator,
    PalladiumRadiator,
    StellariteRadiator,
  }

  #region Region1
  /*

  public enum ShipEngine
  {
    RocketEngine,
    ImuplseEngine,
    IonEngine,
    WarpDrive,
    HyperDrive
  }

  public enum ShipWeapon
  {
    PhotonCannon,
    PlasmaCannon,
    IonCannon,
    DarkCannon
  }

  public enum ShipShield
  {
    NoShield,
    StandardShiled,
    ResonantShield,
    ElectromagneticShield,
    FluxPinnedShield
  }

  public enum ShipDeflector
  {
    NoDeflector,
    ElectromagneticDeflector,
    PlasmaticDeflector,
  }

  public enum ShipFuelTank
  {
    SmallTank,
    MediumTank,
    LargeTank,
    StellariteReactor, //HugeTank
    DarkoreReactor //SolarRegenerator
  }

  public enum ShipCargoBay
  {
    TinyCargoBay,
    SmallCargoBay,
    MediumCargoBay,
    LargeCargoBay,
    HugeCargoBay
  }
  #endregion
  */
  #endregion

  public class ShipConfiguration : MonoBehaviour
  {
    private ShipController shipController;

    private ShipHull shipHull;
    private int hullStrengthMax;
    private int hullStrengthCurrent;

    private ShipRadiator shipRadiator;
    private int radiatorEfficiency;
    private float hullTemperatureMax;
    [SerializeField] float hullTemperatureCurrent;
    private float ambientTemperature;
    internal float timersLength = 1;
    internal float tempIncreaser;
    internal float tempDecreaser;

    public ShipHull ShipHullCurrent { get => shipHull; }
    public ShipRadiator ShipRadiatorCurrent { get => shipRadiator; }

    public float SetAmbientTemperature { set => ambientTemperature = value; }

    //----------------------------------------------------------------------------------------------

    #region Setter

    public void AssignNewHull(ShipHull newHull)
    {
      shipHull = newHull;
      SetHull();
    }

    public void AssignNewRadiators(ShipRadiator newRadiator)
    {
      shipRadiator = newRadiator;
      SetRadiator();
    }
    #endregion

    private void Awake()
    {
      shipController = GetComponent<ShipController>();

      shipHull = ShipHull.SteelHull;
      shipRadiator = ShipRadiator.SteelRadiator;

      SetHull();
      SetRadiator();
    }

    public int TakeDamage(int _damage)
    {
      hullStrengthCurrent -= _damage;
      
      if (hullStrengthCurrent <= 0) shipController.Die();

      return hullStrengthCurrent;
    }

    private void Update()
    {
      UseRadiators();
    }

    private void UseRadiators()
    {
      if (ambientTemperature == 0 && hullTemperatureCurrent == 0) return;

      if (ambientTemperature > hullTemperatureCurrent)
      {
        if (tempIncreaser > 0) {
          tempIncreaser -= Time.deltaTime;
          return;
        }

        hullTemperatureCurrent += ambientTemperature / 10;
        hullTemperatureCurrent = Mathf.Clamp(hullTemperatureCurrent, 0, hullTemperatureMax);

        tempIncreaser = timersLength;
      }
      else
      {
        if (tempDecreaser > 0) {
          tempDecreaser -= Time.deltaTime;
          return;
        }

        hullTemperatureCurrent -= (hullTemperatureMax / 14) * radiatorEfficiency;
        hullTemperatureCurrent = Mathf.Clamp(hullTemperatureCurrent, 0, hullTemperatureMax);

        tempDecreaser = timersLength;
      }
    }

    #region UpgradeMethods
    private void SetHull()
    {
      switch (shipHull)
      {
        case ShipHull.SteelHull:
          hullStrengthMax = 100;
          break;
        case ShipHull.TitaniumHull:
          hullStrengthMax = 200;
          break;
        case ShipHull.CobaltHull:
          hullStrengthMax = 400;
          break;
        case ShipHull.StellariteHull:
          hullStrengthMax = 800;
          break;
        case ShipHull.DarkoreHull:
          hullStrengthMax = 10000;
          break;
      }

      hullStrengthCurrent = hullStrengthMax;
    }

    private void SetRadiator()
    {
      switch (shipRadiator)
      {
        case ShipRadiator.SteelRadiator:
          hullTemperatureMax = 100000;
          radiatorEfficiency = 1;
          break;
        case ShipRadiator.TitaniumRadiator:
          hullTemperatureMax = 1000000;
          radiatorEfficiency = 2;
          break;
        case ShipRadiator.PalladiumRadiator:
          hullTemperatureMax = 15000000;
          radiatorEfficiency = 3;
          break;
        case ShipRadiator.CobaltRadiator:
          hullTemperatureMax = 40000000;
          radiatorEfficiency = 4;
          break;
        case ShipRadiator.StellariteRadiator:
          hullTemperatureMax = 50000000;
          radiatorEfficiency = 5;
          break;
      }

      hullTemperatureCurrent = 0;
    }
    #endregion
  }
}