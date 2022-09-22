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
    private Transform asteroidTransform;
    private SpriteRenderer renderer;
    private Chunk2 parentChunk;

    private AsteroidSize asteroidSize;
    private AsteroidType asteroidType;

    private int health;

    //------------------------------------------------------------------------------
    
    public Vector2 SpawnPoint { get => spawnPosition; }
    public Vector2 Position { get => currentPosition; }

    public GameObject AttachedObject { get => asteroidObject; }
    public Transform AttachedTransform { get => asteroidTransform; }
    public Renderer AttachedRenderer { get => renderer; }
    public Chunk2 ParentChunk { get => parentChunk; }

    public AsteroidSize Size { get => asteroidSize; }
    public AsteroidType Type { get => asteroidType; }

    public int Health { get => health; }
    public bool RendererStatus { get => renderer.enabled; }

    public Asteroid2(Chunk2 _parentChunk, AsteroidSize _size, AsteroidType _type, Vector2 _position, int _health)
    {
      parentChunk = _parentChunk;
      asteroidSize = _size;
      asteroidType = _type;

      spawnPosition = _position;
      health = _health;
    }

    public void SetAsteroidObject(GameObject _asteroidObject, Vector2 _position)
    {
      asteroidObject = _asteroidObject;
      asteroidTransform = asteroidObject.transform;
      renderer = asteroidObject.GetComponent<SpriteRenderer>();

      asteroidObject.transform.position = _position;
    }

    public void IsRendered(bool _enabled)
    {
      renderer.enabled = _enabled;
    }

    public void DisposeObject()
    {
      currentPosition = asteroidTransform.position;

      Vector2Int newChunkKey = ChunkManager2.Instance.QuantisePosition(currentPosition);

      if (asteroidObject != null)
      {
        GameObject.Destroy(asteroidObject);
        asteroidObject = null;
        asteroidTransform = null;
        renderer = null;
      }

      if (parentChunk.Key == newChunkKey) return;
      parentChunk.RemoveAsteroid(spawnPosition);
      spawnPosition = currentPosition;

      parentChunk = ChunkManager2.Instance.GetChunk(newChunkKey);
      parentChunk.AddAsteroid(this, spawnPosition);
    }

    public int UpdateHealth(int damage)
    {
      health -= damage;
      return health;
    }

    public void UpdatePosition(Vector2 newPosition) => currentPosition = newPosition;
  }
}
