using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class ShipController : MonoBehaviour
  {
    private Rigidbody2D rigid2D;
    private Transform shipTransform;

    [SerializeField] GameObject smallAsteroid;
    [SerializeField] GameObject mediumAsteroid;
    [SerializeField] GameObject largeAsteroid;

    [SerializeField] Vector2 playerInput;
    [SerializeField] Vector2 mousePosition;
    private Vector3 directionToMouse;

    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;

    public void Awake()
    {
      rigid2D = gameObject.GetComponent<Rigidbody2D>();
      shipTransform = gameObject.GetComponent<Transform>();
    }

    public void Update()
    {
      UpdateInputs();
      ModerationTools();

      transform.up = directionToMouse;
    }

    private void UpdateInputs()
    {
      //Mouse input code
      mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

      directionToMouse = (Vector2) mousePosition - rigid2D.position;
      directionToMouse.Normalize();

      //Keyboard input code
      var inputX = Input.GetAxisRaw("Horizontal");
      var inputY = Input.GetAxisRaw("Vertical");

      playerInput = new Vector2(inputX, inputY);
      playerInput.Normalize();
    }

    public void FixedUpdate()
    {
      rigid2D.AddForce(playerInput * moveSpeed, ForceMode2D.Force);
    }

    private void ModerationTools()
    {
      if (Input.GetKeyDown(KeyCode.Alpha1))
      {
        Instantiate(smallAsteroid, mousePosition, Quaternion.identity);
      }
      else if (Input.GetKeyDown(KeyCode.Alpha2))
      {
        Instantiate(mediumAsteroid, mousePosition, Quaternion.identity);
      }
      if (Input.GetKeyDown(KeyCode.Alpha3))
      {
        Instantiate(largeAsteroid, mousePosition, Quaternion.identity);
      }
    } 
  }
}