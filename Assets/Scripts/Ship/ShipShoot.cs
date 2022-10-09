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

    private float fireRate = 0.40f;
    private float _fireTimer;
    private bool canFire = true;

    public bool ToggleFiring { get => canFire; set => canFire = value; }

    public ShipLaserFire LaserWeaponInfo { get => laserWeapon; }

    public void Start()
    {
      laserWeapon = laserObject.GetComponent<ShipLaserFire>();
    }

    public void Update()
    {
      if (GameManager.Instance.GamePaused) return;

      if (_fireTimer > 0)
      {
        _fireTimer -= Time.deltaTime;
        return;
      }

      if (!canFire) return;

      if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
      {
        zephkelly.AudioManager.Instance.PlaySoundRandomPitch("ShipShoot", 0.6f, 0.8f);

        _fireTimer = fireRate;
        laserWeapon.Shoot();
      }
    }

    public void SetFireRate(float _fireRate)
    {
      fireRate = _fireRate;
    }

    public void SetProjectileMat(Material _material)
    {
      laserWeapon.SetProjectileMat(_material);
    }

    public void SetProjectileSpeed(float _speed)
    {
      laserWeapon.SetProjectileSpeed(_speed);
    }
  }
}