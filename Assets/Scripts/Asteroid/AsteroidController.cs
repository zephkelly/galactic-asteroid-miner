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

    //------------------------------------------------------------------------------

    public AsteroidType AsteroidType { get => asteroidInfo.Type; }
    public Rigidbody2D AsteroidRigid2D { get => asteroidInfo.Rigid2D; }
    public Collider2D AsteroidCollider { get => asteroidInfo.Collider; }
    public SpriteRenderer AsteroidSpriteRenderer { get => asteroidInfo.Renderer; }

    private void Awake()
    {
      asteroidBehaviour = Resources.Load("ScriptableObjects/AsteroidBehaviour")
        as AsteroidBehaviour;
    }

    public void SetAsteroid(Asteroid _asteroidInfo, Chunk asteroidChunk)
    {
      asteroidInfo = _asteroidInfo;
      asteroidInfo.ParentChunk = asteroidChunk;

      asteroidInfo.AsteroidObject = gameObject;
      asteroidInfo.AsteroidTransform = transform;
      asteroidInfo.Rigid2D = GetComponent<Rigidbody2D>();
      asteroidInfo.Collider = GetComponent<Collider2D>();
      asteroidInfo.Renderer = asteroidSpriteRenderer;

      asteroidInfo = asteroidBehaviour.SetHealth(asteroidInfo);
      asteroidInfo.HasBeenActive = true;
    }

    private void Update()
    {
      asteroidInfo.CurrentPosition = asteroidInfo.AsteroidTransform.position;
    }

    public void TakeDamage(int damage, Vector2 hitVector)
    {
      asteroidInfo.Health = asteroidBehaviour.TakeDamage(asteroidInfo, damage, hitVector);

      if(asteroidInfo.Health <= 0)
      {
        ChunkManager.Instance.OcclusionManager.RemoveAsteroid(asteroidInfo);
        Destroy(gameObject);
      }
    }
  }
}