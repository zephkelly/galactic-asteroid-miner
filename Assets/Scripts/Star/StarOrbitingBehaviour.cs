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
      //Grab rigidbody from entering object
      var newRigidbody = c.GetComponent<Rigidbody2D>();

      
      //Set the correct orbital velocity if we arent the player
      if (c.CompareTag("Player")) return;
      if (c.CompareTag("Star")) return;

      //Add to list of orbiting bodies and set velocity
      orbitingBodies.Add(newRigidbody);
      ApplyInstantOrbitalVelocity(newRigidbody);
    }

    private void OnTriggerExit2D(Collider2D c) 
    {
      var exitingRigidbody = c.GetComponent<Rigidbody2D>();

      orbitingBodies.Remove(exitingRigidbody);
    }

    private void FixedUpdate()
    {
      Gravity();
    }

    private void Gravity()
    {
      foreach(Rigidbody2D body in orbitingBodies)
      {
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

    public Vector2 GetAverageOrbitingSpeed()
    {
      Vector2 sumOfOrbitingVectors = Vector2.zero;

      foreach(Rigidbody2D body in orbitingBodies)
      {
        sumOfOrbitingVectors += GetOrbitalVelocity(body);
      }

      return sumOfOrbitingVectors / orbitingBodies.Count;
    }
  }
}