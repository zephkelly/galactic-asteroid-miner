using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class StarAsteroidBeltGenerator : MonoBehaviour
  {
    private GameObject mediumAsteroidPrefab;
    
    [SerializeField] float beltMinumumRadius = 60f;
    [SerializeField] float beltMaximumRadius = 80f;
    [SerializeField] float beltDensity;

    //Variables unique to spawned asteroid
    private Vector2 localPosition;
    private Vector2 worldOffset;
    private Vector2 worldPosition;
    private float randomRadius;
    private float randomAngle;
    private float positionX;
    private float positionY;

    private void Awake()
    {
      Random.InitState(System.DateTime.Now.Millisecond);

      mediumAsteroidPrefab = Resources.Load("Prefabs/Asteroid-M") as GameObject;
    }

    private void Start()
    {
      for (int i = 0; i < beltDensity; i++)
      {
        do
        {
          randomRadius = Random.Range(beltMinumumRadius, beltMaximumRadius);
          randomAngle = Random.Range(0f, (2 * Mathf.PI));

          positionX = randomRadius * Mathf.Cos(randomAngle);
          positionY = randomRadius * Mathf.Sin(randomAngle);

          localPosition = new Vector2(positionX, positionY);
          worldPosition = (Vector2) transform.position + localPosition;

          GameObject _asteroid = Instantiate(mediumAsteroidPrefab, worldPosition, Quaternion.identity);

          var newAsteroidInfo = new Asteroid();
          newAsteroidInfo.Type = AsteroidType.Cobalt;
          newAsteroidInfo.Size = AsteroidSize.Medium;
          newAsteroidInfo.SpawnPosition = worldPosition;

          _asteroid.GetComponent<AsteroidController>().SetAsteroid(newAsteroidInfo, new Chunk());
          _asteroid.transform.parent = transform;
        }
        while (float.IsNaN(positionX) && float.IsNaN(positionY));
      }
    }
  }
}