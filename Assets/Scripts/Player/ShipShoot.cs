using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class ShipShoot : MonoBehaviour
  {
    [SerializeField] GameObject laserObject; //Set in inspector
    [SerializeField] LaserScript laserScript;

    public void Start()
    {
      laserScript = laserObject.GetComponent<LaserScript>();
    }

    public void Update()
    {
      if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
      {
        laserScript.Shoot();
      }
    }  
  }
}