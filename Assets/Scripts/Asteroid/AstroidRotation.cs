using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class AstroidRotation : MonoBehaviour
  {
    private AsteroidController asteroidController;
    private Transform asteroidTransform;
    private Rigidbody2D rigid2D;

    [SerializeField, Range(0, 1)] float maxRotation;
    [SerializeField, Range(0, 100)] float rotationMultiplier = 1f;

    public void Awake()
    {
      asteroidController = gameObject.GetComponent<AsteroidController>();
      asteroidTransform = gameObject.GetComponent<Transform>();
      rigid2D = gameObject.GetComponent<Rigidbody2D>();
    }

    public void Start()
    {
      float randomSpin = Random.Range(-1f, 1f);

      asteroidTransform.RotateAround(asteroidTransform.position, Vector3.forward, Random.Range(0, 360));

      if (asteroidController.AsteroidInfo.Size == AsteroidSize.Large)
      {
        rigid2D.AddTorque(Mathf.Clamp(randomSpin, -maxRotation, maxRotation) * rotationMultiplier * Random.Range(10, 22), ForceMode2D.Impulse);
      }
      else {
        rigid2D.AddTorque(Mathf.Clamp(randomSpin, -maxRotation, maxRotation) * rotationMultiplier, ForceMode2D.Impulse);
      }
    }
  }
}