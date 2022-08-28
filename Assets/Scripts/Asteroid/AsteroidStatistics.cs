using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  [CreateAssetMenu(fileName = "Asteroid", menuName = "ScriptableObjects/Asteroid", order = 0)]
  public class AsteroidStatistics : ScriptableObject 
  {
    enum AsteroidType { Iron, Nickel, Silver, Gold, Platinum, Cobalt, Titanium, }

    [SerializeField] AsteroidType asteroidType;

    [SerializeField] float asteroidDensity;
    [SerializeField] float asteroidValue;
    [SerializeField] float value;

    public float Value()
    {
      return value;
    }

    public void Configure()
    {
      asteroidValue = DetermineValue();

      asteroidDensity = RandomDensity();

      value = CalculateValue();
    }

    private float CalculateValue()
    {
      return asteroidValue * asteroidDensity;
    }

    private float RandomDensity()
    {
      return Random.Range(1f, 2f);
    }
   
    private float DetermineValue()
    {
      switch (asteroidType)
      {
        case AsteroidType.Iron:
          return 2f;
        case AsteroidType.Nickel:
          return 3f;
        case AsteroidType.Titanium:
          return 4f;
        case AsteroidType.Platinum:
          return 5f;
        case AsteroidType.Cobalt:
          return 6f;
        case AsteroidType.Silver:
          return 7f;
        case AsteroidType.Gold:
          return 8f;
        default:
          return 0f;
      }
    }
  }
}