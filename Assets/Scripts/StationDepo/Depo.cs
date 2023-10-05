using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public enum DepoType
  {
    Standard,
    Rare,
    Exotic
  }

  public class Depo
  {
    private Chunk parentChunk;
    private DepoType depoType;
    private GameObject depoObject;

    public GameObject AttachedObject { get => depoObject; }
    public Vector2 SpawnPoint { get; private set; }

    public Depo(Chunk _parentChunk, DepoType _type)
    {
      parentChunk = _parentChunk;
      depoType = _type;

      SpawnPoint = 
      new Vector2(
            UnityEngine.Random.Range(_parentChunk.ChunkBounds.min.x + 20, _parentChunk.ChunkBounds.max.x + 20),
            UnityEngine.Random.Range(_parentChunk.ChunkBounds.min.y + 20, _parentChunk.ChunkBounds.max.y + 20)
          );
    }

    public void SetDepoObject(GameObject _depoObject)
    {
      depoObject = _depoObject;
      depoObject.transform.position = SpawnPoint;
    }

    public void DisposeObject()
    {
      if (depoObject == null) return;

      UnityEngine.Object.Destroy(depoObject);
      depoObject = null;
    }
  }
}