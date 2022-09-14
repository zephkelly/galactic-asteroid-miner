using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace zephkelly
{
  public class AsteroidPickup : MonoBehaviour
  {
    [SerializeField] Transform pickupTransform;
    [SerializeField] Collider2D pickupCollider;
    private Collider2D pickupTriggerCollider;
    private Rigidbody2D rigid2D;

    private const float pickupSpeed = 0.8f;

    private void Awake()
    {
      pickupTriggerCollider = GetComponent<Collider2D>();
      rigid2D = pickupTriggerCollider.attachedRigidbody;
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
      if (!otherCollider.CompareTag("Player")) return;

      pickupCollider.isTrigger = true;
      pickupTriggerCollider.enabled = false;
      PickupLerp(otherCollider.transform);
    }

    private async void PickupLerp(Transform playerTransform)
    {
      Vector2 directionToPlayer;
      float distanceToPlayer;
      float lerpTime = 0;

      do
      {
        if (pickupTransform == null) break;   //Exit if the pickup has been destroyed

        directionToPlayer = playerTransform.position - pickupTransform.position;
        directionToPlayer.Normalize();

        distanceToPlayer = Vector2.Distance(playerTransform.position, pickupTransform.position);
        float force = 1 / distanceToPlayer;

        //lerp from our position to player position by time and force
        pickupTransform.position = Vector2.Lerp(
          pickupTransform.position, 
          playerTransform.position, 
          lerpTime * force * pickupSpeed);

        lerpTime += Time.deltaTime;

        await Task.Yield();
      }
      while (distanceToPlayer > 0.1f);
    }
  }
}