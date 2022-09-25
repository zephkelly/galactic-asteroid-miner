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
    private float beltMin = 20;
    private float beltMax = 30;

    private GameObject starObject;
    private Renderer starRenderer;
    private Chunk parentChunk;

    private List<GameObject> orbitingBodies = new List<GameObject>();

    //------------------------------------------------------------------------------

    public Vector2 SpawnPoint { get => starPosition; }
    public Vector2 AsteroidBeltRadius { get => new Vector2(beltMin, beltMax); }
    public StarType Type { get => starType; }
    public float MaxOrbitRadius { get => starMaxRadius; }

    public Chunk ParentChunk { get => parentChunk; }
    public GameObject AttachedObject { get => starObject; }
    public List<GameObject> OrbitingBodies { get => orbitingBodies; }

    //------------------------------------------------------------------------------

    public Star(Chunk _parentChunk, StarType _type)
    {
      parentChunk = _parentChunk;
      starPosition = _parentChunk.Position;

      starType = _type;
      starObject = null;
    }

    public void SetStarObject(GameObject _starObject)
    {
      starObject = _starObject;
      starRenderer = starObject.GetComponent<SpriteRenderer>();

      _starObject.transform.position = starPosition;
    }

    public void DisposeObject()
    {
      Debug.Log("Disposing star object");
      if (starObject != null) return;

      starObject = null;
      starRenderer = null;
    }
  }
}