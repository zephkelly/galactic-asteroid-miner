using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class AsteroidPickup : MonoBehaviour
  {
    private Rigidbody2D rigid2D;
    private Collider2D pickupCollider;

    private void Awake()
    {
      rigid2D = GetComponent<Rigidbody2D>();
      pickupCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
      if (!otherCollider.CompareTag("Player")) return;

      //Lerp to player
    }
  }
}