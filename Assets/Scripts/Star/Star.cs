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

    private GameObject currentStarObject;
    private Chunk2 parentChunk;

    private List<GameObject> orbitingBodies = new List<GameObject>();

    //------------------------------------------------------------------------------

    public Vector2 Position { get => starPosition; }
    public Vector2 AsteroidBeltRadius { get => new Vector2(beltMin, beltMax); }
    public StarType Type { get => starType; }
    public float MaxOrbitRadius { get => starMaxRadius; }

    public Chunk2 ParentChunk { get => parentChunk; }
    public GameObject StarObject { get => currentStarObject; }
    public List<GameObject> OrbitingBodies { get => orbitingBodies; }

    //------------------------------------------------------------------------------

    public Star(Chunk2 _parentChunk, StarType _type)
    {
      parentChunk = _parentChunk;
      starPosition = _parentChunk.Position;

      starType = _type;

      //Star Orbit Radius based on Star Types
      //Get asteroid orbit radius

      currentStarObject = null;
    }
  }
}