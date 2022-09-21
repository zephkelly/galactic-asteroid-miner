using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class Asteroid
  {
    private Vector2 spawnPosition;
    private Vector2 currentPosition;
    private Vector2 lazyKey;

    //------------------------------------------------------------------------------

    public AsteroidType Type { get; set; }
    public AsteroidSize Size { get; set; }
    public Chunk ParentChunk { get; set; }

    public GameObject AsteroidObject { get; set; }
    public Transform AsteroidTransform { get; set; }
    public Collider2D Collider { get; set; }
    public Rigidbody2D Rigid2D { get; set; }
    public SpriteRenderer Renderer { get; set; }

    public Vector2 SpawnPosition { get => spawnPosition; }
    public Vector2 CurrentPosition { get => currentPosition; }
    public Vector2 LazyKey { get => lazyKey; }

    public bool IsLazy { get; set; }
    public int Health { get; set; }

    //------------------------------------------------------------------------------

    public void SetSpawnPosition(Vector2 _spawnPosition)
    {
      spawnPosition = _spawnPosition;
      currentPosition = _spawnPosition;
    }

    public Vector2 SetLazyKey()
    {
      lazyKey = AsteroidTransform.position;
      return lazyKey;
    }

    public void UpdatePosition()
    {
      if (AsteroidTransform == null)
      {
        Debug.LogError("AsteroidTransform is null");
        return;
      }

      currentPosition = AsteroidTransform.position;
    }

    public void SetNewSpawn(Vector2 _newSpawn)
    {
      spawnPosition = _newSpawn;
    }

    public void RemoveParentChunk()
    {
      ParentChunk.DestroyAsteroidFromLazy(this);
      ParentChunk = null;
    }
  }
}