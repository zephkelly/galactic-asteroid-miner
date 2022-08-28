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

    [SerializeField] Rigidbody2D rigid2D;
    private Transform shipTransform;
    private Vector2 thrustDirection;

    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;
    private bool toggleMode = false;

    public void Awake()
    {
      rigid2D = GetComponent<Rigidbody2D>();
      shipTransform = gameObject.GetComponent<Transform>();
    }

    private void Start()
    {
      inputs = InputManager.Instance;
    }

    public void Update()
    {
      ModerationTools();

      thrustDirection = inputs.MouseWorldPosition - (Vector2) shipTransform.position;
      thrustDirection.Normalize();

      transform.up = thrustDirection;
    }

    public void FixedUpdate()
    {
      if (toggleMode)
      {
        if (Input.GetKey(KeyCode.W))
        {
          rigid2D.AddForce(thrustDirection * moveSpeed, ForceMode2D.Force);
        }
        else if (Input.GetKey(KeyCode.S))
        {
          rigid2D.AddForce(-thrustDirection * (moveSpeed / 3), ForceMode2D.Force);
        }
      } else
      {
        rigid2D.AddForce(inputs.KeyboardInput * moveSpeed, ForceMode2D.Force);
      }
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

      //Toggle flight modes
      if (Input.GetKeyDown(KeyCode.Tab))
      {
        toggleMode = !toggleMode;
      }
    } 
  }
}