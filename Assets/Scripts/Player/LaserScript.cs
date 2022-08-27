using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace zephkelly
{
  public class LaserScript : MonoBehaviour
  {
    [SerializeField] ParticleSystem laserParticleSystem;
    [SerializeField] GameObject explosionPrefab;

    private List<ParticleCollisionEvent> collisonEvents;

    public void Awake()
    {
      laserParticleSystem = gameObject.GetComponent<ParticleSystem>();
      collisonEvents = new List<ParticleCollisionEvent>();
    }

    public void Start()
    {
      laserParticleSystem.Stop();
    }

    public void Shoot()
    {
      laserParticleSystem.Play();
    }

    private void OnParticleCollision(GameObject other)
    {
      laserParticleSystem.GetCollisionEvents(other, collisonEvents);

      GameObject explosion = Instantiate(explosionPrefab, collisonEvents[0].intersection, Quaternion.identity);

      laserParticleSystem.Clear();
      Destroy(explosion, 0.5f);
    }
  }
}