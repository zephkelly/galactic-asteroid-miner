using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class Chunk2
  {
    private Vector2Int chunkKey;
    private Vector2 chunkWorldPosition;
    private bool hasBeenPopulated;

    private GameObject attackedObject;
    private Bounds chunkBounds;

    private Dictionary<Vector2, Asteroid2> asteroids =
      new Dictionary<Vector2, Asteroid2>();

    private Star star;
    private bool containsStar;

    //------------------------------------------------------------------------------

    public Vector2Int Key { get => chunkKey; }
    public Vector2 Position { get => chunkWorldPosition; }

    public Bounds ChunkBounds { get => chunkBounds; }
    public GameObject AttachedObject { get => attackedObject; }

    public Dictionary<Vector2, Asteroid2> Asteroids { 
      get => asteroids; 
      set => asteroids = value; 
    }

    public Star ChunkStar { get => star; }
    public bool HasStar { get => containsStar; }
    public bool HasBeenPopulated { get => hasBeenPopulated; }

    //------------------------------------------------------------------------------

    public Chunk2(Vector2Int _chunkKey, int _chunkDiameter, GameObject _object)
    {
      chunkKey = _chunkKey;
      chunkWorldPosition = chunkKey * _chunkDiameter;

      attackedObject = _object;
      chunkBounds =  new Bounds(chunkWorldPosition, Vector2.one * _chunkDiameter);

      containsStar = false;
      hasBeenPopulated = false;
      star = null;
    }

    public void SetStar(Star _star)
    {
      containsStar = true;
      star = _star;
    }

    public void PopulateAsteroid(Asteroid2 asteroid, Vector2 populatePosition)
    {
      asteroids.Add(populatePosition, asteroid);
    }

    public void AddAsteroid(Asteroid2 asteroid, Vector2 spawnPosition)
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