using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class ShipAsteroidPickup : MonoBehaviour
  {
    private void OnCollisionEnter2D(Collision2D collision)
    {
      if (collision.gameObject.tag == "AsteroidPickup")
      {
        int asteroidValue = collision.gameObject.GetComponent<AsteroidPickup>().Amount;

        Debug.Log("Asteroid Pickup: " + asteroidValue);

        Destroy(collision.gameObject);

        //Instantiate itempickuptext with the weight of the asteroid
      }
    }
  }
}