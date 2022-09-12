using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  [CreateAssetMenu(fileName = "Inventory", menuName = "ScriptableObjects/Inventory", order = 0)]
  public class Inventory : ScriptableObject
  {
    //public event System.Action OnInventoryChanged;
    
    [SerializeField] Dictionary <string, int> inventory = new Dictionary<string, int>();

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
        Debug.Log("Item not found");
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
        Debug.Log("Item not found");
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
