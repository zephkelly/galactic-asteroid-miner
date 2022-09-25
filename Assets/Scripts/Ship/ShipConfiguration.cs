using System.Collections;
using System.Collections.Generic;
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

  public class ShipConfiguration
  {
    private ShipController shipController;
    private ShipHull shipHullCurrent;
    private ShipEngine shipEngineCurrent;
    private ShipWeapon shipWeaponCurrent;
    private ShipShield shipShieldCurrent;
    private ShipFuelTank shipFuelTankCurrent;
    private ShipCargoBay shipCargoBayCurrent;
    private ShipDeflector shipDeflectorCurrent;

    private int hullStrengthMax;
    private int engineSpeedMax;
    private int weaponDamageMax;
    private int shieldStrengthMax;
    private int fuelCapacityMax;
    private int cargoCapacityMax;
    private int deflectorStrengthMax;

    private int hullStrengthCurrent;
    private int engineSpeedCurrent;
    private int weaponDamageCurrent;
    private int shieldStrengthCurrent;
    private int fuelCapacityCurrent;
    private int cargoCapacityCurrent;
    private int deflectorStrengthCurrent;

    //------------------------------------------------------------------------------
    
    public ShipConfiguration (ShipController _shipController)
    {
      shipController = _shipController;
    }

    public ShipHull HullType { get => shipHullCurrent; }
    public ShipEngine EngineType { get => shipEngineCurrent; }
    public ShipWeapon WeaponType { get => shipWeaponCurrent; }
    public ShipShield ShieldType { get => shipShieldCurrent; }
    public ShipFuelTank FuelTankType { get => shipFuelTankCurrent; }
    public ShipCargoBay CargoBayType { get => shipCargoBayCurrent; }
    public ShipDeflector DeflectorType { get => shipDeflectorCurrent; }

    public int HullStrengthMax { get => hullStrengthMax; }
    public int EngineSpeedMax { get => engineSpeedMax; }
    public int WeaponDamageMax { get => weaponDamageMax; }
    public int ShieldStrengthMax { get => shieldStrengthMax; }
    public int FuelCapacityMax { get => fuelCapacityMax; }
    public int CargoCapacityMax { get => cargoCapacityMax; }
    public int DeflectorStrengthMax { get => deflectorStrengthMax; }

    public int HullStrengthCur { get => hullStrengthCurrent; }
    public int EngineSpeedCur { get => engineSpeedCurrent; }
    public int WeaponDamageCur { get => weaponDamageCurrent; }
    public int ShieldStrengthCur { get => shieldStrengthCurrent; }
    public int FuelCapacityCur { get => fuelCapacityCurrent; }
    public int CargoCapacityCur { get => cargoCapacityCurrent; }
    public int DeflectorStrengthCur { get => deflectorStrengthCurrent; }

    //------------------------------------------------------------------------------

    public ShipHull SetHull { set => shipHullCurrent = value; }
    public ShipEngine SetEngine { set => shipEngineCurrent = value; }

    public void UpdateWeapon (ShipWeapon _shipWeapon) {
      shipWeaponCurrent = _shipWeapon;
      SetShipConfiguration();
    }

    public void UpdateShield (ShipShield _shipShield) {
      shipShieldCurrent = _shipShield;
      SetShipConfiguration();
    }

    public void UpdateFuelTank (ShipFuelTank _shipFuelTank) {
      shipFuelTankCurrent = _shipFuelTank;
      SetShipConfiguration();
    }

    public void UpdateCargoBay (ShipCargoBay _shipCargoBay) {
      shipCargoBayCurrent = _shipCargoBay;
      SetShipConfiguration();
    }

    public void UpdateDeflector (ShipDeflector _shipDeflector) {
      shipDeflectorCurrent = _shipDeflector;
      SetShipConfiguration();
    }

    public int TakeDamage(int _damage)
    {
      hullStrengthCurrent -= _damage;
      return hullStrengthCurrent;
    }

    //------------------------------------------------------------------------------

    public void AssignDefaults()
    {
      shipHullCurrent = ShipHull.SteelHull;
      shipEngineCurrent = ShipEngine.RocketEngine;
      shipWeaponCurrent = ShipWeapon.PhotonCannon;
      shipShieldCurrent = ShipShield.NoShield;
      shipFuelTankCurrent = ShipFuelTank.SmallTank;
      shipCargoBayCurrent = ShipCargoBay.TinyCargoBay;
      shipDeflectorCurrent = ShipDeflector.NoDeflector;

      SetShipConfiguration();
    }

    private void LoadGameSaveData()
    {
      //Load game save data
    }

    private void SetShipConfiguration()
    {
      SetHull();
      SetEngine();
      SetWeapon();
      SetShield();
      SetFuelTank();
      SetCargoBay();
      SetDeflector();

      AssignToCurrent();

      //Adjust-everything-below-------------------------------------------------------

      void SetHull()
      {
        switch (shipHullCurrent)
        {
          case ShipHull.SteelHull:
            hullStrengthMax = 100;
            break;
          case ShipHull.TitaniumHull:
            hullStrengthMax = 200;
            break;
          case ShipHull.CobaltHull:
            hullStrengthMax = 300;
            break;
          case ShipHull.StellariteHull:
            hullStrengthMax = 400;
            break;
          case ShipHull.DarkoreHull:
            hullStrengthMax = 500;
            break;
        }
      }

      void SetEngine()
      {
        switch (shipEngineCurrent)
        {
          case ShipEngine.RocketEngine:
            engineSpeedMax = 100;
            break;
          case ShipEngine.ImuplseEngine:
            engineSpeedMax = 200;
            break;
          case ShipEngine.IonEngine:
            engineSpeedMax = 300;
            break;
          case ShipEngine.WarpDrive:
            engineSpeedMax = 400;
            break;
          case ShipEngine.HyperDrive:
            engineSpeedMax = 500;
            break;
        }
      }

      void SetWeapon()
      {
        switch (shipWeaponCurrent)
        {
          case ShipWeapon.PhotonCannon:
            weaponDamageMax = 100;
            break;
          case ShipWeapon.PlasmaCannon:
            weaponDamageMax = 200;
            break;
          case ShipWeapon.IonCannon:
            weaponDamageMax = 300;
            break;
          case ShipWeapon.DarkCannon:
            weaponDamageMax = 400;
            break;
        }
      }

      void SetShield()
      {
        switch (shipShieldCurrent)
        {
          case ShipShield.NoShield:
            shieldStrengthMax = 0;
            break;
          case ShipShield.StandardShiled:
            shieldStrengthMax = 100;
            break;
          case ShipShield.ResonantShield:
            shieldStrengthMax = 200;
            break;
          case ShipShield.ElectromagneticShield:
            shieldStrengthMax = 300;
            break;
          case ShipShield.FluxPinnedShield:
            shieldStrengthMax = 400;
            break;
        }
      }

      void SetFuelTank()
      {
        switch (shipFuelTankCurrent)
        {
          case ShipFuelTank.SmallTank:
            fuelCapacityMax = 100;
            break;
          case ShipFuelTank.MediumTank:
            fuelCapacityMax = 200;
            break;
          case ShipFuelTank.LargeTank:
            fuelCapacityMax = 300;
            break;
          case ShipFuelTank.StellariteReactor:
            fuelCapacityMax = 400;
            break;
          case ShipFuelTank.DarkoreReactor:
            fuelCapacityMax = 500;
            break;
        }
      }

      void SetCargoBay()
      {
        switch (shipCargoBayCurrent)
        {
          case ShipCargoBay.TinyCargoBay:
            cargoCapacityMax = 100;
            break;
          case ShipCargoBay.SmallCargoBay:
            cargoCapacityMax = 200;
            break;
          case ShipCargoBay.MediumCargoBay:
            cargoCapacityMax = 300;
            break;
          case ShipCargoBay.LargeCargoBay:
            cargoCapacityMax = 400;
            break;
          case ShipCargoBay.HugeCargoBay:
            cargoCapacityMax = 500;
            break;
        }
      }

      void SetDeflector()
      {
        switch (shipDeflectorCurrent)
        {
          case ShipDeflector.NoDeflector:
            deflectorStrengthMax = 0;
            break;
          case ShipDeflector.ElectromagneticDeflector:
            deflectorStrengthMax = 100;
            break;
          case ShipDeflector.PlasmaticDeflector:
            deflectorStrengthMax = 200;
            break;
        }
      }

      void AssignToCurrent()
      {
        hullStrengthCurrent = hullStrengthMax;
        engineSpeedCurrent = engineSpeedMax;
        weaponDamageCurrent = weaponDamageMax;
        shieldStrengthCurrent = shieldStrengthMax;
        fuelCapacityCurrent = fuelCapacityMax;
        cargoCapacityCurrent = cargoCapacityMax;
        deflectorStrengthCurrent = deflectorStrengthMax;
      }
    }
  }
}