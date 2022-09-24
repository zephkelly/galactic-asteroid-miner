using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class AsteroidController : MonoBehaviour
  {
    private AsteroidBehaviour asteroidBehaviour;
    private Asteroid2 asteroidInfo;

    [SerializeField] SpriteRenderer asteroidSpriteRenderer;   //In inspector
    private Rigidbody2D asteroidRigid2D;
    private Transform asteroidTransform;

    //------------------------------------------------------------------------------

    public Rigidbody2D AsteroidRigid2D { get => asteroidRigid2D; }
    public Asteroid2 AsteroidInfo { get => asteroidInfo; }
    public float Health { get => asteroidInfo.Health; }

    private void Awake()
    {
      asteroidBehaviour = Resources.Load("ScriptableObjects/AsteroidBehaviour")
        as AsteroidBehaviour;

      asteroidRigid2D = GetComponent<Rigidbody2D>();
      asteroidTransform = GetComponent<Transform>();
    }

    public void SetAsteroidInfo(Asteroid2 _asteroidInfo)
    {
      asteroidInfo = _asteroidInfo;
    }

    public void TakeDamage(int damage, Vector2 hitVector)
    {
      asteroidInfo.SetHealth(asteroidBehaviour.TakeDamage(asteroidInfo, damage, hitVector));

      if(asteroidInfo.Health <= 0)
      {
        Destroy(gameObject);
        //Needa remove asteroid from occlusion manager
      }
    }
  }
}