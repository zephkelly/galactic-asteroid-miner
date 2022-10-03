using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  //See LaserParticleFire.cs for more info
  public class ShipShoot : MonoBehaviour
  {
    [SerializeField] GameObject laserObject; //Set in inspector
    [SerializeField] ShipLaserFire laserWeapon;

    private const float fireTime = 0.40f;
    private float _fireTimer;
    private bool canFire = true;

    public bool ToggleFiring { get => canFire; set => canFire = value; }

    public void Start()
    {
      laserWeapon = laserObject.GetComponent<ShipLaserFire>();
    }

    public void Update()
    {
      if (_fireTimer > 0)
      {
        _fireTimer -= Time.deltaTime;
        return;
      }

      if (!canFire) return;

      if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
      {
        _fireTimer = fireTime;
        laserWeapon.Shoot();
      }
    }  
  }
}