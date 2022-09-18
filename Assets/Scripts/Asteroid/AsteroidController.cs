using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class AsteroidController : MonoBehaviour
  {
    private AsteroidBehaviour asteroidBehaviour;
    private Asteroid asteroidInfo;

    [SerializeField] SpriteRenderer asteroidSpriteRenderer;   //In inspector
    private Rigidbody2D asteroidRigid2D;

    //------------------------------------------------------------------------------

    public Rigidbody2D AsteroidRigid2D { get => asteroidRigid2D; }
    public Asteroid AsteroidInfo { get => asteroidInfo; }

    private void Awake()
    {
      asteroidBehaviour = Resources.Load("ScriptableObjects/AsteroidBehaviour")
        as AsteroidBehaviour;

      asteroidRigid2D = GetComponent<Rigidbody2D>();
    }

    public void SetAsteroid(Asteroid _asteroidInfo, Chunk asteroidChunk)
    {
      asteroidInfo = _asteroidInfo;
      asteroidInfo.ParentChunk = asteroidChunk;

      asteroidInfo.AsteroidObject = gameObject;
      asteroidInfo.AsteroidTransform = transform;
      asteroidInfo.Rigid2D = asteroidRigid2D;
      asteroidInfo.Collider = GetComponent<Collider2D>();
      asteroidInfo.Renderer = asteroidSpriteRenderer;

      asteroidInfo = asteroidBehaviour.SetHealth(asteroidInfo);
    }

    private void Update()
    {
      if (asteroidInfo.AsteroidTransform == null)
      {
        Debug.LogError("AsteroidController: Update: AsteroidTransform is null");
        return;
      }

      asteroidInfo.UpdatePosition();
    }

    public void TakeDamage(int damage, Vector2 hitVector)
    {
      asteroidInfo.Health = asteroidBehaviour.TakeDamage(asteroidInfo, damage, hitVector);

      if(asteroidInfo.Health <= 0)
      {
        ChunkManager.Instance.OcclusionManager.RemoveAsteroid(asteroidInfo);
      }
    }
  }
}