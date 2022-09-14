using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace zephkelly
{
  public class StarOrbitingBehaviour : MonoBehaviour
  {
    private const float G = 0.2f;   //Newtons Gravity constant

    private Rigidbody2D starRigidbody;
    private List<Rigidbody2D> orbitingBodies = new List<Rigidbody2D>();

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

    public void ApplyInstantOrbitalVelocity(Rigidbody2D body)
    {
      float bodyMass = body.mass;
      float starMass = starRigidbody.mass;
      float distanceToStar = Vector2.Distance(starRigidbody.position, body.position);

      Vector2 directionToStar = (starRigidbody.position - body.position).normalized;
      Vector2 perpendicularDirection = Vector2.Perpendicular(directionToStar);

      Vector2 appliedOrbitalVelocity = perpendicularDirection * Mathf.Sqrt((G * starMass) / distanceToStar);

      //Only apply enough force to orbit the star
      if (body.velocity.magnitude > appliedOrbitalVelocity.magnitude) return;
      
      var deltaVelocity = appliedOrbitalVelocity - body.velocity;
      body.velocity += deltaVelocity;
    }

    public Vector2 GetOrbitalVelocity(Rigidbody2D body)
    {
      float bodyMass = body.mass;
      float starMass = starRigidbody.mass;
      float distanceToStar = Vector2.Distance(starRigidbody.position, body.position);

      Vector2 directionToStar = (starRigidbody.position - body.position).normalized;
      Vector2 perpendicularDirection = Vector2.Perpendicular(directionToStar);

      return perpendicularDirection * Mathf.Sqrt((G * starMass) / distanceToStar);
    }
  }
}