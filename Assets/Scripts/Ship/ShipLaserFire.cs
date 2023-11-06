using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using zephkelly.AI.Scavenger;

namespace zephkelly
{
  public class ShipLaserFire : MonoBehaviour
  {
    [SerializeField] ParticleSystem laserParticleSystem;
    private ParticleSystemRenderer laserParticleRenderer;
    [SerializeField] GameObject explosionPrefab;

    [SerializeField] float explosionForce = 4f;
    List<ParticleCollisionEvent> collisonEvents = new List<ParticleCollisionEvent>();

    private Transform shipTransform;

    public void Awake()
    {
      laserParticleSystem = gameObject.GetComponent<ParticleSystem>();
      laserParticleRenderer = gameObject.GetComponent<ParticleSystemRenderer>();
    }

    public void Start()
    {
      laserParticleSystem.Stop();
    }

    public void Shoot()
    {
      laserParticleSystem.Play();
    }

    public void SetProjectileMat(Material _material)
    {
      laserParticleRenderer.material = _material;
      laserParticleRenderer.trailMaterial = _material;
    }

    public void SetProjectileSpeed(float _speed)
    {
      var main = laserParticleSystem.main;
      main.startSpeed = _speed;
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
        var asteroidInfo = hitObject.GetComponent<AsteroidController>().AsteroidInfo;

        OcclusionManager.Instance.RemoveAsteroid.Add(asteroidInfo, asteroidInfo.ParentChunk);
      }
      else if (hitObject.CompareTag("Enemy"))
      {
        hitObject.GetComponentInParent<ScavengerController>().TakeDamage(10);
      }
      else if (hitObject.CompareTag("Player"))
      {
        Debug.Log("Scavenger hit player!");
        hitObject.GetComponent<ShipController>().ShipConfig.TakeDamage(4);
      }

      //Fade the explosion light over time and destroy when done
      StartCoroutine(Explosion(0.5f, hitPoint));
    }

    IEnumerator Explosion(float duration, Vector2 explosionPoint)
    {
      zephkelly.AudioManager.Instance.PlaySoundRandomPitch("ShipShootImpact", 0.7f, 1.3f);

      GameObject explosion = Instantiate(explosionPrefab, explosionPoint, Quaternion.identity);
      var light = explosion.GetComponent<Light2D>();

      float startIntensity = light.intensity;
      float _lerp = 0;

      while (_lerp < 1)
      {
        _lerp += Time.deltaTime / duration;
        light.intensity = Mathf.Lerp(startIntensity, 0, _lerp);
        yield return new WaitForSeconds(0.1f);
      }

      Destroy(light.gameObject);
    }
  }
}