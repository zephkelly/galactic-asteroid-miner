using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
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

    private Light2D shipExplosionLight;
    private GameObject shipExplosion;

    private ParticleSystem shipThrustersParticle;
    private Light2D shipThrustersLight;

    //Orbiting Variables
    private StarController starController;
    private Vector2 orbitalVelocity;
    private Vector2 lastOrbitalVelocity;
    private bool orbitingStar;

    //----------------------------------------------------------------------------------------------

    public ShipConfiguration ShipConfig { get => shipConfiguration; }
    public int EngineTopSpeed { get => shipConfiguration.EngineSpeed; }

    public int MaxHealth { get => shipConfiguration.HullStrengthMax; }
    public int Health { get => shipConfiguration.HullStrengthCurrent; }

    public float MaxFuel { get => shipConfiguration.FuelMax; }
    public float Fuel { get => shipConfiguration.FuelCurrent; }

    public int CargoCurrentCapacity { get => shipConfiguration.CargoBayCurrentCapacity; }
    public int CargoMaxCapacity { get => shipConfiguration.CargoBayMaxCapacity; }
    public Inventory Inventory { get => playerInventory; }

    private void Awake()
    {
      playerInventory = Resources.Load("ScriptableObjects/PlayerInventory") as Inventory;
      shipConfiguration = GetComponent<ShipConfiguration>();
      shipShoot = GetComponent<ShipShoot>();

      shipExplosion = Resources.Load("Prefabs/ShipExplosion") as GameObject;

      shipThrustersParticle = GameObject.FindGameObjectWithTag("ShipThruster").GetComponent<ParticleSystem>();
      shipThrustersLight = GameObject.FindGameObjectWithTag("ShipThrusterLight").GetComponent<Light2D>();
      shipThrustersLight.enabled = false;

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

    public void Die(string deathMessage)
    {
      Instantiate(shipExplosion, gameObject.transform.position, Quaternion.identity);

      GameManager.Instance.GameOver(deathMessage);

      if (zephkelly.AudioManager.Instance.IsSoundPlaying("ShipThrusters")) {
        zephkelly.AudioManager.Instance.StopSound("ShipThrusters");
      }

      zephkelly.AudioManager.Instance.PlaySound("ShipExplosion");

      Destroy(gameObject);
    }


    public void CanShoot(bool toggleFire)
    {
      shipShoot.ToggleFiring = toggleFire;
    }

    public void SetStarBehaviour(StarController _starController, bool _orbitingStar = true)
    {
      orbitingStar = _orbitingStar;

      if (orbitingStar) {
        starController = _starController;
        orbitingStar = true;
      } else {
        starController = null;
        orbitingStar = false;
      }
    }

    public void UpgradeEngine(Gradient newLifetimeColour)
    {
      var colourModule = shipThrustersParticle.colorOverLifetime;
      var trailColourModule = shipThrustersParticle.trails.colorOverLifetime;

      colourModule.color = newLifetimeColour;
      trailColourModule.gradient = newLifetimeColour;
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
      Debug.Log(ambientTemperature);
    }

    private void FixedUpdate()
    {
      if (Input.GetKey(KeyCode.LeftShift)) {

        playerRigid2D.AddForce(playerInputs.KeyboardInput * EngineTopSpeed, ForceMode2D.Force);
      } else {

        playerRigid2D.AddForce(playerInputs.KeyboardInput * 60, ForceMode2D.Force);
      }

      if (playerInputs.KeyboardInput != Vector2.zero && Input.GetKey(KeyCode.LeftShift)) {
        if (!zephkelly.AudioManager.Instance.IsSoundPlaying("ShipThrusters")) {
          zephkelly.AudioManager.Instance.PlaySound("ShipThrusters");
        }

        if (!shipThrustersLight.enabled)
        {
          shipThrustersParticle.Play();
          shipThrustersLight.enabled = true;
        }
      } 
      else {
        if (zephkelly.AudioManager.Instance.IsSoundPlaying("ShipThrusters")) {
          zephkelly.AudioManager.Instance.StopSound("ShipThrusters");
        }

        if (shipThrustersLight.enabled)
        {
          shipThrustersParticle.Stop();
          shipThrustersLight.enabled = false;
        }
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
        shipConfiguration.TakeDamage(damage);
        
        return;
      }
      else if (otherObject.CompareTag("Star"))
      {
        Die("You fell into a star...");
        return;
      }
    }
  }
}
