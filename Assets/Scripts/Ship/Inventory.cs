using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  [CreateAssetMenu(menuName = "ScriptableObjects/Inventory")]
  public class Inventory : ScriptableObject
  {
    private Dictionary <string, int> inventory = new Dictionary<string, int>();

    //------------------------------------------------------------------------------

    public void AddItem(string item, int amount)
    {
      if (inventory.ContainsKey(item))
      {
        inventory[item] += amount;
      }
      else
      {
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
      inventory.Clear();
    }
  }
}
