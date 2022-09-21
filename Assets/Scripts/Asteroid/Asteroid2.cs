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
    Huge,
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

  public class Asteroid2
  {
    private Vector2 spawnPosition;
    private Vector2 currentPosition;

    private GameObject asteroidObject;
    private Chunk2 parentChunk;

    private AsteroidSize asteroidSize;
    private AsteroidType asteroidType;

    private int health;

    //------------------------------------------------------------------------------
    
    public Vector2 SpawnPoint { get => spawnPosition; }
    public Vector2 Position { get => currentPosition; }

    public GameObject AsteroidObject { get => asteroidObject; }
    public Chunk2 ParentChunk { get => parentChunk; }

    public AsteroidSize Size { get => asteroidSize; }
    public AsteroidType Type { get => asteroidType; }

    public int Health { get => health; }

    public Asteroid2(Chunk2 _parentChunk, AsteroidSize _size, AsteroidType _type, Vector2 _position, int _health)
    {
      parentChunk = _parentChunk;
      asteroidSize = _size;
      asteroidType = _type;

      spawnPosition = _position;
      health = _health;
    }

    public int UpdateHealth(int damage)
    {
      health -= damage;
      return health;
    }

    public void UpdatePosition(Vector2 newPosition) => currentPosition = newPosition;

    public void SetSpawnToPosition() => spawnPosition = currentPosition;
  }
}
