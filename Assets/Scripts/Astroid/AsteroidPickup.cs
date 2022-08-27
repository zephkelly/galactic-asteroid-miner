using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class AsteroidPickup : MonoBehaviour
  {
    [SerializeField] AsteroidStatistics asteroidStats;

    [SerializeField] float asteroidValue;

    public void Start()
    {
      //A way to randomly assign an asteroid type
      //asteroidType = (AsteroidType)Random.Range(0, System.Enum.GetValues(typeof(AsteroidType)).Length);

      asteroidStats.Configure();
      asteroidValue = asteroidStats.Value();
    }
  }
}