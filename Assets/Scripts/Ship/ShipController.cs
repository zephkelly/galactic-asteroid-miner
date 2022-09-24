using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class ShipController : MonoBehaviour
  {
    private InputManager inputs;
    private Inventory inventory;
    private Rigidbody2D rigid2D;

    //----------------------------------------------------------------------------------------------

    [SerializeField] float moveSpeed = 60f;
    [SerializeField] bool moveToMouse;
    private Vector2 mouseDirection;

    //Star-Orbiting-Behaviour-----------------------------------------------------------------------

    private StarOrbitingBehaviour _currentStarBehaviour;
    private Vector2 _shipOrbitalVelocity;
    private Vector2 _lastVelocity;
    private bool _activateStarOrbiting;

    //----------------------------------------------------------------------------------------------

    public Inventory Inventory => inventory;

    private void Awake()
    {
      rigid2D = GetComponent<Rigidbody2D>();
      inventory = Resources.Load("ScriptableObjects/PlayerInventory") as Inventory;
    }

    private void Start()
    {
      _activateStarOrbiting = false;
      inputs = InputManager.Instance;
      rigid2D.centerOfMass = Vector2.zero;
    }

    private void Update()
    {
      mouseDirection = inputs.MouseWorldPosition - (Vector2) transform.position;
      mouseDirection.Normalize();

      if (Input.GetKeyDown(KeyCode.Tab)) moveToMouse = !moveToMouse;
      
      LookAtMouse();
    }

    private void FixedUpdate()
    {
      ControlBehaviour();
      
      if (_activateStarOrbiting) 
      {
        StarOrbiting();
      }
      else 
      {
        //Linear dragging while in space
        rigid2D.AddForce(-rigid2D.velocity * rigid2D.mass, ForceMode2D.Force);

        if (rigid2D.velocity.magnitude < 0.1f)
        {
          rigid2D.velocity = Vector2.zero;
        }
      }
    }

    private void ControlBehaviour()
    {
      if (moveToMouse)
      {
        if (Input.GetKey(KeyCode.LeftShift))
        {
          rigid2D.AddForce(mouseDirection * (moveSpeed * 3), ForceMode2D.Force);
        }
        else
        {
          rigid2D.AddForce(mouseDirection * moveSpeed, ForceMode2D.Force);
        }
      }
      else
      {
        if (Input.GetKey(KeyCode.LeftShift))
        {
          rigid2D.AddForce(inputs.KeyboardInput * (moveSpeed * 3), ForceMode2D.Force);
        }
        else
        {
          rigid2D.AddForce(inputs.KeyboardInput * moveSpeed, ForceMode2D.Force);
        }
      }
    }

    private void StarOrbiting()
    {
      if (_currentStarBehaviour == null) 
      {
        print("Error: StarOrbiting() called but no StarOrbiting behaviour found on player");
        return;
      }

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

    private void LookAtMouse()
    {
      float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;
      transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      //We will get a null ref here because if we touch a star we will be destroyed
      
      if (!other.CompareTag("Star")) return;

      //Activate star behaviour
      _currentStarBehaviour = other.GetComponent<StarOrbitingBehaviour>();
      _currentStarBehaviour.ApplyInstantOrbitalVelocity(rigid2D);
      _activateStarOrbiting = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
      if (!other.CompareTag("Star")) return;

      _currentStarBehaviour = null;
      _activateStarOrbiting = false;
    }
  }
}
