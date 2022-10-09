using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  [CreateAssetMenu(menuName = "ScriptableObjects/Inventory")]
  public class Inventory : ScriptableObject
  {
    private Dictionary <string, int> inventory = new Dictionary<string, int>();
    public long credits = 0;

    //------------------------------------------------------------------------------

    public long GetCreditsAmount { get => credits; }

    public void AddItem(string item, int amount)
    {
      var shipConfig = ShipController.Instance.ShipConfig;

      if (inventory.ContainsKey(item))
      {
        if (shipConfig.CargoBayCurrentCapacity >= shipConfig.CargoBayMaxCapacity) {
          return;
        }

        inventory[item] += amount;
        shipConfig.AddItemToCargo(amount);
        DepoUIManager.Instance.OnUpdateCargo?.Invoke();
      }
      else {
        inventory.Add(item, amount);
      }
    }

    public void RemoveItem(string item, int amount)
    {
      if (inventory.ContainsKey(item))
      {
        inventory[item] -= amount;
      }
      else
      {
        Debug.LogError("Item not found");
      }
    }

    public void AddMoney(int amount)
    {
      credits += amount;

      GameManager.Instance.StatisticsManager.IncrementCreditsGained(amount);
    }

    public void RemoveMoney(int amount)
    {
      credits -= amount;

      //Update stats manager
      GameManager.Instance.StatisticsManager.IncrementCreditsSpent(amount);
    }

    public int GetItemAmount(string item)
    {
      if (inventory.ContainsKey(item))
      {
        return inventory[item];
      }
      else
      {
        Debug.LogError("Item not found");
        return 0;
      }
    }

    public void PrintInventory()
    {
      foreach (KeyValuePair<string, int> item in inventory)
      {
        Debug.Log(item.Key + " " + item.Value);
      }
    }

    public void ClearInventory()
    {
      ShipController.Instance.ShipConfig.ClearCargoBay();
      inventory.Clear();

      AddItem(AsteroidType.Iron.ToString(), 0);
      AddItem(AsteroidType.Platinum.ToString(), 0);
      AddItem(AsteroidType.Titanium.ToString(), 0);
      AddItem(AsteroidType.Gold.ToString(), 0);
      AddItem(AsteroidType.Palladium.ToString(), 0);
      AddItem(AsteroidType.Cobalt.ToString(), 0);
      AddItem(AsteroidType.Stellarite.ToString(), 0);
      AddItem(AsteroidType.Darkore.ToString(), 0);
    }

    public void ClearCredits()
    {
      credits = 0;
    }
  }
}
