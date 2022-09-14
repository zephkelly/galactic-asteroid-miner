using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace zephkelly
{
  public class LaserParticleFire : MonoBehaviour
  {
    [SerializeField] ParticleSystem laserParticleSystem;
    [SerializeField] GameObject explosionPrefab;

    [SerializeField] float explosionForce = 4f;
    List<ParticleCollisionEvent> collisonEvents = new List<ParticleCollisionEvent>();


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

      //Make a prefab of the explosion and grab a reference to the light attached to its gameobject
      GameObject explosion = Instantiate(explosionPrefab, hitPoint, Quaternion.identity);
      var explosionLight = explosion.GetComponent<Light2D>();

      //Add force to the object we hit
      var directionOfForce = (hitPoint - (Vector2)transform.position).normalized;
      hitObject.GetComponent<Rigidbody2D>().AddForceAtPosition(directionOfForce * explosionForce, hitPoint, ForceMode2D.Impulse);

      if (hitObject.CompareTag("Asteroid"))
      {
        hitObject.GetComponent<AsteroidController>().TakeDamage(1, hitPoint);
      }
      else if (hitObject.CompareTag("AsteroidPickup"))
      {
        Destroy(hitObject);
      }

      //Fade the explosion light over time and destroy when done
      StartCoroutine(FadeExplosionLight(explosionLight, 0, 0.5f));
      Destroy(explosion, 0.5f);
    }

    IEnumerator FadeExplosionLight(Light2D light, float targetIntensity, float duration)
    {
      float startIntensity = light.intensity;
      float _lerp = 0;
      while (_lerp < 1)
      {
        _lerp += Time.deltaTime / duration;
        light.intensity = Mathf.Lerp(startIntensity, targetIntensity, _lerp);
        yield return null;
      }
    }
  }
}