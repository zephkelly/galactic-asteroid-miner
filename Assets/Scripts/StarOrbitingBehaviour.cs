using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class StarOrbitingBehaviour : MonoBehaviour
  {
    private GameObject star;

    [SerializeField] float starOrbitSpeed = 0.2f;
    [SerializeField] float distanceSpeedModifier = 0.8f;
    [SerializeField] float dragModifier = 1f;

    private List<Rigidbody2D> orbitingBodies = new List<Rigidbody2D>();

    private float _playerDrag;

    private void Awake()
    {
      star = gameObject;
    }

    private void Update()
    {
      foreach (Rigidbody2D body in orbitingBodies)
      {
        var directionToStar = (star.transform.position - body.transform.position).normalized;
        var perpendicularDirection = Vector2.Perpendicular(directionToStar);

        var distanceToStar = Vector2.Distance(star.transform.position, body.transform.position);
        var rotationForce = starOrbitSpeed * (distanceToStar * distanceSpeedModifier);

        //Arbitrary force applied based on my weird mass formula
        body.AddForce(perpendicularDirection * rotationForce * (body.mass / 40), ForceMode2D.Force);
      }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
      var _rigid2D = other.gameObject.GetComponent<Rigidbody2D>();

      if(other.gameObject.tag == "Player") _playerDrag = other.gameObject.GetComponent<Rigidbody2D>().drag;

      orbitingBodies.Add(_rigid2D);
      _rigid2D.drag = dragModifier;
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
      var _rigid2D = other.gameObject.GetComponent<Rigidbody2D>();

      orbitingBodies.Remove(_rigid2D);
      _rigid2D.drag = 0f;

      if(other.gameObject.tag == "Player") _rigid2D.drag = _playerDrag;
    }
  }
}