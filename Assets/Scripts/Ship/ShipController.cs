using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class ShipController : MonoBehaviour
  {
    private InputManager playerInputs;
    private Inventory playerInventory;
    private ShipConfiguration shipConfiguration;

    private Rigidbody2D playerRigid2D;
    private Transform playerTransform;
    
    private Vector2 mouseDirection;
    [SerializeField] float moveSpeed = 60f;

    //Orbiting Variables
    private StarOrbitingBehaviour starBehaviour;
    private Vector2 orbitalVelocity;
    private Vector2 lastOrbitalVelocity;
    private bool orbitingStar;

    //----------------------------------------------------------------------------------------------

    public static event Action OnPlayerDied;

    public Inventory Inventory => playerInventory;

    private void Awake()
    {
      playerInventory = Resources.Load("ScriptableObjects/PlayerInventory") as Inventory;
      shipConfiguration = new ShipConfiguration(this);
      shipConfiguration.AssignDefaults();

      playerTransform = gameObject.transform;
      playerRigid2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
      orbitingStar = false;
      playerInputs = InputManager.Instance;
      playerRigid2D.centerOfMass = Vector2.zero;
    }

    private void Update()
    {
      LookAtMouse();
      mouseDirection = playerInputs.MouseWorldPosition - (Vector2) transform.position;
      mouseDirection.Normalize();

    }

    private void FixedUpdate()
    {
      if (Input.GetKey(KeyCode.LeftShift)) {
        playerRigid2D.AddForce(playerInputs.KeyboardInput * (moveSpeed * 3), ForceMode2D.Force);
      } else {
        playerRigid2D.AddForce(playerInputs.KeyboardInput * moveSpeed, ForceMode2D.Force);
      }
      
      if (orbitingStar) {
        StarOrbiting();
        return;
      }

      //Linear drag
      playerRigid2D.AddForce(-playerRigid2D.velocity * playerRigid2D.mass, ForceMode2D.Force);
      if (playerRigid2D.velocity.magnitude < 0.1f) playerRigid2D.velocity = Vector2.zero;
    }

    private void StarOrbiting()
    {
      if (starBehaviour == null) {
        Debug.LogError("Error: StarOrbiting() null reference");
        return;
      }

      //MSet constant orbit velocity
      lastOrbitalVelocity = orbitalVelocity;
      orbitalVelocity = starBehaviour.GetOrbitalVelocity(playerRigid2D);

      playerRigid2D.velocity -= lastOrbitalVelocity;   //Working around unity physics
      playerRigid2D.velocity += orbitalVelocity;

      var orbitalDragX = new Vector2(orbitalVelocity.x - playerRigid2D.velocity.x, 0);
      var orbitalDragY = new Vector2(0, orbitalVelocity.y - playerRigid2D.velocity.y);

      //Orbital drag
      if (playerRigid2D.velocity.x > orbitalVelocity.x || playerRigid2D.velocity.x < orbitalVelocity.x) {
        playerRigid2D.AddForce(orbitalDragX * playerRigid2D.mass, ForceMode2D.Force);
      }

      if (playerRigid2D.velocity.y > orbitalVelocity.y || playerRigid2D.velocity.y < orbitalVelocity.y) {
        playerRigid2D.AddForce(orbitalDragY * playerRigid2D.mass, ForceMode2D.Force);
      }  
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      if (!other.CompareTag("Star")) return;

      orbitingStar = true;
      starBehaviour = other.GetComponent<StarOrbitingBehaviour>();
      starBehaviour.ApplyInstantOrbitalVelocity(playerRigid2D);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
      if (!other.CompareTag("Star")) return;

      orbitingStar = false;
      starBehaviour = null;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
      if (!other.gameObject.CompareTag("Star")) return;

      OnPlayerDied?.Invoke();
      Destroy(gameObject);
    }

    private void LookAtMouse()
    {
      float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;
      transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    //private void OnDestroy() => OnPlayerDied?.Invoke();
  }
}
