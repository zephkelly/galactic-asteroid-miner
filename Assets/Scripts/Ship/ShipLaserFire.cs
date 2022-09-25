using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace zephkelly
{
  public class ShipLaserFire : MonoBehaviour
  {
    [SerializeField] ParticleSystem laserParticleSystem;
    [SerializeField] GameObject explosionPrefab;

    [SerializeField] float explosionForce = 4f;
    List<ParticleCollisionEvent> collisonEvents = new List<ParticleCollisionEvent>();

    private Transform shipTransform;

    public void Awake()
    {
      laserParticleSystem = gameObject.GetComponent<ParticleSystem>();
    }

    public void Start()
    {
      laserParticleSystem.Stop();
    }

    public void Shoot()
    {
      laserParticleSystem.Play();
    }

    private void OnParticleCollision(GameObject hitObject)
    {
      //Grab where our particles are colliding
      laserParticleSystem.GetCollisionEvents(hitObject, collisonEvents);
      Vector2 hitPoint = collisonEvents[0].intersection;

      //Add force to the object we hit
      var directionOfForce = (hitPoint - (Vector2)transform.position).normalized;
      var asteroidRigid2D = hitObject.GetComponent<Rigidbody2D>();

      asteroidRigid2D.AddForceAtPosition(
        directionOfForce * explosionForce,
        hitPoint,
        ForceMode2D.Impulse
      );

      if (hitObject.CompareTag("Asteroid"))
      {
        hitObject.GetComponent<AsteroidController>().TakeDamage(1, hitPoint);
      }
      else if (hitObject.CompareTag("AsteroidPickup"))
      {
        Destroy(hitObject);
      }

      //Fade the explosion light over time and destroy when done
      StartCoroutine(Explosion(0.5f, hitPoint));
    }

    IEnumerator Explosion(float duration, Vector2 explosionPoint)
    {
      GameObject explosion = Instantiate(explosionPrefab, explosionPoint, Quaternion.identity);
      var light = explosion.GetComponent<Light2D>();

      float startIntensity = light.intensity;
      float _lerp = 0;

      while (_lerp < 1)
      {
        _lerp += Time.deltaTime / duration;
        light.intensity = Mathf.Lerp(startIntensity, 0, _lerp);
        yield return null;
      }

      Destroy(light.gameObject);
    }
  }
}