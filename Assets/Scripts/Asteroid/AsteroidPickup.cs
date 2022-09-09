using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class AsteroidPickup : MonoBehaviour
  {
    public Rigidbody2D rigid2D;

    [SerializeField] AsteroidType asteroidType;
    [SerializeField] int asteroidAmount;

    public int Amount { get { return asteroidAmount; } }
    public AsteroidType Type { get { return asteroidType; } }

    public void CreatePickup(AsteroidType type)
    {
      asteroidType = type;
      rigid2D = GetComponent<Rigidbody2D>();

      DeterminAmount();

      switch (type)
      {
        case AsteroidType.Iron:
          IronAsteroid();
          break;
        case AsteroidType.Cobalt:
          CobaltAsteroid();
          break;
        case AsteroidType.Gold:
          GoldAsteroid();
          break;
      }
    }

    private void DeterminAmount()
    {
      var randomNumber = Random.Range(1, 5);

      if (randomNumber == 1 )
      {
        asteroidAmount = 2;
      }
      else
      {
        asteroidAmount = 1;
      }
    }

    private void IronAsteroid()
    {
      gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void CobaltAsteroid()
    {
      gameObject.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.5f, 1f, 1f);
    }

    private void GoldAsteroid()
    {
      gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0.8f, 0.3f, 1f);
    }
  }
}