using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class ShipShoot : MonoBehaviour
  {
    [SerializeField] GameObject laserObject; //Set in inspector
    [SerializeField] LaserParticleFire laserWeapon;

    public void Start()
    {
      laserWeapon = laserObject.GetComponent<LaserParticleFire>();
    }

    public void Update()
    {
      if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
      {
        laserWeapon.Shoot();
      }
    }  
  }
}