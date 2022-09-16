using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class AsteroidInformation
  {
    public AsteroidType Type { get; set; }
    public AsteroidSize Size { get; set; }

    public GameObject GameObject { get; set; }
    public Collider2D Collider { get; set; }
    public Rigidbody2D Rigid2D { get; set; }
    public SpriteRenderer Renderer { get; set; }

    public Vector2 Position { get; set; }
    public int Health { get; set; }
  }
}