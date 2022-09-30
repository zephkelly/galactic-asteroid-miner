using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace zephkelly
{
  public class StarController : MonoBehaviour
  {
    private Star starInfo;
    private StarOrbitingBehaviour starOrbitingBehaviour;

    [SerializeField] Light2D starLight;
    [SerializeField] CircleCollider2D starRadiusCollider;
    [SerializeField] SpriteRenderer starSpriteRenderer;

    private Transform starTransform;
    private Vector2 starPosition;

    //------------------------------------------------------------------------------

    public Star StarInfo { get => starInfo; }
    public Chunk ParentChunk { get => starInfo.ParentChunk; }
    public StarOrbitingBehaviour OrbitingBehaviour { get => starOrbitingBehaviour; }
    public Vector2 StarPosition { get => starPosition; }
    public float StarRadius { get => starInfo.MaxOrbitRadius; }

    public void SetStarInfo(Star _starInfo)
    {
      starInfo = _starInfo;

      starPosition = starInfo.SpawnPoint;
      starPosition = _starInfo.SpawnPoint;

      starRadiusCollider.radius = starInfo.MaxOrbitRadius;
      starLight.pointLightOuterRadius = starInfo.MaxOrbitRadius;

      starInfo.SetStarObject(this.gameObject, starSpriteRenderer);
    }

    private void Start()
    {
      starOrbitingBehaviour = gameObject.GetComponent<StarOrbitingBehaviour>();
    }
  }
}