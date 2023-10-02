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


    public Depo(Chunk _parentChunk, DepoType _type)
    {
      parentChunk = _parentChunk;
      depoType = _type;
    }

    public void SetDepoObject(GameObject _depoObject)
    {
      depoObject = _depoObject;
      depoObject.transform.position = parentChunk.Position;
    }

    public void DisposeObject()
    {
      if (depoObject == null) return;

      UnityEngine.Object.Destroy(depoObject);
      depoObject = null;
    }
  }
}