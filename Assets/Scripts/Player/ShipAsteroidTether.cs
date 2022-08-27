using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class ShipAsteroidTether : MonoBehaviour
  {
    [SerializeField] Collider2D shipCollider;
    private Transform tetherTransform;

    public void Awake()
    {
      shipCollider = gameObject.GetComponentInParent<Collider2D>();
      tetherTransform = gameObject.transform;
    }

    public void Start()
    {
    }

    public void Update()
    {

    }
  }
}