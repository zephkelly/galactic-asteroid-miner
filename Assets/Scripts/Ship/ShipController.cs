using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace zephkelly
{
  public class ShipController : MonoBehaviour
  {
    public static ShipController Instance;

    private InputManager playerInputs;
    private Inventory playerInventory;
    private ShipConfiguration shipConfiguration;
    private ShipShoot shipShoot;

    private Rigidbody2D playerRigid2D;
    private Transform playerTransform;
    
    private Vector2 mouseDirection;
    [SerializeField] float moveSpeed = 60f;

    //Orbiting Variables
    private StarController starController;
    private Vector2 orbitalVelocity;
    private Vector2 lastOrbitalVelocity;
    private bool orbitingStar;

    //----------------------------------------------------------------------------------------------

    public ShipConfiguration ShipConfig { get => shipConfiguration; }
    public int MaxHealth { get => shipConfiguration.HullStrengthMax; }
    public int Health { get => shipConfiguration.HullStrengthCurrent; }
    public float MaxFuel { get => shipConfiguration.FuelMax; }
    public float Fuel { get => shipConfiguration.FuelCurrent; }
    public Inventory Inventory { get => playerInventory; }

    public static event Action OnPlayerDied;

    private void Awake()
    {
      playerInventory = Resources.Load("ScriptableObjects/PlayerInventory") as Inventory;

      playerInventory.AddItem(AsteroidType.Iron.ToString(), 0);
      playerInventory.AddItem(AsteroidType.Platinum.ToString(), 0);
      playerInventory.AddItem(AsteroidType.Titanium.ToString(), 0);
      playerInventory.AddItem(AsteroidType.Gold.ToString(), 0);
      playerInventory.AddItem(AsteroidType.Palladium.ToString(), 0);
      playerInventory.AddItem(AsteroidType.Cobalt.ToString(), 0);
      playerInventory.AddItem(AsteroidType.Stellarite.ToString(), 0);
      playerInventory.AddItem(AsteroidType.Darkore.ToString(), 0);

      shipConfiguration = GetComponent<ShipConfiguration>();
      shipShoot = GetComponent<ShipShoot>();

      playerTransform = transform;
      playerRigid2D = GetComponent<Rigidbody2D>();

      //Set up singleton
      if (Instance == null) {
        Instance = this;
      } else {
        Destroy(gameObject);
      }
    }

    private void Start()
    {
      orbitingStar = false;
      playerInputs = InputManager.Instance;
      playerRigid2D.centerOfMass = Vector2.zero;
    }

    public void Die()
    {
      OnPlayerDied?.Invoke();
      Destroy(gameObject);
    }

    public void CanShoot(bool toggleFire)
    {
      shipShoot.ToggleFiring = toggleFire;
    }

    public void SetStarBehaviour(StarController _starController, bool _orbitingStar = true)
    {
      orbitingStar = _orbitingStar;

      if (orbitingStar)
      {
        starController = _starController;
        orbitingStar = true;
      }
      else
      {
        starController = null;
        orbitingStar = false;
      }
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

    private void OnCollisionEnter2D(Collision2D other)
    {
      GameObject otherObject = other.gameObject;

      if (otherObject.CompareTag("Asteroid"))
      {
        var damage = 10;
        var healthRemaining = shipConfiguration.TakeDamage(damage);
        
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
