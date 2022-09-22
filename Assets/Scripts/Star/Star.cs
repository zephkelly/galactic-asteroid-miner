using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public enum StarType
  {
    WhiteDwarf,
    BrownDwarf,
    RedDwarf,
    YellowDwarf,
    BlueGiant,
    OrangeGiant,
    RedGiant,
    BlueSuperGiant,
    RedSuperGiant,
    BlueHyperGiant,
    RedHyperGiant,
    NeutronStar,
    BlackHole
  }

  public class Star
  {
    private Vector2 starPosition;
    private StarType starType;

    private float starMaxRadius;
    private float beltMin;
    private float beltMax;

    private GameObject starObject;
    private Renderer starRenderer;
    private Chunk2 parentChunk;

    private List<GameObject> orbitingBodies = new List<GameObject>();

    //------------------------------------------------------------------------------

    public Vector2 Position { get => starPosition; }
    public Vector2 AsteroidBeltRadius { get => new Vector2(beltMin, beltMax); }
    public StarType Type { get => starType; }
    public float MaxOrbitRadius { get => starMaxRadius; }

    public Chunk2 ParentChunk { get => parentChunk; }
    public GameObject AttachedObject { get => starObject; }
    public List<GameObject> OrbitingBodies { get => orbitingBodies; }

    //------------------------------------------------------------------------------

    public Star(Chunk2 _parentChunk, StarType _type)
    {
      parentChunk = _parentChunk;
      starPosition = _parentChunk.Position;

      starType = _type;
      starObject = null;
    }

    public void SetStarObject(GameObject _starObject)
    {
      starObject = _starObject;
      starRenderer = starObject.GetComponent<Renderer>();

      _starObject.transform.position = starPosition;
    }

    public void DisposeObject()
    {
      if (starObject != null) return;

      GameObject.Destroy(starObject);
      starObject = null;
      starRenderer = null;
    }
  }
}