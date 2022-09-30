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
    private Transform asteroidTransform;

    //------------------------------------------------------------------------------

    public Rigidbody2D AsteroidRigid2D { get => asteroidRigid2D; }
    public Asteroid AsteroidInfo { get => asteroidInfo; }
    public float Health { get => asteroidInfo.Health; }

    private void Awake()
    {
      asteroidBehaviour = Resources.Load("ScriptableObjects/AsteroidBehaviour")
        as AsteroidBehaviour;

      asteroidRigid2D = GetComponent<Rigidbody2D>();
      asteroidTransform = GetComponent<Transform>();
    }

    public void SetAsteroidInfo(Asteroid _asteroidInfo)
    {
      asteroidInfo = _asteroidInfo;
    }

    public void TakeDamage(int damage, Vector2 hitVector)
    {
      asteroidInfo.SetHealth(asteroidBehaviour.TakeDamage(asteroidInfo, damage, hitVector));

      if(asteroidInfo.Health <= 0)
      {
        OcclusionManager.Instance.RemoveAsteroid.Add(asteroidInfo, asteroidInfo.ParentChunk);
      }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      if (!other.CompareTag("Star")) return;
      var starChunk = other.GetComponent<StarController>().ParentChunk;

      if (starChunk == asteroidInfo.ParentChunk) return;
      if (OcclusionManager.Instance.ChangeAsteroidChunk.ContainsKey(asteroidInfo)) return;
      OcclusionManager.Instance.ChangeAsteroidChunk.Add(asteroidInfo, starChunk);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
      if (!other.CompareTag("Star")) return;
      var newChunkPosition = ChunkManager.Instance.GetChunkPosition(asteroidTransform.position);
      var newChunk = ChunkManager.Instance.GetChunk(newChunkPosition);

      if (newChunk == asteroidInfo.ParentChunk) return;
      if (OcclusionManager.Instance.ChangeAsteroidChunk.ContainsKey(asteroidInfo)) return;
      OcclusionManager.Instance.ChangeAsteroidChunk.Add(asteroidInfo, newChunk);
    }
  }
}