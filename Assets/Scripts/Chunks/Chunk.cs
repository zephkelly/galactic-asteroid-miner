using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class Chunk
  {
    public Vector2 Position { get; set; }
    public GameObject ChunkObject { get; set; }
    public Transform ChunkTransform { get; set; }

    private Dictionary<Vector2, Asteroid> asteroids = 
      new Dictionary<Vector2, Asteroid>();

    public Dictionary<Vector2, Asteroid> Asteroids { get => asteroids; set => asteroids = value; }

    public void SetChunkObject(Vector2Int _position, GameObject _chunkObject)
    {
      Position = _position;
      ChunkObject = _chunkObject;
      ChunkTransform = ChunkObject.transform;
    }

    public void AddAsteroid(Asteroid asteroid)
    {
      asteroids.Add(asteroid.SpawnPosition, asteroid);
    }

    public void AddForiegnAsteroid(Asteroid asteroid)
    {
      asteroid.ParentChunk = this;
      asteroid.SetNewSpawn(asteroid.CurrentPosition);
      asteroids.Add(asteroid.SpawnPosition, asteroid);
    }

    public void DestroyAsteroidFromLazy(Asteroid asteroid)
    {
      asteroids.Remove(asteroid.LazyKey);
    }

    public void DestroyAsteroidFromSpawn(Asteroid asteroid)
    {
      asteroids.Remove(asteroid.SpawnPosition);
    }
  }
}