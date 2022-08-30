using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class ShipController : MonoBehaviour
  {
    private InputManager inputs;

    [SerializeField] GameObject smallAsteroid;
    [SerializeField] GameObject mediumAsteroid;
    [SerializeField] GameObject largeAsteroid;

    //Information about the star we are orbiting
    private OrbitingBehaviour _currentStarBehaviour;
    private Vector2 _starAverageOrbitalVelocity;
    private Vector2 _shipOrbitalVelocity;
    private Vector2 _lastVelocity;
    private bool _activateStarOrbiting;


    private Rigidbody2D rigid2D;
    private Vector2 mouseDirection;

    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;

    public void Awake()
    {
      rigid2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
      _activateStarOrbiting = false;
      inputs = InputManager.Instance;
    }

    public void Update()
    {
      ModerationTools();

      mouseDirection = inputs.MouseWorldPosition - (Vector2) rigid2D.position;
      transform.up = mouseDirection;
    }

    public void FixedUpdate()
    {
      //Regular movement
      rigid2D.AddForce(inputs.KeyboardInput * moveSpeed, ForceMode2D.Force);

      //Movement while in orbit
      if (_activateStarOrbiting)
      {
        //Makes sure that we are travelling the correct speed around the star
        _lastVelocity = _shipOrbitalVelocity;
        _shipOrbitalVelocity = _currentStarBehaviour.GetOrbitalVelocity(rigid2D);

        rigid2D.velocity -= _lastVelocity;
        rigid2D.velocity += _shipOrbitalVelocity;

        //Dragging While Orbiting
        if (rigid2D.velocity.x > _shipOrbitalVelocity.x || rigid2D.velocity.x < _shipOrbitalVelocity.x) {
          rigid2D.AddForce(new Vector2((_shipOrbitalVelocity.x - rigid2D.velocity.x), 0) * rigid2D.mass, ForceMode2D.Force);
        }

        if (rigid2D.velocity.y > _shipOrbitalVelocity.y || rigid2D.velocity.y < _shipOrbitalVelocity.y) {
          rigid2D.AddForce(new Vector2(0, (_shipOrbitalVelocity.y - rigid2D.velocity.y)) * rigid2D.mass, ForceMode2D.Force);
        }    
      } 
      else 
      {
        //Regular drag
        rigid2D.AddForce(-rigid2D.velocity * rigid2D.mass, ForceMode2D.Force);
      }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      if (!other.CompareTag("Star")) return;

      //Activate star behaviour
      _activateStarOrbiting = true;
      _currentStarBehaviour = other.GetComponent<OrbitingBehaviour>();
      _currentStarBehaviour.ApplyInstantOrbitalVelocity(rigid2D);
      _starAverageOrbitalVelocity = _currentStarBehaviour.GetAverageOrbitingSpeed();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
      if (!other.CompareTag("Star")) return;

      _activateStarOrbiting = false;
      _currentStarBehaviour = null;
    }

    private void ModerationTools()
    {
      //Spawn different asteroids
      if (Input.GetKeyDown(KeyCode.Alpha1))
      {
        Instantiate(smallAsteroid, inputs.MouseWorldPosition, Quaternion.identity);
      }
      else if (Input.GetKeyDown(KeyCode.Alpha2))
      {
        Instantiate(mediumAsteroid, inputs.MouseWorldPosition, Quaternion.identity);
      }
      if (Input.GetKeyDown(KeyCode.Alpha3))
      {
        Instantiate(largeAsteroid, inputs.MouseWorldPosition, Quaternion.identity);
      }
    } 
  }
}