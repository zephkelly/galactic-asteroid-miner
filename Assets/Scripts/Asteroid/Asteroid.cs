using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public enum AsteroidSize
  {
    Pickup,
    Small,
    Medium,
    Large,
    ExtraLarge
  }

  public enum AsteroidType
  {
    Iron,
    Platinum,
    Gold,
    Palladium,
    Cobalt,
    Stellarite,
    Darkore
  }

  public class Asteroid
  {
    public AsteroidType Type { get; set; }
    public AsteroidSize Size { get; set; }
    public Chunk ParentChunk { get; set; }

    public GameObject AsteroidObject { get; set; }
    public Transform AsteroidTransform { get; set; }
    public Collider2D Collider { get; set; }
    public Rigidbody2D Rigid2D { get; set; }
    public SpriteRenderer Renderer { get; set; }

    public Vector2 SpawnPosition { get; set; }
    public Vector2 CurrentPosition { get; set; }

    public int Health { get; set; }
    public bool HasBeenActive { get; set; }
  }
}