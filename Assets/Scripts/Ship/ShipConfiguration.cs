using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace zephkelly
{
  public enum ShipWeapon
  {
    StandardPhaser,
    PhotonCannon,
    PlasmaCannon,
    IonCannon,
    DarkCannon
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
    HugeTank,
    MegaTank //SolarRegenerator
  }
  public enum RefuelType
  {
    Fifty,
    Hundred,
    Half,
    All
  }
  public enum ShipCargoBay
  {
    TinyCargoBay,
    SmallCargoBay,
    MediumCargoBay,
    LargeCargoBay,
    HugeCargoBay
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

    private ShipWeapon shipWeapon;
    private float weaponRange;
    private float weaponDamage;
    private float fireRate;
    private Color weaponColor;

    private ShipCargoBay shipCargoBay;
    private int cargoBayMaxCapacity;
    private int cargoBayCurrentCapacity;

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

    public int ironPrice = 2;
    public int platinumPrice = 3;
    public int titaniumPrice = 8;
    public int goldPrice = 12;
    public int palladiumPrice = 18;
    public int cobaltPrice = 24;
    public int stellaritePrice = 50;
    public int darkorePrice = 80;

    public ShipWeapon ShipWeapon { get => shipWeapon; }

    public ShipCargoBay ShipsCargoBay { get => shipCargoBay; }

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

    private void Start()
    {
      shipController = GetComponent<ShipController>();
      playerInventory = shipController.Inventory;

      shipWeapon = ShipWeapon.StandardPhaser;
      shipHull = ShipHull.SteelHull;
      shipRadiator = ShipRadiator.SteelRadiator;
      shipFuelTank = ShipFuelTank.SmallTank;

      SetWeapon();
      SetHull();
      SetRadiator();
      SetFuelTank();

      DepoUIManager.Instance.OnUpdateFuel?.Invoke();
      DepoUIManager.Instance.OnUpdateHull?.Invoke();
    }

    private void Update()
    {
      UseRadiators();
      if (toggleFuel) UseFuel();
    }

    #region Setters
    public void AssignNewWeapon(int type)
    {
      switch (type)
      {
        case 1:
          shipWeapon = ShipWeapon.PhotonCannon;
          break;
        case 2:
          shipWeapon = ShipWeapon.PlasmaCannon;
          break;
        case 3:
          shipWeapon = ShipWeapon.IonCannon;
          break;
        case 4:
          shipWeapon = ShipWeapon.DarkCannon;
          break;
      }

      SetWeapon();
    }

    public void AssignNewHull(int type)
    {
      switch (type)
      {
        case 1:
          shipHull = ShipHull.TitaniumHull;
          break;
        case 2:
          shipHull = ShipHull.CobaltHull;
          break;
        case 3:
          shipHull = ShipHull.StellariteHull;
          break;
        case 4:
          shipHull = ShipHull.DarkoreHull;
          break;
      }

      SetHull();
    }

    public void AssignNewFuelTank(int type)
    {
      switch (type)
      {
        case 1:
          shipFuelTank = ShipFuelTank.MediumTank;
          break;
        case 2:
          shipFuelTank = ShipFuelTank.LargeTank;
          break;
        case 3:
          shipFuelTank = ShipFuelTank.HugeTank;
          break;
        case 4:
          shipFuelTank = ShipFuelTank.MegaTank;
          break;
      }

      SetFuelTank();
    }

    public void AssignNewCargoBay(int type)
    {
      switch (type)
      {
        case 1:
          shipCargoBay = ShipCargoBay.SmallCargoBay;
          break;
        case 2:
          shipCargoBay = ShipCargoBay.MediumCargoBay;
          break;
        case 3:
          shipCargoBay = ShipCargoBay.LargeCargoBay;
          break;
        case 4:
          shipCargoBay = ShipCargoBay.HugeCargoBay;
          break;
      }

      SetCargoBay();
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

      fuelTankCurrent = Mathf.Clamp(fuelTankCurrent, 0, fuelTankMaxCapacity);

      DepoUIManager.Instance.OnUpdateFuel?.Invoke();
      DepoUIManager.Instance.MadeAPurchase();
    }

    public void RepairHull(int type)
    {
      int hullPointsNeeded = 0;
      int hullPointPrice = 0;

      if (type == 1)
      {
        hullPointsNeeded = (int)(hullStrengthMax - hullStrengthCurrent);
        hullPointPrice = (int)(hullPointsNeeded * hullCostPerUnit);

        shipController.Inventory.RemoveMoney((int)hullPointPrice);
        hullStrengthCurrent = hullStrengthMax;
      }
      else if (type == 2)
      {
        hullPointsNeeded = (int)((hullStrengthMax - hullStrengthCurrent) / 2);
        hullPointPrice = (int)(hullPointsNeeded * hullCostPerUnit);

        shipController.Inventory.RemoveMoney((int)hullPointPrice);
        hullStrengthCurrent += hullPointsNeeded;

      }
      else if (type == 3)
      {
        hullPointsNeeded = (int)(100 / hullCostPerUnit);

        shipController.Inventory.RemoveMoney(100);
        hullStrengthCurrent += hullPointsNeeded;
      }
      else if (type == 4)
      {
        hullPointsNeeded = (int)(50 / hullCostPerUnit);

        shipController.Inventory.RemoveMoney(50);
        hullStrengthCurrent += hullPointsNeeded;
      }

      hullStrengthCurrent = Mathf.Clamp(hullStrengthCurrent, 0, hullStrengthMax);

      DepoUIManager.Instance.OnUpdateHull?.Invoke();
      DepoUIManager.Instance.MadeAPurchase();
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
      DepoUIManager.Instance.MadeAPurchase();
    }

    public int TakeDamage(int _damage)
    {
      hullStrengthCurrent -= _damage;
      DepoUIManager.Instance.OnUpdateHull?.Invoke();
      
      if (hullStrengthCurrent <= 0) shipController.Die();

      return hullStrengthCurrent;
    }
    #endregion

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

      DepoUIManager.Instance.OnUpdateFuel?.Invoke();
    }

    #region UpgradeMethods
    private void SetWeapon()
    {
      switch (shipWeapon)
      {
        case ShipWeapon.StandardPhaser:
          fireRate = 0.5f;
          weaponColor = Color.red;
          weaponDamage = 1;
          weaponRange = 50;
          break;
        case ShipWeapon.PhotonCannon:
          fireRate = 0.3f;
          weaponColor = Color.yellow;
          weaponDamage = 2;
          weaponRange = 100;
          break;
        case ShipWeapon.PlasmaCannon:
          fireRate = 0.1f;
          weaponColor = Color.blue;
          weaponDamage = 3;
          weaponRange = 150;
          break;
        case ShipWeapon.IonCannon:
          fireRate = 0.05f;
          weaponColor = Color.green;
          weaponDamage = 4;
          weaponRange = 200;
          break;
        case ShipWeapon.DarkCannon:
          fireRate = 0.01f;
          weaponColor = Color.black;
          weaponDamage = 5;
          weaponRange = 250;
          break;
      }
    }

    private void SetCargoBay()
    {
      switch (shipCargoBay)
      {
        case ShipCargoBay.TinyCargoBay:
          cargoBayMaxCapacity = 50;
          break;
        case ShipCargoBay.SmallCargoBay:
          cargoBayMaxCapacity = 100;
          break;
        case ShipCargoBay.MediumCargoBay:
          cargoBayMaxCapacity = 150;
          break;
        case ShipCargoBay.LargeCargoBay:
          cargoBayMaxCapacity = 200;
          break;
        case ShipCargoBay.HugeCargoBay:
          cargoBayMaxCapacity = 300;
          break;
      }
    }

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
        case ShipFuelTank.MegaTank:
          fuelTankMaxCapacity = 480;
          break;
        case ShipFuelTank.HugeTank:
          fuelTankMaxCapacity = 960;
          break;
      }

      fuelTankCurrent = fuelTankMaxCapacity;
    }
    #endregion
  }
}