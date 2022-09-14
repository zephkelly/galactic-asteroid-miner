using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class StarOrbitingBehaviour : MonoBehaviour
  {
    readonly float G = 0.2f;
    private Rigidbody2D starRigidbody;
    List<Rigidbody2D> orbitingBodies = new List<Rigidbody2D>();

    private float playerDrag;

    private void Awake()
    {
      starRigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D c) 
    {
      if (!(c.CompareTag("Asteroid") || c.CompareTag("AsteroidPickup"))) return;
      
      //Add to list of orbiting bodies and set velocity
      var newRigidbody = c.GetComponent<Rigidbody2D>();
      orbitingBodies.Add(newRigidbody);
      ApplyInstantOrbitalVelocity(newRigidbody);
    }

    private void OnTriggerExit2D(Collider2D c) 
    {
      if (!(c.CompareTag("Asteroid") || c.CompareTag("AsteroidPickup"))) return;

      var exitingRigidbody = c.GetComponent<Rigidbody2D>();
      orbitingBodies.Remove(exitingRigidbody);
    }

    private void FixedUpdate()
    {
      Gravity();
    }

    private void Gravity()
    {
      for (int i = 0; i < orbitingBodies.Count; i++)
      {
        Rigidbody2D body = orbitingBodies[i];

        float bodyMass = body.mass;
        float starMass = starRigidbody.mass;
        float distanceToStar = Vector2.Distance(starRigidbody.position, body.position);

        //Newtons gravitational theory
        float gravitationalForce = (G * bodyMass * starMass) / (distanceToStar * distanceToStar);

        body.AddForce((starRigidbody.position - body.position).normalized * gravitationalForce);
      }
    }

    public void ApplyInstantOrbitalVelocity(Rigidbody2D _body)
    {
      float bodyMass = _body.mass;
      float starMass = starRigidbody.mass;
      float distanceToStar = Vector2.Distance(starRigidbody.position, _body.position);

      Vector2 directionToStar = (starRigidbody.position - _body.position).normalized;
      Vector2 perpendicularDirection = Vector2.Perpendicular(directionToStar);

      _body.velocity += perpendicularDirection * Mathf.Sqrt((G * starMass) / distanceToStar);
    }

    public Vector2 GetOrbitalVelocity(Rigidbody2D _body)
    {
      float bodyMass = _body.mass;
      float starMass = starRigidbody.mass;
      float distanceToStar = Vector2.Distance(starRigidbody.position, _body.position);

      Vector2 directionToStar = (starRigidbody.position - _body.position).normalized;
      Vector2 perpendicularDirection = Vector2.Perpendicular(directionToStar);

      return perpendicularDirection * Mathf.Sqrt((G * starMass) / distanceToStar);
    }
  }
}