using UnityEngine;
using System.Collections;

namespace zephkelly
{
  public class CameraStarfield : MonoBehaviour {

    private Transform cameraTransform;
    private ParticleSystem startfieldParticleSystem;
    private ParticleSystem.Particle[] stars;

    //----------------------------------------------------------------------------------------------

    [SerializeField] int starsMax = 100;
    [SerializeField] float starSizeMin = 0.15f;
    [SerializeField] float starSizeMax = 0.4f;
    [SerializeField] float starSpawnRadius = 10;

      private float starDistanceSqr;
      private float starClipDistanceSqr;

    //----------------------------------------------------------------------------------------------
 
    private void Awake()
    {
      cameraTransform = transform;
      startfieldParticleSystem = GetComponent<ParticleSystem>();
    }

    private void Start () 
    {
      starDistanceSqr = starSpawnRadius * starSpawnRadius;
    }
 
    private void CreateStars()
    {
      stars = new ParticleSystem.Particle[starsMax];

      for (int i = 0; i < starsMax; i++)
      {
        stars[i].position = ((Vector3)Random.insideUnitCircle * starSpawnRadius) + cameraTransform.position;
        stars[i].position = new Vector3(stars[i].position.x, stars[i].position.y, 2);
        stars[i].startColor = new Color(1,1,1, 1);
        stars[i].startSize = Random.Range(starSizeMin, starSizeMax);
      }
    }
 
    private void Update () 
    {
      if ( stars == null ) CreateStars();

      for (int i = 0; i < starsMax; i++)
      {
        Vector3 cameraOffset = new Vector3(cameraTransform.position.x, cameraTransform.position.y, 2);

        if ((stars[i].position - cameraOffset).sqrMagnitude > starDistanceSqr)
        {
          stars[i].position = ((Vector3)Random.insideUnitCircle.normalized * starSpawnRadius) + cameraTransform.position;
          stars[i].position = new Vector3(stars[i].position.x, stars[i].position.y, 2);
        }
      }

      startfieldParticleSystem.SetParticles( stars, stars.Length );
    }
  }
}
