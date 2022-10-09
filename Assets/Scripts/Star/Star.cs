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
    private float starTemperature;
    private float beltMin = 30;
    private float beltMax = 60;

    private GameObject starObject;
    private Renderer starRenderer;
    private Chunk parentChunk;

    private List<GameObject> orbitingBodies = new List<GameObject>();

    //------------------------------------------------------------------------------

    public Vector2 SpawnPoint { get => starPosition; }
    public Vector2 AsteroidBeltRadius { get => new Vector2(beltMin, beltMax); }
    public StarType Type { get => starType; }
    public float MaxOrbitRadius { get => starMaxRadius; }
    public float Temperature { get => starTemperature; }

    public Chunk ParentChunk { get => parentChunk; }
    public GameObject AttachedObject { get => starObject; }
    public List<GameObject> OrbitiSpawngBodies { get => orbitingBodies; }

    //------------------------------------------------------------------------------

    public Star(Chunk _parentChunk, StarType _type)
    {
      parentChunk = _parentChunk;

      starPosition = new Vector2(
            UnityEngine.Random.Range(_parentChunk.ChunkBounds.min.x, _parentChunk.ChunkBounds.max.x),
            UnityEngine.Random.Range(_parentChunk.ChunkBounds.min.y, _parentChunk.ChunkBounds.max.y)
          );

      starType = _type;
      SetStarProperties();

      starObject = null;
    }

    public void SetStarObject(GameObject _starObject, SpriteRenderer _starRenderer)
    {
      starObject = _starObject;
      starRenderer = _starRenderer;

      _starObject.transform.position = starPosition;
    }

    public void DisposeObject()
    {
      if (starObject == null) return;
      Debug.Log("Disposing star object");

      starObject = null;
      starRenderer = null;
    }

    private void SetStarProperties()
    {
      switch (starType)
      {
        case StarType.WhiteDwarf:
          starMaxRadius = 60;
          starTemperature = 50000;
          break;
        case StarType.BrownDwarf:
          starMaxRadius = 80;
          starTemperature = 80000;
          break;
        case StarType.RedDwarf:
          starMaxRadius = 100;
          starTemperature = 100000;
          break;
        case StarType.YellowDwarf:
          starMaxRadius = 140;
          starTemperature = 400000;
          break;
        case StarType.BlueGiant:
          starMaxRadius = 250;
          starTemperature = 800000;
          break;
        case StarType.OrangeGiant:
          starMaxRadius = 300;
          starTemperature = 1000000;
          break;
        case StarType.RedGiant:
          starMaxRadius = 350;
          starTemperature = 5000000;
          break;
        case StarType.BlueSuperGiant:
          starMaxRadius = 400;
          starTemperature = 15000000;
          break;
        case StarType.RedSuperGiant:
          starMaxRadius = 450;
          starTemperature = 25000000;
          break;
        case StarType.BlueHyperGiant:
          starMaxRadius = 500;
          starTemperature = 40000000;
          break;
        case StarType.RedHyperGiant:
          starMaxRadius = 600;
          starTemperature = 48000000;
          break;
        case StarType.NeutronStar:
          starMaxRadius = 300;
          starTemperature = 50000000;
          break;
        case StarType.BlackHole:
          starMaxRadius = 300;
          starTemperature = 15000000;
          break;
      }
    }
  }
}