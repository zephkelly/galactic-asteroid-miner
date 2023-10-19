using UnityEngine;
using System.Collections;

namespace zephkelly
{
  public class ParallaxGasCloud : MonoBehaviour {

    private ParticleSystem cloudParticleSystem;
    private ParticleSystem.Particle[] cloudParticles;
    private Transform cameraTransform;

    [SerializeField] Transform parallaxLayer;
    [SerializeField] int cloudsMax = 100;
    private int currentClouds = 0;

    [SerializeField] float starSizeMin = 0.15f;
    [SerializeField] float starSizeMax = 0.4f;
    [SerializeField] float starSpawnRadius = 100;
    [SerializeField] float parallaxFactor = 0.9f;

    private Gradient gasGradient;
    [SerializeField] private float perlinScale = 20.0f; // Determines how stretched the noise is. You can adjust this value.
    [SerializeField] private Vector2 perlinOffset = new Vector2(200, 200);

    private Vector2 lastPosition = Vector2.zero;

    private float starDistanceSqr;
    private float starClipDistanceSqr;
    [SerializeField] private int particleZ = 2;
    
    //----------------------------------------------------------------------------------------------

    private void Awake()
    {
      cameraTransform = Camera.main.transform;
      starDistanceSqr = starSpawnRadius * starSpawnRadius;
      cloudParticleSystem = GetComponent<ParticleSystem>();
    }

    private void Start () 
    {
      currentClouds = cloudsMax;

      InitializeGradient();
      CreateCloud();
    }

    private void InitializeGradient()
    {
      gasGradient = new Gradient();

      GradientColorKey[] colorKey = new GradientColorKey[5];
      colorKey[0].color = Color.blue; 
      colorKey[0].time = 0.6f;
      colorKey[1].color = Color.cyan; 
      colorKey[1].time = 0.7f;
      colorKey[2].color = Color.red; 
      colorKey[2].time = 0.8f;
      colorKey[3].color = new Color(0.1f, 0.1f, 0.8f); 
      colorKey[3].time = 0.9f;
      colorKey[4].color = Color.black; 
      colorKey[4].time = 1f;

      GradientAlphaKey[] alphaKey = new GradientAlphaKey[3];
      alphaKey[0].alpha = 0.1f;
      alphaKey[0].time = 0.0f;
      alphaKey[1].alpha = 0.3f;
      alphaKey[1].time = 0.1f;
      alphaKey[2].alpha = 0.2f;
      alphaKey[2].time = 0.2f;

      gasGradient.SetKeys(colorKey, alphaKey);
    }

    private void CreateCloud()
    {
      cloudParticles = new ParticleSystem.Particle[cloudsMax];

      for (int i = 0; i < currentClouds; i++)
      {
        cloudParticles[i].position = ((Vector3)lastPosition + ((Vector3)Random.insideUnitCircle * starSpawnRadius) + cameraTransform.position) / 2;
        cloudParticles[i].position = new Vector3(cloudParticles[i].position.x, cloudParticles[i].position.y, particleZ);
        lastPosition = cloudParticles[i].position;

        cloudParticles[i].rotation3D = new Vector3(0, 0, Random.Range(0, 360));
        cloudParticles[i].startSize = Random.Range(starSizeMin, starSizeMax);
        cloudParticles[i].startColor = GetPerlinColor();
      }

      cloudParticleSystem.SetParticles(cloudParticles, cloudParticles.Length);
    }
 
    private void Update() 
    {
      Vector3 cameraParallaxDelta = (Vector2)(cameraTransform.position - parallaxLayer.position);

      for (int i = 0; i < currentClouds; i++)
      {
        Vector2 starPosition = (Vector2)(cloudParticles[i].position + parallaxLayer.position);

        if((starPosition - (Vector2)cameraTransform.position).sqrMagnitude > starDistanceSqr) 
        {
          cloudParticles[i].position = (Vector2)(Random.insideUnitCircle.normalized * starSpawnRadius) + (Vector2)cameraParallaxDelta;
          cloudParticles[i].startColor = GetPerlinColor();
        }
      }
    }

    public void Parallax(Vector3 cameraLastPosition)   //called on camera controller
    {
      Vector3 cameraDelta = (Vector2)(cameraTransform.position - cameraLastPosition); 

      parallaxLayer.position = Vector3.Lerp(
        parallaxLayer.position, 
        parallaxLayer.position + cameraDelta, 
        parallaxFactor * Time.deltaTime
      );

      cloudParticleSystem.SetParticles(cloudParticles, cloudParticles.Length);
    }

    private int GetCurrentCloudsCount()
    {
      float sinValue = Mathf.Abs(Mathf.Sin(cameraTransform.position.x * perlinScale + perlinOffset.x));
      if (sinValue > 0.5f) sinValue = 0f;
      return (int)(sinValue * 100);
    }

    private Vector2 GetNewPosition()
    {
      float perlinValueX = Mathf.PerlinNoise(cameraTransform.position.x * perlinScale + perlinOffset.x, 0);
      float perlinValueY = Mathf.PerlinNoise(0, cameraTransform.position.y * perlinScale + perlinOffset.y);

      return new Vector2(perlinValueX, perlinValueY); 
    }

    private Color GetPerlinColor()
    {
      float perlinValue = Mathf.PerlinNoise(cameraTransform.position.x * 0.3f * perlinScale + perlinOffset.x, 0);
      float colorPerlinValue = Mathf.Abs(Mathf.Sin(perlinValue * Mathf.PI));

      Color cloudColor = gasGradient.Evaluate(colorPerlinValue);
      return cloudColor;
    }
  }
}
