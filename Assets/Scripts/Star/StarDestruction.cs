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
        Destroy(collision.gameObject);
      }

      if (collision.gameObject.tag == "Player")
      {
        Destroy(collision.gameObject);
        Debug.Log("Player destroyed");	
      }
    }
  }
}