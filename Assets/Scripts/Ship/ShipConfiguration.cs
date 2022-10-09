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
  public enum ShipEngine
  {
    RocketEngine,
    ImuplseEngine,
    IonEngine,
    WarpDrive,
    HyperDrive
  }
  #region Region1
  /*
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
    private ShipShoot shipShootScript;
    [SerializeField] SpriteRenderer shipWeaponSprite;
    [SerializeField] Material standardWeaponMaterial;
    [SerializeField] Material photonWeaponMaterial;
    [SerializeField] Material plasmaWeaponMaterial;
    [SerializeField] Material ionWeaponMaterial;
    [SerializeField] Material darkWeaponMaterial;

    private ShipEngine shipEngine;
    private int engineSpeed;

    private ShipCargoBay shipCargoBay;
    private int cargoBayMaxCapacity;
    private int cargoBayCurrentCapacity;

    private ShipHull shipHull;
    private int hullStrengthMax;
    private int hullStrengthCurrent;
    public float hullCostPerUnit = 4;
    const float invulnerabilityTime = 0.5f;
    private float invulnerabilityTimer = 0f;

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
    public float fuelCostPerLitre = 1.8f;

    //Ore Values
    public int ironPrice = 4;
    public int platinumPrice = 6;
    public int titaniumPrice = 11;
    public int goldPrice = 16;
    public int palladiumPrice = 23;
    public int cobaltPrice = 35;
    public int stellaritePrice = 50;
    public int darkorePrice = 80;

    //Engine Costs
    public int impulseEngineCost = 1200;
    public int ionEngineCost = 2500;
    public int warpDriveCost = 3800;
    public int hyperDriveCost = 5300;

    //Weapon Costs
    public int photonWeaponCost = 1000;
    public int plasmaWeaponCost = 2200;
    public int ionWeaponCost = 3800;
    public int darkWeaponCost = 5200;

    //Hull Costs
    public int titaniumHullCost = 1200;
    public int cobaltHullCost = 2500;
    public int stellariteHullCost = 3000;
    public int darkoreHullCost = 5500;

    //FuelTank Costs
    public int mediumTankCost = 1000;
    public int largeTankCost = 1800;
    public int hugeTankCost = 2500;
    public int megaTankCost = 5000;

    //CargoBay Costs
    public int smallCargoBayCost = 1500;
    public int mediumCargoBayCost = 2200;
    public int largeCargoBayCost = 3400;
    public int hugeCargoBayCost = 4800;

    public ShipEngine ShipEngine { get => shipEngine; }
    public int EngineSpeed { get => engineSpeed; }

    public ShipWeapon ShipWeapon { get => shipWeapon; }

    public ShipCargoBay ShipsCargoBay { get => shipCargoBay; }
    public int CargoBayMaxCapacity { get => cargoBayMaxCapacity; }
    public int CargoBayCurrentCapacity { get => cargoBayCurrentCapacity; }

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
      shipShootScript = GetComponent<ShipShoot>();
      ShipUIManager.Instance = gameObject.GetComponent<ShipUIManager>();
      playerInventory = shipController.Inventory;

      shipEngine = ShipEngine.RocketEngine;
      shipWeapon = ShipWeapon.StandardPhaser;
      shipHull = ShipHull.SteelHull;
      shipRadiator = ShipRadiator.SteelRadiator;
      shipCargoBay = ShipCargoBay.TinyCargoBay;
      shipFuelTank = ShipFuelTank.SmallTank;

      SetEngine();
      SetCargoBay();
      SetWeapon();
      SetHull();
      SetRadiator();
      SetFuelTank();

      DepoUIManager.Instance.OnUpdateFuel?.Invoke();
      DepoUIManager.Instance.OnUpdateHull?.Invoke();
      DepoUIManager.Instance.OnUpdateCargo?.Invoke();

      cargoBayCurrentCapacity = 0;
      playerInventory.ClearInventory();
      playerInventory.ClearCredits();
    }

    private void Update()
    {
      UseRadiators();
      if (toggleFuel) UseFuel();

      if (invulnerabilityTimer > 0) invulnerabilityTimer -= Time.deltaTime;
    }

    #region Setters
    public void AssignNewEngine(int type)
    {
      switch (type)
      {
        case 1:
          shipEngine = ShipEngine.ImuplseEngine;
          shipController.Inventory.RemoveMoney(impulseEngineCost);
          break;
        case 2:
          shipEngine = ShipEngine.IonEngine;
          shipController.Inventory.RemoveMoney(ionEngineCost);
          break;
        case 3:
          shipEngine = ShipEngine.WarpDrive;
          shipController.Inventory.RemoveMoney(warpDriveCost);
          break;
        case 4:
          shipEngine = ShipEngine.HyperDrive;
          shipController.Inventory.RemoveMoney(hyperDriveCost);
          break;
      }

      SetEngine();
    }

    public void AssignNewWeapon(int type)
    {
      switch (type)
      {
        case 1:
          shipWeapon = ShipWeapon.PhotonCannon;
          shipController.Inventory.RemoveMoney(photonWeaponCost);
          break;
        case 2:
          shipWeapon = ShipWeapon.PlasmaCannon;
          shipController.Inventory.RemoveMoney(plasmaWeaponCost);
          break;
        case 3:
          shipWeapon = ShipWeapon.IonCannon;
          shipController.Inventory.RemoveMoney(ionWeaponCost);
          break;
        case 4:
          shipWeapon = ShipWeapon.DarkCannon;
          shipController.Inventory.RemoveMoney(darkWeaponCost);
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
          shipController.Inventory.RemoveMoney(titaniumHullCost);
          break;
        case 2:
          shipHull = ShipHull.CobaltHull;
          shipController.Inventory.RemoveMoney(cobaltHullCost);
          break;
        case 3:
          shipHull = ShipHull.StellariteHull;
          shipController.Inventory.RemoveMoney(stellariteHullCost);
          break;
        case 4:
          shipHull = ShipHull.DarkoreHull;
          shipController.Inventory.RemoveMoney(darkoreHullCost);
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
          shipController.Inventory.RemoveMoney(mediumTankCost);
          break;
        case 2:
          shipFuelTank = ShipFuelTank.LargeTank;
          shipController.Inventory.RemoveMoney(largeTankCost);
          break;
        case 3:
          shipFuelTank = ShipFuelTank.HugeTank;
          shipController.Inventory.RemoveMoney(hugeTankCost);
          break;
        case 4:
          shipFuelTank = ShipFuelTank.MegaTank;
          shipController.Inventory.RemoveMoney(megaTankCost);
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
          shipController.Inventory.RemoveMoney(smallCargoBayCost);
          break;
        case 2:
          shipCargoBay = ShipCargoBay.MediumCargoBay;
          shipController.Inventory.RemoveMoney(mediumCargoBayCost);
          break;
        case 3:
          shipCargoBay = ShipCargoBay.LargeCargoBay;
          shipController.Inventory.RemoveMoney(largeCargoBayCost);
          break;
        case 4:
          shipCargoBay = ShipCargoBay.HugeCargoBay;
          shipController.Inventory.RemoveMoney(hugeCargoBayCost);
          break;
      }

      SetCargoBay();
    }

    public void AddItemToCargo(int amount)
    {
      cargoBayCurrentCapacity += amount;

      cargoBayCurrentCapacity = Mathf.Clamp(cargoBayCurrentCapacity, 0, cargoBayMaxCapacity);

      if (cargoBayCurrentCapacity == cargoBayMaxCapacity)
      {
        ShipUIManager.Instance.OnCargoFull?.Invoke();
      }
    }

    public void ClearCargoBay()
    {
      cargoBayCurrentCapacity = 0;
      ShipUIManager.Instance.OnEmptyCargo?.Invoke();
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
        fuelNeeded = (int)(25 / fuelCostPerLitre);

        shipController.Inventory.RemoveMoney(25);
        fuelTankCurrent += fuelNeeded;
      }
      else if (type == 4)
      {
        fuelNeeded = (int)(10 / fuelCostPerLitre);

        shipController.Inventory.RemoveMoney(10);
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
      ShipUIManager.Instance.OnEmptyCargo?.Invoke();
      DepoUIManager.Instance.MadeAPurchase();
      DepoUIManager.Instance.OnUpdateCargo?.Invoke();
    }

    public void TakeDamage(int _damage)
    {
      if (invulnerabilityTimer > 0) return;
      invulnerabilityTimer = invulnerabilityTime;

      hullStrengthCurrent -= _damage;
      DepoUIManager.Instance.OnUpdateHull?.Invoke();

      zephkelly.AudioManager.Instance.PlaySound("ShipDamage");

      if (hullStrengthCurrent <= 0) {
        StartCoroutine(DeathDelay());
        return;
      }

      StartCoroutine(InvulnerabilityFlash());
    }
    #endregion

    IEnumerator DeathDelay()
    {
      shipWeaponSprite.color = Color.red;
      yield return new WaitForSeconds(0.6f);
      shipController.Die("Hull failure, you hit one too many asteroids...");
    }

    IEnumerator InvulnerabilityFlash()
    {
      shipWeaponSprite.color = Color.red;
      yield return new WaitForSeconds(0.4f);
      shipWeaponSprite.color = Color.white;
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
        shipController.Die("You ran out of fuel...");
      }

      DepoUIManager.Instance.OnUpdateFuel?.Invoke();
    }

    #region UpgradeMethods
    private void SetEngine()
    {
      var color1 = Color.white;
      var color2 = Color.white;

      switch (shipEngine)
      {
        case ShipEngine.RocketEngine:
          engineSpeed = 120;

          var newGradient = new Gradient();
          color1 = new Color(1f, 0.775f, 0.3f);
          color2 = new Color(0.575f, 0f, 0f);

          newGradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(color1, 0.0f), new GradientColorKey(color2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1, 0.0f), new GradientAlphaKey(1, 1.0f) }
          );

          shipController.UpgradeEngine(newGradient);
          break;

        case ShipEngine.ImuplseEngine:
          engineSpeed = 180;

          var newGradient2 = new Gradient();
          color1 = new Color(1f, 1f, 0.75f);
          color2 = new Color(0.62f, 0.32f, 0f);

          newGradient2.SetKeys(
            new GradientColorKey[] { new GradientColorKey(color1, 0.0f), new GradientColorKey(color2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1, 0.0f), new GradientAlphaKey(1, 1.0f) }
          );

          shipController.UpgradeEngine(newGradient2);
          break;


        case ShipEngine.IonEngine:
          engineSpeed = 240;

          var newGradient3 = new Gradient();
          color1 = new Color(0.5f, 1f, 0.86f);
          color2 = new Color(0f, 0.65f, 0f);

          newGradient3.SetKeys(
            new GradientColorKey[] { new GradientColorKey(color1, 0.0f), new GradientColorKey(color2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1, 0.0f), new GradientAlphaKey(1, 1.0f) }
          );

          shipController.UpgradeEngine(newGradient3);
          break;
        
        case ShipEngine.WarpDrive:
          engineSpeed = 300;

          var newGradient4 = new Gradient();
          color1 = new Color(0.5f, 0.75f, 1f);
          color2 = new Color(0f, 0.3f, 0.65f);

          newGradient4.SetKeys(
            new GradientColorKey[] { new GradientColorKey(color1, 0.0f), new GradientColorKey(color2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1, 0.0f), new GradientAlphaKey(1, 1.0f) }
          );

          shipController.UpgradeEngine(newGradient4);
          break;

        case ShipEngine.HyperDrive:
          engineSpeed = 360;

          var newGradient5 = new Gradient();
          color1 = Color.white;
          color2 = new Color(0.15f, 0.15f, 0.15f);

          newGradient5.SetKeys(
            new GradientColorKey[] { new GradientColorKey(color1, 0.0f), new GradientColorKey(color2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1, 0.0f), new GradientAlphaKey(1, 1.0f) }
          );

          shipController.UpgradeEngine(newGradient5);
          break;
      }
    }

    private void SetWeapon()
    {
      switch (shipWeapon)
      {
        case ShipWeapon.StandardPhaser:
          shipShootScript.SetFireRate(0.35f);
          shipShootScript.SetProjectileSpeed(50f);
          shipShootScript.SetProjectileMat(standardWeaponMaterial);
          break;
        case ShipWeapon.PhotonCannon:
          shipShootScript.SetFireRate(0.25f);
          shipShootScript.SetProjectileSpeed(65f);
          shipShootScript.SetProjectileMat(photonWeaponMaterial);
          break;
        case ShipWeapon.PlasmaCannon:
          shipShootScript.SetFireRate(0.18f);
          shipShootScript.SetProjectileSpeed(80f);
          shipShootScript.SetProjectileMat(plasmaWeaponMaterial);
          break;
        case ShipWeapon.IonCannon:
          shipShootScript.SetFireRate(0.12f);
          shipShootScript.SetProjectileSpeed(100f);
          shipShootScript.SetProjectileMat(ionWeaponMaterial);
          break;
        case ShipWeapon.DarkCannon:
          shipShootScript.SetFireRate(0.09f);
          shipShootScript.SetProjectileSpeed(120f);
          shipShootScript.SetProjectileMat(darkWeaponMaterial);
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
          cargoBayMaxCapacity = 300;
          break;
        case ShipCargoBay.HugeCargoBay:
          cargoBayMaxCapacity = 500;
          break;
      }
    }

    private void SetHull()
    {
      switch (shipHull)
      {
        case ShipHull.SteelHull:
          hullStrengthMax = 40;
          break;
        case ShipHull.TitaniumHull:
          hullStrengthMax = 60;
          break;
        case ShipHull.CobaltHull:
          hullStrengthMax = 80;
          break;
        case ShipHull.StellariteHull:
          hullStrengthMax = 100;
          break;
        case ShipHull.DarkoreHull:
          hullStrengthMax = 120;
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
          fuelTankMaxCapacity = 40;
          break;
        case ShipFuelTank.MediumTank:
          fuelTankMaxCapacity = 140;
          break;
        case ShipFuelTank.LargeTank:
          fuelTankMaxCapacity = 260;
          break;
        case ShipFuelTank.MegaTank:
          fuelTankMaxCapacity = 380;
          break;
        case ShipFuelTank.HugeTank:
          fuelTankMaxCapacity = 600;
          break;
      }

      fuelTankCurrent = fuelTankMaxCapacity;
    }
    #endregion
  }
}