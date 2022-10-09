using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class StarDestruction : MonoBehaviour
  {
    private void OnCollisionEnter2D(Collision2D collision)
    {
      if (collision.gameObject.tag == "Asteroid" || collision.gameObject.tag == "AsteroidPickup")
      {
        var asteroidInfo = collision.gameObject.GetComponent<AsteroidController>().AsteroidInfo;

        if (OcclusionManager.Instance.RemoveAsteroid.ContainsKey(asteroidInfo))
        {
          Destroy(gameObject);
          return;
        }
        OcclusionManager.Instance.RemoveAsteroid.Add(asteroidInfo, asteroidInfo.ParentChunk);
      }
    }
  }
}