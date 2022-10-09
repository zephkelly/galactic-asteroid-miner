using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace zephkelly
{
  public class DepoUIManager : MonoBehaviour
  {
    public static DepoUIManager Instance;

    public UnityEvent OnHoverDepo;
    public UnityEvent OnLeaveDepoHover;

    public UnityEvent OnUpdateFuel;
    public UnityEvent OnUpdateHull;
    public UnityEvent OnUpdateCargo;

    private Slider healthSlider;
    private Slider fuelSlider;
    private Slider cargoSlider;

    #region Depot UI
    private GameObject depotMenuObject;
    private TextMeshProUGUI playerCreditsAmount;

    private Button refuelAllButton;
    private Button refuelHalfButton;
    private Button refuel25Button;
    private Button refuel10Button;
    private Button repairAllButton;
    private Button repairHalfButton;
    private Button repair25Button;
    private Button repair10Button;
    private TextMeshProUGUI refuelAllCost;
    private TextMeshProUGUI refuelHalfCost;
    private TextMeshProUGUI refuel25Cost;
    private TextMeshProUGUI refuel10Cost;

    private TextMeshProUGUI repairAllCost;
    private TextMeshProUGUI repairHalfCost;
    private TextMeshProUGUI repair25Cost;
    private TextMeshProUGUI repair10Cost;

    private TextMeshProUGUI tooltipUIElement;
    private TextMeshProUGUI ironCount;
    private TextMeshProUGUI platinumCount;
    private TextMeshProUGUI titaniumCount;
    private TextMeshProUGUI goldCount;
    private TextMeshProUGUI palladiumCount;
    private TextMeshProUGUI cobaltCount;
    private TextMeshProUGUI stellariteCount;
    private TextMeshProUGUI darkoreCount;

    private TextMeshProUGUI ironValue;
    private TextMeshProUGUI platinumValue;
    private TextMeshProUGUI titaniumValue;
    private TextMeshProUGUI goldValue;
    private TextMeshProUGUI palladiumValue;
    private TextMeshProUGUI cobaltValue;
    private TextMeshProUGUI stellariteValue;
    private TextMeshProUGUI darkoreValue;
    private TextMeshProUGUI totalValue;
    #endregion

    #region Upgrades
    private GameObject upgradesMenuObject;
    private Button impulseEngineUpgrade;
    private Button ionEngineUpgrade;
    private Button warpEngineUpgrade;
    private Button hyperEngineUpgrade;

    private Button photonWeaponUpgrade;
    private Button plasmaWeaponUpgrade;
    private Button ionWeaponUpgrade;
    private Button darkWeaponUpgrade;

    private Button titaniumHullUpgrade;
    private Button cobaltHullUpgrade;
    private Button stellariteHullUpgrade;
    private Button darkHullUpgrade;

    private Button mediumTankUpgrade;
    private Button largeTankUpgrade;
    private Button hugeTankUpgrade;
    private Button megaTankUpgrade;

    private Button smallBayUpgrade;
    private Button mediumBayUpgrade;
    private Button largeBayUpgrade;
    private Button hugeBayUpgrade;

    private TextMeshProUGUI currentEngineText;
    private TextMeshProUGUI currentWeaponText;
    private TextMeshProUGUI currentHullText;
    private TextMeshProUGUI currentTankText;
    private TextMeshProUGUI currentBayText;
    #endregion

    private Inventory playerInventory;
    private GameObject depotUIObject;

    internal bool depotUI;
    internal bool depotToggle;


    public void DisableMenu()
    {
      DepoMenuDisable();
    }

    private void Awake()
    {
      if (Instance == null)
      {
        Instance = this;
      }
      else
      {
        Destroy(gameObject);
      }

      #region Ship Setup
      fuelSlider = GameObject.Find("ShipFuelUI").GetComponent<Slider>();
      healthSlider = GameObject.Find("ShipHullUI").GetComponent<Slider>();
      cargoSlider = GameObject.Find("ShipBayUI").GetComponent<Slider>();

      OnUpdateFuel.AddListener(UpdateFuelUI);
      OnUpdateHull.AddListener(UpdateHullUI);
      OnUpdateCargo.AddListener(UpdateCargoUI);
      #endregion
    }

    private void Start()
    {
      UpdateCargoUI();
      
      playerCreditsAmount = GameObject.Find("CreditsAmount").GetComponent<TextMeshProUGUI>();
      playerInventory = ShipController.Instance.Inventory;
      tooltipUIElement = GameObject.Find("TooltipUI").GetComponent<TextMeshProUGUI>();

      #region Depo Panel
      //Reapir/Refuel Buttons
      refuelAllButton = GameObject.Find("RefuelAllButton").GetComponent<Button>();
      refuelHalfButton = GameObject.Find("RefuelHalfButton").GetComponent<Button>();
      refuel25Button = GameObject.Find("Refuel25Button").GetComponent<Button>();
      refuel10Button = GameObject.Find("Refuel10Button").GetComponent<Button>();
      repairAllButton = GameObject.Find("RepairAllButton").GetComponent<Button>();
      repairHalfButton = GameObject.Find("RepairHalfButton").GetComponent<Button>();
      repair25Button = GameObject.Find("Repair25Button").GetComponent<Button>();
      repair10Button = GameObject.Find("Repair10Button").GetComponent<Button>();

      //Repir/Refuel Text
      refuelAllCost = GameObject.Find("RefuelAllCost").GetComponent<TextMeshProUGUI>();
      refuelHalfCost = GameObject.Find("RefuelHalfCost").GetComponent<TextMeshProUGUI>();
      refuel25Cost = GameObject.Find("Refuel25").GetComponent<TextMeshProUGUI>();
      refuel10Cost = GameObject.Find("Refuel10").GetComponent<TextMeshProUGUI>();
      repairAllCost = GameObject.Find("RepairAllCost").GetComponent<TextMeshProUGUI>();
      repairHalfCost = GameObject.Find("RepairHalfCost").GetComponent<TextMeshProUGUI>();
      repair25Cost = GameObject.Find("Repair25").GetComponent<TextMeshProUGUI>();
      repair10Cost = GameObject.Find("Repair10").GetComponent<TextMeshProUGUI>();

      //Ore count text
      ironCount = GameObject.Find("IronCount").GetComponent<TextMeshProUGUI>();
      platinumCount = GameObject.Find("PlatinumCount").GetComponent<TextMeshProUGUI>();
      titaniumCount = GameObject.Find("TitaniumCount").GetComponent<TextMeshProUGUI>();
      goldCount = GameObject.Find("GoldCount").GetComponent<TextMeshProUGUI>();
      palladiumCount = GameObject.Find("PalladiumCount").GetComponent<TextMeshProUGUI>();
      cobaltCount = GameObject.Find("CobaltCount").GetComponent<TextMeshProUGUI>();
      stellariteCount = GameObject.Find("StellariteCount").GetComponent<TextMeshProUGUI>();
      darkoreCount = GameObject.Find("DarkoreCount").GetComponent<TextMeshProUGUI>();

      //Ore sell amounts
      ironValue = GameObject.Find("IronPrice").GetComponent<TextMeshProUGUI>();
      platinumValue = GameObject.Find("PlatinumPrice").GetComponent<TextMeshProUGUI>();
      titaniumValue = GameObject.Find("TitaniumPrice").GetComponent<TextMeshProUGUI>();
      goldValue = GameObject.Find("GoldPrice").GetComponent<TextMeshProUGUI>();
      palladiumValue = GameObject.Find("PalladiumPrice").GetComponent<TextMeshProUGUI>();
      cobaltValue = GameObject.Find("CobaltPrice").GetComponent<TextMeshProUGUI>();
      stellariteValue = GameObject.Find("StellaritePrice").GetComponent<TextMeshProUGUI>();
      darkoreValue = GameObject.Find("DarkorePrice").GetComponent<TextMeshProUGUI>();

      //Total Cargo Value
      totalValue = GameObject.Find("TotalMineralPrice").GetComponent<TextMeshProUGUI>();

      depotMenuObject = GameObject.Find("DepotMenu");
      depotMenuObject.SetActive(true);

      OnHoverDepo.AddListener(DepoMenuEnable);
      OnLeaveDepoHover.AddListener(DepoMenuDisable);
      #endregion

      #region Upgrades Panel
      impulseEngineUpgrade = GameObject.Find("ImpulseEngineButton").GetComponent<Button>();
      ionEngineUpgrade = GameObject.Find("IonEngineButton").GetComponent<Button>();
      warpEngineUpgrade = GameObject.Find("WarpDriveButton").GetComponent<Button>();
      hyperEngineUpgrade = GameObject.Find("HyperDriveButton").GetComponent<Button>();
      
      photonWeaponUpgrade = GameObject.Find("PhotonButton").GetComponent<Button>();
      plasmaWeaponUpgrade = GameObject.Find("PlasmaButton").GetComponent<Button>();
      ionWeaponUpgrade = GameObject.Find("IonButton").GetComponent<Button>();
      darkWeaponUpgrade = GameObject.Find("DarkButton").GetComponent<Button>();

      titaniumHullUpgrade = GameObject.Find("TitaniumHullButton").GetComponent<Button>();
      cobaltHullUpgrade = GameObject.Find("CobaltHullButton").GetComponent<Button>();
      stellariteHullUpgrade = GameObject.Find("StellariteHullButton").GetComponent<Button>();
      darkHullUpgrade = GameObject.Find("DarkHullButton").GetComponent<Button>();

      mediumTankUpgrade = GameObject.Find("MediumTankButton").GetComponent<Button>();
      largeTankUpgrade = GameObject.Find("LargeTankButton").GetComponent<Button>();
      hugeTankUpgrade = GameObject.Find("HugeTankButton").GetComponent<Button>();
      megaTankUpgrade = GameObject.Find("MegaTankButton").GetComponent<Button>();

      smallBayUpgrade = GameObject.Find("SmallCargoButton").GetComponent<Button>();
      mediumBayUpgrade = GameObject.Find("MediumCargoButton").GetComponent<Button>();
      largeBayUpgrade = GameObject.Find("LargeCargoButton").GetComponent<Button>();
      hugeBayUpgrade = GameObject.Find("HugeCargoButton").GetComponent<Button>();

      currentEngineText = GameObject.Find("CurrentEngineText").GetComponent<TextMeshProUGUI>();
      currentWeaponText = GameObject.Find("CurrentWeaponText").GetComponent<TextMeshProUGUI>();
      currentHullText = GameObject.Find("CurrentHullText").GetComponent<TextMeshProUGUI>();
      currentTankText = GameObject.Find("CurrentTankText").GetComponent<TextMeshProUGUI>();
      currentBayText = GameObject.Find("CurrentBayText").GetComponent<TextMeshProUGUI>();

      upgradesMenuObject = GameObject.Find("UpgradesMenu");
      upgradesMenuObject.SetActive(false);

      depotUIObject = GameObject.Find("DepotUI");
      depotUIObject.SetActive(false);
      #endregion
    }

    private void Update()
    {
      if (depotUI) DepotMenuBehaviour();
    }

    public void MadeAPurchase()
    {
      UpdateOreOverlay();
      UpdateDepoPrices();
      UpdateUpgradesMenu();

      UpdateHullUI();
      UpdateFuelUI();
      UpdateCargoUI();
    }
    
    private void DepoMenuEnable()
    {
      depotUI = true;
      depotToggle = false;

      UpdateOreOverlay();
      UpdateDepoPrices();
      UpdateUpgradesMenu();

      tooltipUIElement.color = new Color(1, 1, 1, 1);
    }

    private void DepoMenuDisable()
    {
      depotUI = false;

      tooltipUIElement.color = new Color(1, 1, 1, 0);

      upgradesMenuObject.SetActive(false);
      depotMenuObject.SetActive(true);

      if (depotToggle == false) return;
      depotToggle = false;

      ShipController.Instance.CanShoot(true);
      depotUIObject.SetActive(false);

      CameraController.Instance.ClearOffset();
      ShipController.Instance.ShipConfig.ConsumeFuel = true;
    }

    private void DepotMenuBehaviour()
    {
      if (Input.GetKeyDown(KeyCode.Tab))
      {
        depotToggle = !depotToggle;
      }

      if (depotToggle)
      {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
          depotToggle = false;
        }
      }

      if (depotToggle)
      {
        tooltipUIElement.color = new Color(1, 1, 1, 0);

        ShipController.Instance.CanShoot(false);
        depotUIObject.SetActive(true);

        CameraController.Instance.SetOffset(new Vector2(20, 0));
        ShipController.Instance.ShipConfig.ConsumeFuel = false;
      }
      else
      {
        tooltipUIElement.color = new Color(1, 1, 1, 1);

        ShipController.Instance.CanShoot(true);
        depotUIObject.SetActive(false);

        CameraController.Instance.ClearOffset();
        ShipController.Instance.ShipConfig.ConsumeFuel = true;
      }
    }

    private void UpdateOreOverlay()
    {
      var shipConfig = ShipController.Instance.ShipConfig;

      //Get ore counts
      var iron = playerInventory.GetItemAmount("Iron");
      var platinum = playerInventory.GetItemAmount("Platinum");
      var titanium = playerInventory.GetItemAmount("Titanium");
      var gold = playerInventory.GetItemAmount("Gold");
      var palladium = playerInventory.GetItemAmount("Palladium");
      var cobalt = playerInventory.GetItemAmount("Cobalt");
      var stellarite = playerInventory.GetItemAmount("Stellarite");
      var darkore = playerInventory.GetItemAmount("Darkore");

      ironCount.text = iron.ToString();
      platinumCount.text = platinum.ToString();
      titaniumCount.text = titanium.ToString();
      goldCount.text = gold.ToString();
      palladiumCount.text = palladium.ToString();
      cobaltCount.text = cobalt.ToString();
      stellariteCount.text = stellarite.ToString();
      darkoreCount.text = darkore.ToString();

      //Get ore prices
      var ironTotal = iron * shipConfig.ironPrice;
      var platinumTotal = platinum * shipConfig.platinumPrice;
      var titaniumTotal = titanium * shipConfig.titaniumPrice;
      var goldTotal = gold * shipConfig.goldPrice;
      var palladiumTotal = palladium * shipConfig.palladiumPrice;
      var cobaltTotal = cobalt * shipConfig.cobaltPrice;
      var stellariteTotal = stellarite * shipConfig.stellaritePrice;
      var darkoreTotal = darkore * shipConfig.darkorePrice;

      var totalPrice = 
        ironTotal + 
        platinumTotal + 
        titaniumTotal + 
        goldTotal + 
        palladiumTotal + 
        cobaltTotal + 
        stellariteTotal + 
        darkoreTotal;

      ironValue.text = "= $" + ironTotal.ToString();
      platinumValue.text = "= $" + platinumTotal.ToString();
      titaniumValue.text = "= $" + titaniumTotal.ToString();
      goldValue.text = "= $" + goldTotal.ToString();
      palladiumValue.text = "= $" + palladiumTotal.ToString();
      cobaltValue.text = "= $" + cobaltTotal.ToString();
      stellariteValue.text = "= $" + stellariteTotal.ToString();
      darkoreValue.text = "= $" + darkoreTotal.ToString();

      totalValue.text = "= $" + totalPrice.ToString();
    }

    private void UpdateDepoPrices()
    {
      var ship = ShipController.Instance.ShipConfig;

      var fuelPrice = ship.fuelCostPerLitre;
      var hullPrice = ship.hullCostPerUnit;

      var fuelDelta = ship.FuelMax - ship.FuelCurrent;
      var hullDelta = ship.HullStrengthMax - ship.HullStrengthCurrent;

      var fuelDeltaPrice = fuelDelta * fuelPrice;
      var hullDeltaPrice = hullDelta * hullPrice;

      var fuelDeltaPriceHalf = fuelDeltaPrice / 2;
      var hullDeltaPriceHalf = hullDeltaPrice / 2;

      fuelDeltaPrice = Mathf.RoundToInt(fuelDeltaPrice);
      hullDeltaPrice = Mathf.RoundToInt(hullDeltaPrice);

      refuelAllCost.text = "$" + fuelDeltaPrice.ToString();
      refuelHalfCost.text = "$" + (fuelDeltaPrice / 2).ToString();

      repairAllCost.text = "$" + hullDeltaPrice.ToString();
      repairHalfCost.text = "$" + (hullDeltaPrice / 2).ToString();

      playerCreditsAmount.text = "$" + playerInventory.GetCreditsAmount;

      //PlayerCredits
      if (playerInventory.GetCreditsAmount < 0) {
        playerCreditsAmount.color = Color.red;
      } else {
        playerCreditsAmount.color = Color.black;
      }
      
      #region Refuel
      //Refuel all
      if (playerInventory.GetCreditsAmount < fuelDeltaPrice || fuelDeltaPrice == 0) {
        refuelAllButton.interactable = false;

        if (ship.FuelCurrent == ship.FuelMax) {
          refuelAllCost.color = Color.grey;
        } else {
          refuelAllCost.color = Color.red;
        }
      } else {
        refuelAllButton.interactable = true;
        refuelAllCost.color = Color.black;
      }

      //Refuel half
      if (playerInventory.GetCreditsAmount < fuelDeltaPriceHalf || fuelDeltaPriceHalf == 0) {
        refuelHalfButton.interactable = false;

        if (ship.FuelCurrent == ship.FuelMax) {
          refuelHalfCost.color = Color.grey;
        } else {
          refuelHalfCost.color = Color.red;
        }
      }
      else {
        refuelHalfButton.interactable = true;
        refuelHalfCost.color = Color.black;
      }

      //refuel $25
      if (playerInventory.GetCreditsAmount < 25 || fuelDeltaPrice < 25) {
        refuel25Button.interactable = false;

        if (fuelDeltaPrice < 25) {
          refuel25Cost.color = Color.grey;
        } else {
          refuel25Cost.color = Color.red;
        }
      }
      else {
        refuel25Button.interactable = true;
        refuel25Cost.color = Color.black;
      }

      //refuel $10
      if (playerInventory.GetCreditsAmount < 10 || fuelDeltaPrice < 10) {
        refuel10Button.interactable = false;

        if (fuelDeltaPrice < 10) {
          refuel10Cost.color = Color.grey;
        } else {
          refuel10Cost.color = Color.red;
        }
      }
      else {
        refuel10Button.interactable = true;
        refuel10Cost.color = Color.black;
      }
      #endregion

      #region Repair
      //Reapir all
      if (playerInventory.GetCreditsAmount < hullDeltaPrice || hullDeltaPrice == 0) {
        repairAllButton.interactable = false;

        if (ship.HullStrengthCurrent == ship.HullStrengthMax) {
          repairAllCost.color = Color.grey;
        } else {
          repairAllCost.color = Color.red;
        }
      }
      else {
        repairAllButton.interactable = true;
        repairAllCost.color = Color.black;
      }
      
      //Repair half
      if (playerInventory.GetCreditsAmount < hullDeltaPriceHalf || hullDeltaPriceHalf == 0) {
        repairHalfButton.interactable = false;

        if (ship.HullStrengthCurrent == ship.HullStrengthMax) {
          repairHalfCost.color = Color.grey;
        } else {
          repairHalfCost.color = Color.red;
        }
      }
      else {
        repairHalfButton.interactable = true;
        repairHalfCost.color = Color.black;
      }

      //Reapir $100
      if (playerInventory.GetCreditsAmount < 25 || hullDeltaPrice < 25) {
        repair25Button.interactable = false;

        if (hullDeltaPrice < 25) {
          repair25Cost.color = Color.grey;
        } else {
          repair25Cost.color = Color.red;
        }
      } 
      else {
        repair25Button.interactable = true;
        repair25Cost.color = Color.black;
      }

      //Repair $50
      if (playerInventory.GetCreditsAmount < 10 || hullDeltaPrice < 10) {
        repair10Button.interactable = false;

        if (hullDeltaPrice < 10) {
          repair10Cost.color = Color.grey;
        } else {
          repair10Cost.color = Color.red;
        }
      } 
      else {
        repair10Button.interactable = true;
        repair10Cost.color = Color.black;
      }
      #endregion
    }

    private void UpdateUpgradesMenu()
    {
      var ship = ShipController.Instance.ShipConfig;
      var playerCredits = ShipController.Instance.Inventory.GetCreditsAmount;

      //Engine upgrade availability
      if (playerCredits < ship.impulseEngineCost) {
        impulseEngineUpgrade.interactable = false;
        impulseEngineUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
      } else {
        impulseEngineUpgrade.interactable = true;
        impulseEngineUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
      }

      if (playerCredits < ship.ionEngineCost) {
        ionEngineUpgrade.interactable = false;
        ionEngineUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
      } else {
        ionEngineUpgrade.interactable = true;
        ionEngineUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
      }

      if (playerCredits < ship.warpDriveCost) {
        warpEngineUpgrade.interactable = false;
        warpEngineUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
      } else {
        warpEngineUpgrade.interactable = true;
        warpEngineUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
      }

      if (playerCredits < ship.hyperDriveCost) {
        hyperEngineUpgrade.interactable = false;
        hyperEngineUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
      } else {
        hyperEngineUpgrade.interactable = true;
        hyperEngineUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
      }

      //Weapon upgrade availability
      if (playerCredits < ship.photonWeaponCost) {   //Photon weapon
        photonWeaponUpgrade.interactable = false;
        photonWeaponUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
      } else {
        photonWeaponUpgrade.interactable = true;
        photonWeaponUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
      }

      if (playerCredits < ship.plasmaWeaponCost) {   //Plasma weapon
        plasmaWeaponUpgrade.interactable = false;
        plasmaWeaponUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
      } else {
        plasmaWeaponUpgrade.interactable = true;
        plasmaWeaponUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
      }

      if (playerCredits < ship.ionWeaponCost) {   //Ion weapon
        ionWeaponUpgrade.interactable = false;
        ionWeaponUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
      } else {
        ionWeaponUpgrade.interactable = true;
        ionWeaponUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
      }

      if (playerCredits < ship.darkWeaponCost) {   //Dark weapon
        darkWeaponUpgrade.interactable = false;
        darkWeaponUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
      } else {
        darkWeaponUpgrade.interactable = true;
        darkWeaponUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
      }

      //Hull upgrade availability
      if (playerCredits < ship.titaniumHullCost) {   //Titanium hull
        titaniumHullUpgrade.interactable = false;
        titaniumHullUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
      } else {
        titaniumHullUpgrade.interactable = true;
        titaniumHullUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
      }

      if (playerCredits < ship.cobaltHullCost) {   //Cobalt hull
        cobaltHullUpgrade.interactable = false;
        cobaltHullUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
      } else {
        cobaltHullUpgrade.interactable = true;
        cobaltHullUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
      }
      
      if (playerCredits < ship.stellariteHullCost) {   //Stellarite hull
        stellariteHullUpgrade.interactable = false;
        stellariteHullUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
      } else {
        stellariteHullUpgrade.interactable = true;
        stellariteHullUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
      }

      if (playerCredits < ship.darkoreHullCost) {   //Darkore hull
        darkHullUpgrade.interactable = false;
        darkHullUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
      } else {
        darkHullUpgrade.interactable = true;
        darkHullUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
      }

      //FuelTank upgrade availability
      if (playerCredits < ship.mediumTankCost) {   //Medium tank
        mediumTankUpgrade.interactable = false;
        mediumTankUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
      } else {
        mediumTankUpgrade.interactable = true;
        mediumTankUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
      }

      if (playerCredits < ship.largeTankCost) {   //Large tank
        largeTankUpgrade.interactable = false;
        largeTankUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
      } else {
        largeTankUpgrade.interactable = true;
        largeTankUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
      }

      if (playerCredits < ship.hugeTankCost) {   //Huge tank
        hugeTankUpgrade.interactable = false;
        hugeTankUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
      } else {
        hugeTankUpgrade.interactable = true;
        hugeTankUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
      }

      if (playerCredits < ship.megaTankCost) {   //Huge tank
        megaTankUpgrade.interactable = false;
        megaTankUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
      } else {
        megaTankUpgrade.interactable = true;
        megaTankUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
      }

      //CargoBay upgrade availability
      if (playerCredits < ship.smallCargoBayCost) {   //Small bay
        smallBayUpgrade.interactable = false;
        smallBayUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
      } else {
        smallBayUpgrade.interactable = true;
        smallBayUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
      }

      if (playerCredits < ship.mediumCargoBayCost) {   //Medium bay
        mediumBayUpgrade.interactable = false;
        mediumBayUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
      } else {
        mediumBayUpgrade.interactable = true;
        mediumBayUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
      }

      if (playerCredits < ship.largeCargoBayCost) {   //Large bay
        largeBayUpgrade.interactable = false;
        largeBayUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
      } else {
        largeBayUpgrade.interactable = true;
        largeBayUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
      }

      if (playerCredits < ship.hugeCargoBayCost) {   //Huge bay
        hugeBayUpgrade.interactable = false;
        hugeBayUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
      } else {
        hugeBayUpgrade.interactable = true;
        hugeBayUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
      }

      //--------------------------------------------------------------------------------

      //Engine
      switch (ship.ShipEngine)
      {
        case ShipEngine.RocketEngine:
          currentEngineText.text = "Engine: Rocket";
          break;
        case ShipEngine.ImuplseEngine:
          currentEngineText.text = "Engine: Impulse";
          impulseEngineUpgrade.interactable = false;
          impulseEngineUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;
          break;
        case ShipEngine.IonEngine:
          currentEngineText.text = "Engine: Ion";
          ionEngineUpgrade.interactable = false;
          ionEngineUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;

          impulseEngineUpgrade.interactable = false;
          impulseEngineUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;
          break;
        case ShipEngine.WarpDrive:
          currentEngineText.text = "Engine: Warp";
          warpEngineUpgrade.interactable = false;
          warpEngineUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;

          ionEngineUpgrade.interactable = false;
          ionEngineUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;

          impulseEngineUpgrade.interactable = false;
          impulseEngineUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;
          break;
        case ShipEngine.HyperDrive:
          currentEngineText.text = "Engine: Hyper";
          hyperEngineUpgrade.interactable = false;
          hyperEngineUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;

          warpEngineUpgrade.interactable = false;
          warpEngineUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;

          ionEngineUpgrade.interactable = false;
          ionEngineUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;

          impulseEngineUpgrade.interactable = false;
          impulseEngineUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;
          break;
      }


      //Weapon
      switch (ship.ShipWeapon)
      {
        case ShipWeapon.StandardPhaser:
          currentWeaponText.text = "Phaser: Standard";
          break;
        case ShipWeapon.PhotonCannon:
          currentWeaponText.text = "Phaser: Photon";
          photonWeaponUpgrade.interactable = false;
          photonWeaponUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;
          break;
        case ShipWeapon.PlasmaCannon:
          currentWeaponText.text = "Phaser: Plasma";
          photonWeaponUpgrade.interactable = false;
          photonWeaponUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;

          plasmaWeaponUpgrade.interactable = false;
          plasmaWeaponUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;
          break;
        case ShipWeapon.IonCannon:
          currentWeaponText.text = "Phaser: IonCannon";
          photonWeaponUpgrade.interactable = false;
          photonWeaponUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;

          plasmaWeaponUpgrade.interactable = false;
          plasmaWeaponUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;

          ionWeaponUpgrade.interactable = false;
          ionWeaponUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;
          break;
        case ShipWeapon.DarkCannon:
          currentWeaponText.text = "Phaser: DarkCannon";
          photonWeaponUpgrade.interactable = false;
          photonWeaponUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;

          plasmaWeaponUpgrade.interactable = false;
          plasmaWeaponUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;

          ionWeaponUpgrade.interactable = false;
          ionWeaponUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;

          darkWeaponUpgrade.interactable = false;
          darkWeaponUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;
          break;
      }

      //Hull
      switch (ship.ShipsHull)
      {
        case ShipHull.SteelHull:
          currentHullText.text = "Hull: Steel";
          break;
        case ShipHull.TitaniumHull:
          currentHullText.text = "Hull: Titanium";
          titaniumHullUpgrade.interactable = false;
          titaniumHullUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;
          break;
        case ShipHull.CobaltHull:
          currentHullText.text = "Hull: Cobalt";
          titaniumHullUpgrade.interactable = false;
          titaniumHullUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;

          cobaltHullUpgrade.interactable = false;
          cobaltHullUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;
          break;
        case ShipHull.StellariteHull:
          currentHullText.text = "Hull: Stellarite";
          titaniumHullUpgrade.interactable = false;
          titaniumHullUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;

          cobaltHullUpgrade.interactable = false;
          cobaltHullUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;

          stellariteHullUpgrade.interactable = false;
          stellariteHullUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;
          break;
        case ShipHull.DarkoreHull:
          currentHullText.text = "Hull: Darkore";
          titaniumHullUpgrade.interactable = false;
          titaniumHullUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;

          cobaltHullUpgrade.interactable = false;
          cobaltHullUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;

          stellariteHullUpgrade.interactable = false;
          stellariteHullUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;

          darkHullUpgrade.interactable = false;
          darkHullUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;
          break;
      }


      //Tank
      switch (ship.ShipsFuelTank)
      {
        case ShipFuelTank.SmallTank:
          currentTankText.text = "Tank: Smallk";
          break;
        case ShipFuelTank.MediumTank:
          currentTankText.text = "Tank: Medium";
          mediumTankUpgrade.interactable = false;
          mediumTankUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;
          break;
        case ShipFuelTank.LargeTank:
          currentTankText.text = "Tank: Large";
          mediumTankUpgrade.interactable = false;
          mediumTankUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;

          largeTankUpgrade.interactable = false;
          largeTankUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;
          break;
        case ShipFuelTank.HugeTank:
          currentTankText.text = "Tank: Huge";
          mediumTankUpgrade.interactable = false;
          mediumTankUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;

          largeTankUpgrade.interactable = false;
          largeTankUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;

          hugeTankUpgrade.interactable = false;
          hugeTankUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;
          break;
        case ShipFuelTank.MegaTank:
          currentTankText.text = "Tank: Mega";
          mediumTankUpgrade.interactable = false;
          mediumTankUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;

          largeTankUpgrade.interactable = false;
          largeTankUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;

          hugeTankUpgrade.interactable = false;
          hugeTankUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;

          megaTankUpgrade.interactable = false;
          megaTankUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;
          break;
      }

      //Cargo Bay
      switch (ship.ShipsCargoBay)
      {
        case ShipCargoBay.TinyCargoBay:
          currentBayText.text = "Cargo: Tiny";
          break;
        case ShipCargoBay.SmallCargoBay:
          currentBayText.text = "Cargo: Small";
          smallBayUpgrade.interactable = false;
          smallBayUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;
          break;
        case ShipCargoBay.MediumCargoBay:
          currentBayText.text = "Cargo: Medium";
          smallBayUpgrade.interactable = false;
          smallBayUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;

          mediumBayUpgrade.interactable = false;
          mediumBayUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;
          break;
        case ShipCargoBay.LargeCargoBay:
          currentBayText.text = "Cargo: Large";
          smallBayUpgrade.interactable = false;
          smallBayUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;

          mediumBayUpgrade.interactable = false;
          mediumBayUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;

          largeBayUpgrade.interactable = false;
          largeBayUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;
          break;
        case ShipCargoBay.HugeCargoBay:
          currentBayText.text = "Cargo: Huge";
          smallBayUpgrade.interactable = false;
          smallBayUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;

          mediumBayUpgrade.interactable = false;
          mediumBayUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;

          largeBayUpgrade.interactable = false;
          largeBayUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;

          hugeBayUpgrade.interactable = false;
          hugeBayUpgrade.GetComponentInChildren<TextMeshProUGUI>().color = Color.grey;
          break;
      }
    }

    #region Ship UI
    private void UpdateHullUI()
    {
      int hullStrength = ShipController.Instance.Health;
      int maxHullStrength = ShipController.Instance.MaxHealth;

      float healthPercentage = (float)hullStrength / (float)maxHullStrength;
      
      if (healthPercentage == float.NaN) 
      {
        Debug.LogError("Health percentage is NaN");
        return;
      }

      healthSlider.value = healthPercentage;
    }

    private void UpdateFuelUI()
    {
      float fuelCurrent = ShipController.Instance.Fuel;
      float fuelMax = ShipController.Instance.MaxFuel;

      float fuelPercentage = fuelCurrent / fuelMax;

      if (fuelPercentage == float.NaN)
      {
        Debug.LogError("Fuel percentage is NaN");
        return;
      }

      fuelSlider.value = fuelPercentage;
    }

    private void UpdateCargoUI()
    {
      float cargoCurrent = ShipController.Instance.CargoCurrentCapacity;
      float cargoMax = ShipController.Instance.CargoMaxCapacity;

      float cargoPercentage = cargoCurrent / cargoMax;

      if (cargoPercentage == float.NaN)
      {
        Debug.LogError("Cargo percentage is NaN");
        return;
      }

      cargoSlider.value = cargoPercentage;
    }
    #endregion
  }
}