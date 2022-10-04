using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace zephkelly
{
  public enum RefuelType
  {
    Fifty,
    Hundred,
    Half,
    All
  }
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

  public enum ShipFuelTank
  {
    SmallTank,
    MediumTank,
    LargeTank,
    ExtraLargeTank, //HugeTank
    HugeTank //SolarRegenerator
  }
  #region Region1
  /*

  public enum ShipCargoBay
  {
    TinyCargoBay,
    SmallCargoBay,
    MediumCargoBay,
    LargeCargoBay,
    HugeCargoBay
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
  #endregion
  */
  #endregion

  public class ShipConfiguration : MonoBehaviour
  {    
    private ShipController shipController;
    private Inventory playerInventory;

    private ShipHull shipHull;
    private int hullStrengthMax;
    private int hullStrengthCurrent;
    public float hullCostPerUnit = 4;

    private ShipRadiator shipRadiator;
    private int radiatorEfficiency;
    private float hullTemperatureMax;
    [SerializeField] float hullTemperatureCurrent;
    private float ambientTemperature;
    internal float timersLength = 1;
    internal float tempIncreaser;
    internal float tempDecreaser;

    private ShipFuelTank shipFuelTank;
    private float fuelTankMaxCapacity;
    [SerializeField] float fuelTankCurrent;
    private const int fuelUsage = 1;
    internal bool toggleFuel = true;
    public float fuelCostPerLitre = 0.5f;

    public int ironPrice = 5;
    public int platinumPrice = 10;
    public int titaniumPrice = 15;
    public int goldPrice = 20;
    public int palladiumPrice = 25;
    public int cobaltPrice = 35;
    public int stellaritePrice = 50;
    public int darkorePrice = 80;

    public ShipHull ShipsHull { get => shipHull; }
    public int HullStrengthMax { get => hullStrengthMax; }
    public int HullStrengthCurrent { get => hullStrengthCurrent; }

    public ShipRadiator ShipsRadiator { get => shipRadiator; }
    public float SetAmbientTemperature { set => ambientTemperature = value; }
    public float HullTemperatureCurrent { get => hullTemperatureCurrent; }

    public ShipFuelTank ShipsFuelTank { get => shipFuelTank; }
    public float FuelMax { get => fuelTankMaxCapacity; }
    public float FuelCurrent { get => fuelTankCurrent; }
    public bool ConsumeFuel { set => toggleFuel = value; }

    //----------------------------------------------------------------------------------------------

    #region Setters
    public void AssignNewHull(ShipHull newHull)
    {
      shipHull = newHull;
      SetHull();
    }

    public void AssignNewRadiator(ShipRadiator newRadiator)
    {
      shipRadiator = newRadiator;
      SetRadiator();
    }

    public void AssignNewFuelTank(ShipFuelTank newFuelTank)
    {
      shipFuelTank = newFuelTank;
      SetFuelTank();
    }

    public void RefuelShip(int type)
    {
      int fuelNeeded = 0;
      int fuelPrice = 0;

      if (type == 1)
      {
        fuelNeeded = (int)(fuelTankMaxCapacity - fuelTankCurrent);
        fuelPrice = (int)(fuelNeeded * fuelCostPerLitre);

        shipController.Inventory.RemoveMoney((int)fuelPrice);
        fuelTankCurrent = fuelTankMaxCapacity;
      }
      else if (type == 2)
      {
        fuelNeeded = (int)((fuelTankMaxCapacity - fuelTankCurrent) / 2);
        fuelPrice = (int)(fuelNeeded * fuelCostPerLitre);

        shipController.Inventory.RemoveMoney((int)fuelPrice);
        fuelTankCurrent += fuelNeeded;

      }
      else if (type == 3)
      {
        fuelNeeded = (int)(100 / fuelCostPerLitre);

        shipController.Inventory.RemoveMoney(100);
        fuelTankCurrent += fuelNeeded;
      }
      else if (type == 4)
      {
        fuelNeeded = (int)(50 / fuelCostPerLitre);

        shipController.Inventory.RemoveMoney(50);
        fuelTankCurrent += fuelNeeded;
      }

      UIManager.Instance.OnUpdateFuel?.Invoke();
      UIManager.Instance.MadeAPurchase();
    }

    public void RepairHull(int repairAmount)
    {
      hullStrengthCurrent += repairAmount;
      hullStrengthCurrent = Mathf.Clamp(hullStrengthCurrent, 0, hullStrengthMax);
      UIManager.Instance.OnUpdateHull?.Invoke();
    }

    public void SellInventory()
    {
      var iron = playerInventory.GetItemAmount("Iron");
      var platinum = playerInventory.GetItemAmount("Platinum");
      var titanium = playerInventory.GetItemAmount("Titanium");
      var gold = playerInventory.GetItemAmount("Gold");
      var palladium = playerInventory.GetItemAmount("Palladium");
      var cobalt = playerInventory.GetItemAmount("Cobalt");
      var stellarite = playerInventory.GetItemAmount("Stellarite");
      var darkore = playerInventory.GetItemAmount("Darkore");

      var ironTotal = iron * ironPrice;
      var platinumTotal = platinum * platinumPrice;
      var titaniumTotal = titanium * titaniumPrice;
      var goldTotal = gold * goldPrice;
      var palladiumTotal = palladium * palladiumPrice;
      var cobaltTotal = cobalt * cobaltPrice;
      var stellariteTotal = stellarite * stellaritePrice;
      var darkoreTotal = darkore * darkorePrice;

      var totalPrice = 
        ironTotal + 
        platinumTotal + 
        titaniumTotal + 
        goldTotal + 
        palladiumTotal + 
        cobaltTotal + 
        stellariteTotal + 
        darkoreTotal;

      playerInventory.AddMoney(totalPrice);
      playerInventory.ClearInventory();
      UIManager.Instance.MadeAPurchase();
    }

    public int TakeDamage(int _damage)
    {
      hullStrengthCurrent -= _damage;
      UIManager.Instance.OnUpdateHull?.Invoke();
      
      if (hullStrengthCurrent <= 0) shipController.Die();

      return hullStrengthCurrent;
    }
    #endregion

    private void Start()
    {
      shipController = GetComponent<ShipController>();
      playerInventory = shipController.Inventory;

      shipHull = ShipHull.SteelHull;
      shipRadiator = ShipRadiator.SteelRadiator;
      shipFuelTank = ShipFuelTank.SmallTank;

      SetHull();
      SetRadiator();
      SetFuelTank();

      UIManager.Instance.OnUpdateFuel?.Invoke();
      UIManager.Instance.OnUpdateHull?.Invoke();
    }

    private void Update()
    {
      UseRadiators();
      if (toggleFuel) UseFuel();
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

    private void UseFuel()
    {
      fuelTankCurrent -= fuelUsage * Time.deltaTime;
      fuelTankCurrent = Mathf.Clamp(fuelTankCurrent, 0, fuelTankMaxCapacity);

      if (fuelTankCurrent <= 0) {
        shipController.Die();
      }

      UIManager.Instance.OnUpdateFuel?.Invoke();
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

    private void SetFuelTank()
    {
      switch (shipFuelTank)
      {
        case ShipFuelTank.SmallTank:
          fuelTankMaxCapacity = 60;
          break;
        case ShipFuelTank.MediumTank:
          fuelTankMaxCapacity = 120;
          break;
        case ShipFuelTank.LargeTank:
          fuelTankMaxCapacity = 240;
          break;
        case ShipFuelTank.HugeTank:
          fuelTankMaxCapacity = 480;
          break;
        case ShipFuelTank.ExtraLargeTank:
          fuelTankMaxCapacity = 960;
          break;
      }

      fuelTankCurrent = fuelTankMaxCapacity;
    }
    #endregion
  }
}