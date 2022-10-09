using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class MenuStarDestructor : MonoBehaviour
  {
    void OnoCollisionEnter2D(Collision2D collision)
    {
      Destroy(collision.gameObject);
    }
  }
}
