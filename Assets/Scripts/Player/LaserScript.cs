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
      var explosionLight = explosion.GetComponent<Light2D>();

      StartCoroutine(FadeLight(explosionLight, 0, 0.5f));
      
      Destroy(explosion, 0.5f);
    }

    IEnumerator FadeLight(Light2D light, float targetIntensity, float duration)
    {
      float startIntensity = light.intensity;
      float t = 0;
      while (t < 1)
      {
        t += Time.deltaTime / duration;
        light.intensity = Mathf.Lerp(startIntensity, targetIntensity, t);
        yield return null;
      }
    }
  }
}