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

    private Transform asteroidTransform;
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

      asteroidTransform = transform;
    }

    public void Init(Asteroid _asteroidInfo)
    {
      asteroidInfo = _asteroidInfo;

      asteroidInfo.Rigid2D = GetComponent<Rigidbody2D>();
      asteroidInfo.Collider = GetComponent<Collider2D>();
      asteroidInfo.Renderer = asteroidSpriteRenderer;

      asteroidInfo = asteroidBehaviour.SetHealth(asteroidInfo);;
    }

    private void Update()
    {
      asteroidInfo.Position = asteroidTransform.position;
    }

    public void TakeDamage(int damage, Vector2 hitVector)
    {
      if (GetSize() == AsteroidSize.Pickup) return;

      asteroidInfo.Health = asteroidBehaviour.TakeDamage(asteroidInfo, damage, hitVector);

      if(asteroidInfo.Health <= 0) Destroy(this.gameObject);
    }

    public AsteroidType GetAsteroidType()
    {
      return asteroidInfo.Type;
    }

    public AsteroidSize GetSize()
    {
      return asteroidInfo.Size;
    }

    public int GetHealth()
    {
      return asteroidInfo.Health;
    }
  }
}