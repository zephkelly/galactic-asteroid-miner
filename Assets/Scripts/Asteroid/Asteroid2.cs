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
    private Rigidbody2D rigid2D;
    private Collider2D collider2D;

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
    public Rigidbody2D AttachedRigid { get => rigid2D; }
    public Collider2D AttachedCollider { get => collider2D; }

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

    public void SetObject(GameObject _asteroidObject, Vector2 _position)
    {
      asteroidObject = _asteroidObject;
      asteroidTransform = asteroidObject.transform;

      rigid2D = asteroidObject.GetComponent<Rigidbody2D>();
      collider2D = asteroidObject.GetComponent<Collider2D>();
      renderer = asteroidObject.GetComponentInChildren<SpriteRenderer>();
      
      asteroidObject.GetComponent<AsteroidController>().SetAsteroidInfo(this);
      asteroidObject.transform.position = _position;
    }

    public void DisposeObject()
    {
      parentChunk.RemoveAsteroid(spawnPosition);
      renderer = null;

      if (asteroidTransform == null) return;

      currentPosition = asteroidTransform.position;
      asteroidTransform = null;

      var newChunkKey = ChunkManager2.Instance.GetChunkPosition(currentPosition);
      parentChunk = ChunkManager2.Instance.GetChunk(newChunkKey);
      spawnPosition = currentPosition;

      parentChunk.AddAsteroid(this, spawnPosition);
    }

    public void IsRendered(bool _enabled)
    {
      renderer.enabled = _enabled;
    }

    public void SetHealth(int _health)
    {
      health = _health;
    }

    public void SetChildSize(AsteroidSize _size)
    {
      asteroidSize = _size;
    }

    public void SetChildType(AsteroidType _type)
    {
      asteroidType = _type;
    }

    public void UpdateSpawnPoint() 
    {
      currentPosition = asteroidTransform.position;
      spawnPosition = currentPosition;
    }

  /*
    public void SetParentChunk(Chunk2 _chunk)
    {
      parentChunk = _chunk;
    }
    */

    public void SetPosition(Vector2 newPosition) => currentPosition = newPosition;
  }
}
