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
    [SerializeField] bool invulnerable = false;

    //Orbiting Variables
    private StarController starController;
    private Vector2 orbitalVelocity;
    private Vector2 lastOrbitalVelocity;
    private bool orbitingStar;

    //----------------------------------------------------------------------------------------------

    public static event Action OnPlayerDied;
    public Inventory Inventory => playerInventory;
    public bool OrbitingStar => orbitingStar;

    public void Die()
    {
      OnPlayerDied?.Invoke();
      Destroy(gameObject);
    }

    private void Awake()
    {
      playerInventory = Resources.Load("ScriptableObjects/PlayerInventory") as Inventory;
      shipConfiguration = GetComponent<ShipConfiguration>();

      playerTransform = transform;
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

      if (!orbitingStar) return;

      CalculateThermalGradient();
    }

    private void LookAtMouse()
    {
      float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;
      transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    private void CalculateThermalGradient()
    {
      float distanceToStar = Vector2.Distance(transform.position, starController.transform.position);

      float ambientTemperature = starController.OrbitingBehaviour.GetThermalGradient(distanceToStar);

      shipConfiguration.SetAmbientTemperature = ambientTemperature;
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
      if (starController == null) {
        Debug.LogError("Error: StarOrbiting() null reference");
        return;
      }

      //MSet constant orbit velocity
      lastOrbitalVelocity = orbitalVelocity;
      orbitalVelocity = starController.OrbitingBehaviour.GetOrbitalVelocity(playerRigid2D);

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
      starController = other.GetComponent<StarController>();
      starController.OrbitingBehaviour.ApplyInstantOrbitalVelocity(playerRigid2D);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
      if (!other.CompareTag("Star")) return;

      shipConfiguration.SetAmbientTemperature = 0f;

      orbitingStar = false;
      starController = null;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
      GameObject otherObject = other.gameObject;

      if (otherObject.CompareTag("Asteroid"))
      {
        if (invulnerable) return;

        var damage = 10;

        var healthRemaining = shipConfiguration.TakeDamage(damage);

        Debug.Log($"Player health remaining: {healthRemaining}");
        return;
      }
      else if (otherObject.CompareTag("Star"))
      {
        Die();
        return;
      }
    }
  }
}
