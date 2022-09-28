using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class DepoRotation : MonoBehaviour
  {
    [SerializeField] Transform innerTransform;
    [SerializeField] Transform outerTransform;

    [SerializeField] float innerRotationSpeed = 0.0f;
    [SerializeField] float outerRotationSpeed = 0.0f;

    [SerializeField] float innerRotationDirection = 1.0f;
    [SerializeField] float outerRotationDirection = -1.0f;

    private void Update()
    {
      innerTransform.Rotate(0.0f, 0.0f, innerRotationSpeed * innerRotationDirection * Time.deltaTime);
      outerTransform.Rotate(0.0f, 0.0f, outerRotationSpeed * outerRotationDirection * Time.deltaTime);
    }
  }
}