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
    Titanium,
    Gold,
    Palladium,
    Cobalt,
    Stellarite,
    Darkore
  }

  public class Asteroid
  {
    private Vector2 spawnPosition;
    private Vector2 currentPosition;

    private GameObject asteroidObject;
    private Transform asteroidTransform;
    private SpriteRenderer renderer;
    private Rigidbody2D rigid2D;
    private Collider2D collider2D;

    private Chunk parentChunk;
    private AsteroidSize asteroidSize;
    private AsteroidType asteroidType;

    private int health;

    //------------------------------------------------------------------------------
    
    public Vector2 SpawnPoint { get => spawnPosition; }
    public Vector2 CurrentPosition { get => currentPosition; }

    public GameObject AttachedObject { get => asteroidObject; }
    public Transform AttachedTransform { get => asteroidTransform; }
    public Renderer AttachedRenderer { get => renderer; }
    public Rigidbody2D AttachedRigid { get => rigid2D; }
    public Collider2D AttachedCollider { get => collider2D; }

    public Chunk ParentChunk { get => parentChunk; }
    public Star ParentStar { get => parentChunk.ChunkStar; }
    public AsteroidSize Size { get => asteroidSize; }
    public AsteroidType Type { get => asteroidType; }

    public int Health { get => health; }
    public bool RendererStatus { get => renderer.enabled; }

    public Asteroid(Chunk _parentChunk, AsteroidSize _size, AsteroidType _type, Vector2 _position, int _health)
    {
      parentChunk = _parentChunk;
      asteroidSize = _size;
      asteroidType = _type;

      spawnPosition = _position;
      currentPosition = _position;
      health = _health;
    }

    public void SetObject(GameObject _asteroidObject)
    {
      asteroidObject = _asteroidObject;
      if (asteroidObject == null) return;
      
      asteroidTransform = asteroidObject.transform;

      rigid2D = asteroidObject.GetComponent<Rigidbody2D>();
      collider2D = asteroidObject.GetComponent<Collider2D>();
      renderer = asteroidObject.GetComponentInChildren<SpriteRenderer>();
      
      asteroidObject.GetComponent<AsteroidController>().SetAsteroidInfo(this);
      asteroidObject.transform.position = spawnPosition;
    }

    public void UpdateCurrentPosition()
    {
      currentPosition = asteroidTransform.position;
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

    public void NewParentChunk(Chunk _newParentChunk)
    {
      parentChunk = _newParentChunk;
    }
  }
}