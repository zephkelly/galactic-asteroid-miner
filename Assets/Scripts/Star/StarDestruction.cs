using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class StarDestruction : MonoBehaviour
  {
    private void OnCollisionEnter2D(Collision2D collision)
    {
      if (collision.gameObject.tag == "Asteroid")
      {
        var asteroidInfo = collision.gameObject.GetComponent<AsteroidController>().AsteroidInfo;

        OcclusionManager.Instance.RemoveAsteroid.Add(asteroidInfo, asteroidInfo.ParentChunk);
      }

      /*
      if (collision.gameObject.tag == "Player")
      {
        Destroy(collision.gameObject);
        Debug.Log("Player destroyed");
      }
      */
    }
  }
}