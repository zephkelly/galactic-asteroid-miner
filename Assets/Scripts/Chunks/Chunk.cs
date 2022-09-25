using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class Chunk
  {
    private Vector2Int chunkKey;
    private Vector2 chunkWorldPosition;
    private bool hasBeenPopulated;

    private GameObject attackedObject;
    private Bounds chunkBounds;

    private Dictionary<Vector2, Asteroid> asteroids =
      new Dictionary<Vector2, Asteroid>();

    private Star star;
    private bool hasStar;

    //------------------------------------------------------------------------------

    public Vector2Int Key { get => chunkKey; }
    public Vector2 Position { get => chunkWorldPosition; }

    public Bounds ChunkBounds { get => chunkBounds; }
    public GameObject AttachedObject { get => attackedObject; }

    public Dictionary<Vector2, Asteroid> Asteroids { 
      get => asteroids; 
      set => asteroids = value; 
    }

    public Star ChunkStar { get => star; }
    public bool HasStar { get => hasStar; }
    public bool HasBeenPopulated { get => hasBeenPopulated; }

    //------------------------------------------------------------------------------

    public Chunk(Vector2Int _chunkKey, int _chunkDiameter, GameObject _object)
    {
      chunkKey = _chunkKey;
      chunkWorldPosition = chunkKey * _chunkDiameter;

      attackedObject = _object;
      chunkBounds =  new Bounds(chunkWorldPosition, Vector2.one * _chunkDiameter);

      hasStar = false;
      hasBeenPopulated = false;
      star = null;
    }

    public void SetStar(Star _star)
    {
      hasStar = true;
      star = _star;
    }

    public void PopulateAsteroid(Asteroid asteroid, Vector2 populatePosition)
    {
      asteroids.Add(populatePosition, asteroid);
    }

    public void AddAsteroid(Asteroid asteroid, Vector2 spawnPosition)
    {
      asteroids.Add(spawnPosition, asteroid);
    }

    public void RemoveAsteroid(Vector2 position)
    {
      asteroids.Remove(position);
    }

    public void SetPopulated() => hasBeenPopulated = true;
  }
}