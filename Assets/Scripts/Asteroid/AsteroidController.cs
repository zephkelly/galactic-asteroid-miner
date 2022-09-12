using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class AsteroidController : MonoBehaviour
  {
    private AsteroidBehaviour asteroidBehaviour;

    private Tuple<AsteroidType, AsteroidSize, int> asteroidProperties;
    private AsteroidType asteroidType;
    private AsteroidSize asteroidSize;

    [SerializeField] SpriteRenderer asteroidSpriteRenderer;
    [SerializeField] Rigidbody2D asteroidRigid2D;

    [SerializeField] int asteroidHealth;

    public AsteroidType AsteroidType { get => asteroidType; set => asteroidType = value; }
    public SpriteRenderer AsteroidSpriteRenderer { get => asteroidSpriteRenderer; set => asteroidSpriteRenderer = value; }
    public Rigidbody2D AsteroidRigid2D { get => asteroidRigid2D; set => asteroidRigid2D = value; }

    private void Awake()
    {
      asteroidBehaviour = Resources.Load("ScriptableObjects/AsteroidBehaviour") as AsteroidBehaviour;
      asteroidRigid2D = GetComponent<Rigidbody2D>();
    }

    public void Init(AsteroidType type, AsteroidSize size)
    {
      asteroidProperties = asteroidBehaviour.SetProperties(type, size, this.gameObject, asteroidSpriteRenderer, asteroidRigid2D);
      asteroidType = asteroidProperties.Item1;
      asteroidSize = asteroidProperties.Item2;
      asteroidHealth = asteroidProperties.Item3;
    }

    public void TakeDamage(int damage, Vector2 hitVector)
    {
      //if (GetSize() == AsteroidSize.Pickup) return;

      asteroidHealth = asteroidBehaviour.TakeDamage(this.gameObject, asteroidType, asteroidSize, asteroidHealth, damage, hitVector);

      if(asteroidHealth <= 0) Destroy(this.gameObject);
    }

    public AsteroidType GetAsteroidType()
    {
      return asteroidProperties.Item1;
    }

    public AsteroidSize GetSize()
    {
      return asteroidProperties.Item2;
    }

    public int GetHealth()
    {
      return asteroidProperties.Item3;
    }
  }
}