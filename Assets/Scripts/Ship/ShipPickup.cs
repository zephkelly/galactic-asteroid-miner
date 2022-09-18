using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace zephkelly
{
  public class ShipPickup : MonoBehaviour
  {
    [SerializeField] ShipController shipController;
    private Collider2D shipPickupTrigger;
    private Rigidbody2D rigid2D;

    private const float pickupSpeed = 0.8f;

    private void Awake()
    {
      shipPickupTrigger = GetComponent<Collider2D>();
      rigid2D = shipPickupTrigger.attachedRigidbody;
    }

    private async void OnTriggerEnter2D(Collider2D otherCollider)
    {
      if (!otherCollider.CompareTag("AsteroidPickup")) return;

      otherCollider.isTrigger = true;
      var asteroidType = otherCollider.GetComponent<AsteroidController>().AsteroidInfo.Type;

      //Wait for the pickup to get in range
      Task lerpTask = PickupLerp(otherCollider.transform, asteroidType);
      await Task.WhenAll(lerpTask);

      //Add to inventory and destroy
      shipController.Inventory.AddItem(asteroidType.ToString(), 1);
      shipController.Inventory.PrintInventory();

      Destroy(otherCollider.gameObject);
    }

    private async Task PickupLerp(Transform pickupTransform, AsteroidType pickupType)
    {
      Vector2 directionToPlayer;
      Vector2 shipPosition;
      float distanceToPlayer;
      float lerpTime = 0;

      do
      {
        if (pickupTransform == null) break;   //Exit if the pickup has been destroyed

        shipPosition = rigid2D.position;

        directionToPlayer = this.transform.position - pickupTransform.position;
        directionToPlayer.Normalize();

        distanceToPlayer = Vector2.Distance(this.transform.position, pickupTransform.position);
        float force = 1 / distanceToPlayer;

        //lerp from our position to player position by time and force
        pickupTransform.position = Vector2.Lerp(
          pickupTransform.position, 
          this.transform.position, 
          lerpTime * force * pickupSpeed);

        lerpTime += Time.deltaTime;
        await Task.Yield();
      }
      while (distanceToPlayer > 0.1f);
    }
  }
}
