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

    private GameObject attachedObject;
    private Bounds chunkBounds;

    private List<Asteroid> asteroids = new List<Asteroid>();

    private Depo depo;
    private bool hasDepo;

    private Star star;
    private bool hasStar;

    //------------------------------------------------------------------------------

    public Vector2Int Key { get => chunkKey; }
    public Vector2 Position { get => chunkWorldPosition; }

    public Bounds ChunkBounds { get => chunkBounds; }
    public GameObject AttachedObject { get => attachedObject; }

    public List<Asteroid> Asteroids { 
      get => asteroids; 
      set => asteroids = value; 
    }

    public Star ChunkStar { get => star; }
    public bool HasStar { get => hasStar; }

    public Depo ChunkDepo { get => depo; }
    public bool HasDepo { get => hasDepo; }
    public bool HasBeenPopulated { get => hasBeenPopulated; }

    //------------------------------------------------------------------------------

    public Chunk(Vector2Int _chunkKey, int _chunkDiameter, GameObject _object)
    {
      chunkKey = _chunkKey;
      chunkWorldPosition = chunkKey * _chunkDiameter;

      attachedObject = _object;
      chunkBounds =  new Bounds(chunkWorldPosition, Vector2.one * _chunkDiameter);

      hasStar = false;
      hasBeenPopulated = false;
      star = null;
    }

    public void SetDepo(Depo _depo)
    {
      depo = _depo;
      hasDepo = true;
    }

    public void SetStar(Star _star)
    {
      hasStar = true;
      star = _star;
    }

    //Adding and removing asteroids from chunks is handled by occlusion manager
    public void PopulateAsteroid(Asteroid asteroid, Vector2 populatePosition)
    {
      asteroids.Add(asteroid);
    }

    public void SetPopulated() => hasBeenPopulated = true;
  }
}