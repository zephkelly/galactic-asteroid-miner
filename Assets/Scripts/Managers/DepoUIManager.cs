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
    }

    private void Start()
    {
      #region Depo Steup
      shipConfig = ShipController.Instance.ShipConfig;
      playerInventory = ShipController.Instance.Inventory;

      depotUIObject = GameObject.Find("DepoUI");
      tooltipUIElement = GameObject.Find("TooltipUI").GetComponent<TextMeshProUGUI>();

      creditsAmount = GameObject.Find("CreditsAmount").GetComponent<TextMeshProUGUI>();

      refuelAllCost = GameObject.Find("RefuelAllCost").GetComponent<TextMeshProUGUI>();
      refuelHalfCost = GameObject.Find("RefuelHalfCost").GetComponent<TextMeshProUGUI>();
      refuel100 = GameObject.Find("Refuel100").GetComponent<TextMeshProUGUI>();
      refuel50 = GameObject.Find("Refuel50").GetComponent<TextMeshProUGUI>();
      repairAllCost = GameObject.Find("RepairAllCost").GetComponent<TextMeshProUGUI>();
      repairHalfCost = GameObject.Find("RepairHalfCost").GetComponent<TextMeshProUGUI>();
      repair100 = GameObject.Find("Repair100").GetComponent<TextMeshProUGUI>();
      repair50 = GameObject.Find("Repair50").GetComponent<TextMeshProUGUI>();

      ironCount = GameObject.Find("IronCount").GetComponent<TextMeshProUGUI>();
      platinumCount = GameObject.Find("PlatinumCount").GetComponent<TextMeshProUGUI>();
      titaniumCount = GameObject.Find("TitaniumCount").GetComponent<TextMeshProUGUI>();
      goldCount = GameObject.Find("GoldCount").GetComponent<TextMeshProUGUI>();
      palladiumCount = GameObject.Find("PalladiumCount").GetComponent<TextMeshProUGUI>();
      cobaltCount = GameObject.Find("CobaltCount").GetComponent<TextMeshProUGUI>();
      stellariteCount = GameObject.Find("StellariteCount").GetComponent<TextMeshProUGUI>();
      darkoreCount = GameObject.Find("DarkoreCount").GetComponent<TextMeshProUGUI>();

      ironValue = GameObject.Find("IronPrice").GetComponent<TextMeshProUGUI>();
      platinumValue = GameObject.Find("PlatinumPrice").GetComponent<TextMeshProUGUI>();
      titaniumValue = GameObject.Find("TitaniumPrice").GetComponent<TextMeshProUGUI>();
      goldValue = GameObject.Find("GoldPrice").GetComponent<TextMeshProUGUI>();
      palladiumValue = GameObject.Find("PalladiumPrice").GetComponent<TextMeshProUGUI>();
      cobaltValue = GameObject.Find("CobaltPrice").GetComponent<TextMeshProUGUI>();
      stellariteValue = GameObject.Find("StellaritePrice").GetComponent<TextMeshProUGUI>();
      darkoreValue = GameObject.Find("DarkorePrice").GetComponent<TextMeshProUGUI>();

      totalValue = GameObject.Find("TotalMineralPrice").GetComponent<TextMeshProUGUI>();

      depotUIObject.SetActive(false);
      OnHoverDepo.AddListener(DepoMenuEnable);
      OnLeaveDepoHover.AddListener(DepoMenuDisable);
      #endregion

      #region Ship Setup
      fuelSlider = GameObject.Find("ShipFuelUI").GetComponent<Slider>();
      healthSlider = GameObject.Find("ShipHullUI").GetComponent<Slider>();

      OnUpdateFuel.AddListener(UpdateFuelUI);
      OnUpdateHull.AddListener(UpdateHullUI);
      #endregion
    }

    private void Update()
    {
      if (depotUI) DepotMenuBehaviour();
    }

    #region Depo UI
    private TextMeshProUGUI creditsAmount;

    private TextMeshProUGUI refuelAllCost;
    private TextMeshProUGUI refuelHalfCost;
    private TextMeshProUGUI refuel100;
    private TextMeshProUGUI refuel50;
    private TextMeshProUGUI repairAllCost;
    private TextMeshProUGUI repairHalfCost;
    private TextMeshProUGUI repair100;
    private TextMeshProUGUI repair50;

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

    private ShipConfiguration shipConfig;
    private Inventory playerInventory;
    private GameObject depotUIObject;

    internal bool depotUI;
    internal bool depotToggle;

    public void MadeAPurchase()
    {
      UpdateOreOverlay();
      UpdateDepoPrices();
    }

    private void DepoMenuEnable()
    {
      depotUI = true;
      depotToggle = false;

      UpdateOreOverlay();
      UpdateDepoPrices();

      tooltipUIElement.color = new Color(1, 1, 1, 1);
    }

    private void DepoMenuDisable()
    {
      depotUI = false;

      tooltipUIElement.color = new Color(1, 1, 1, 0);

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
    }

    private void UpdateOreOverlay()
    {
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

      var fuelPrice = ShipController.Instance.ShipConfig.fuelCostPerLitre;
      var hullPrice = ShipController.Instance.ShipConfig.hullCostPerUnit;

      var fuelDeltaPrice = (ship.FuelMax - ship.FuelCurrent) * fuelPrice;
      var hullDeltaPrice = (ship.HullStrengthMax - ship.HullStrengthCurrent) * hullPrice;

      fuelDeltaPrice = Mathf.RoundToInt(fuelDeltaPrice);
      hullDeltaPrice = Mathf.RoundToInt(hullDeltaPrice);

      refuelAllCost.text = "$" + fuelDeltaPrice.ToString();
      refuelHalfCost.text = "$" + (fuelDeltaPrice / 2).ToString();

      repairAllCost.text = "$" + hullDeltaPrice.ToString();
      repairHalfCost.text = "$" + (hullDeltaPrice / 2).ToString();

      creditsAmount.text = "$" + playerInventory.GetCreditsAmount();

      if (playerInventory.GetCreditsAmount() < fuelDeltaPrice)
      {
        refuelAllCost.color = Color.red;
        refuelHalfCost.color = Color.red;
      }
      else
      {
        refuelAllCost.color = Color.black;
        refuelHalfCost.color = Color.black;
      }

      if (playerInventory.GetCreditsAmount() < hullDeltaPrice)
      {
        repairAllCost.color = Color.red;
        repairHalfCost.color = Color.red;
      }
      else
      {
        repairAllCost.color = Color.black;
        repairHalfCost.color = Color.black;
      }

      if (playerInventory.GetCreditsAmount() < fuelDeltaPrice && 
        playerInventory.GetCreditsAmount() < hullDeltaPrice)
      {
        creditsAmount.color = Color.red;
      }
      else
      {
        creditsAmount.color = Color.black;
      }

      if (playerInventory.GetCreditsAmount() < 100)
      {
        repair100.color = Color.red;
        refuel100.color = Color.red;
      }
      else
      {
        repair100.color = Color.black;
        refuel100.color = Color.black;
      }
      
      if (playerInventory.GetCreditsAmount() < 50)
      {
        repair50.color = Color.red;
        refuel50.color = Color.red;
      }
      else
      {
        repair50.color = Color.black;
        refuel50.color = Color.black;
      }
    }
    #endregion

    #region Ship UI
    private Slider healthSlider;
    private Slider fuelSlider;

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
    #endregion
  }
}