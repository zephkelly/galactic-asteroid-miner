using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class ShipPickup : MonoBehaviour
  {
    [SerializeField] ShipController shipController;
    private Rigidbody2D rigid2D;
    private Collider2D pickupCollider;

    private void Awake()
    {
      rigid2D = GetComponent<Rigidbody2D>();
      pickupCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
      if (otherCollider == null) return;
      if (!otherCollider.CompareTag("AsteroidPickup")) return;

      var asteroidType = otherCollider.GetComponent<AsteroidController>().AsteroidType;

      shipController.Inventory.AddItem(asteroidType.ToString(), 1);
      shipController.Inventory.PrintInventory();

      Destroy(otherCollider.gameObject);
    }
  }
}
