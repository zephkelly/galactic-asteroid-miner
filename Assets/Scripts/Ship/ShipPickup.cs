using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace zephkelly
{
  public class ShipPickup : MonoBehaviour
  {
    private Collider2D shipPickupTrigger;
    private Rigidbody2D rigid2D;
    private const float pickupSpeed = 0.95f;

    private void Awake()
    {
      shipPickupTrigger = GetComponent<Collider2D>();
      rigid2D = shipPickupTrigger.attachedRigidbody;
    }

    private async void OnTriggerEnter2D(Collider2D otherCollider)
    {
      if (!otherCollider.CompareTag("AsteroidPickup")) return;

      otherCollider.isTrigger = true;
      var asteroidInfo = otherCollider.GetComponent<AsteroidController>().AsteroidInfo;

      //Wait for the pickup to get in range
      Task lerpTask = PickupLerp(otherCollider.transform, asteroidInfo.Type);
      await Task.WhenAll(lerpTask);

      zephkelly.AudioManager.Instance.PlaySoundRandomPitch("ShipPickup", 0.95f, 1.05f);

      //Add to inventory and destroy
      ShipController.Instance.Inventory.AddItem(asteroidInfo.Type.ToString(), 1);
      ShipUIManager.Instance.AddNewPickupText($"+{asteroidInfo.Type.ToString()}");

      //Update stat manager
      GameManager.Instance.StatisticsManager.IncrementPickupsCollected();

      if (OcclusionManager.Instance.RemoveAsteroid.ContainsKey(asteroidInfo)) return;
      OcclusionManager.Instance.RemoveAsteroid.Add(asteroidInfo, asteroidInfo.ParentChunk);
    }

    private async Task PickupLerp(Transform pickupTransform, AsteroidType pickupType)
    {
      Vector2 directionToPlayer;
      Vector2 shipPosition;
      float distanceToPlayer;
      float lerpTime = 0;

      do
      {
        if (pickupTransform == null) break;
        if (rigid2D == null) break;

        shipPosition = rigid2D.position;

        directionToPlayer = this.transform.position - pickupTransform.position;
        directionToPlayer.Normalize();

        distanceToPlayer = Vector2.Distance(this.transform.position, pickupTransform.position);
        float force = 2 / distanceToPlayer;

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
