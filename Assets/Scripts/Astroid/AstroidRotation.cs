using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class AstroidRotation : MonoBehaviour
  {
    private Rigidbody2D rigid2D;

    [SerializeField, Range(0, 1)] float maxRotation;
    [SerializeField, Range(0, 100)] float rotationMultiplier = 1f;

    public void Awake()
    {
      rigid2D = gameObject.GetComponent<Rigidbody2D>();
    }

    public void Start()
    {
      float randomSpin = Random.Range(-1f, 1f);
      
      rigid2D.AddTorque(Mathf.Clamp(randomSpin, -maxRotation, maxRotation) * rotationMultiplier, ForceMode2D.Impulse);
    }
  }
}