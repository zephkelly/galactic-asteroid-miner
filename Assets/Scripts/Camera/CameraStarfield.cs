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
    [SerializeField] float parallaxFactor = 0.9f;

      private Vector3 cameraStartPosition;
      private float starDistanceSqr;
      private float starClipDistanceSqr;

      private static int particleZ = 2;
    
    //----------------------------------------------------------------------------------------------

    private void Awake()
    {
      cameraTransform = transform;
      startfieldParticleSystem = GetComponent<ParticleSystem>();
    }

    private void Start () 
    {
      starDistanceSqr = starSpawnRadius * starSpawnRadius;
      cameraStartPosition = cameraTransform.position;

      CreateStars();
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
 
    private void Update() 
    {
      Vector3 cameraOffset = new Vector3(cameraTransform.position.x, cameraTransform.position.y, particleZ);

      for (int i = 0; i < starsMax; i++)
      {
        Parallax(i);

        if ((stars[i].position - cameraOffset).sqrMagnitude > starDistanceSqr)
        {
          //Normalize the random vector to place star on edge of spawn radius
          stars[i].position = ((Vector3)Random.insideUnitCircle.normalized * starSpawnRadius) + cameraTransform.position;
          stars[i].position = new Vector3(stars[i].position.x, stars[i].position.y, particleZ);
        }
      }
      
      cameraStartPosition = cameraTransform.position;
      startfieldParticleSystem.SetParticles(stars, stars.Length);
    }

    private void Parallax(int i)
    {
      float parallaxStep = parallaxFactor * Time.deltaTime;

      //Parallaxing the stars
      Vector3 cameraMovement = cameraTransform.position - cameraStartPosition;
      cameraMovement.z = 0;

      //stars[i].position = Vector3.MoveTowards(stars[i].position, stars[i].position + cameraMovement, parallaxStep);
      stars[i].position = Vector3.Lerp(stars[i].position, stars[i].position + cameraMovement, parallaxStep);

    }
  }
}
