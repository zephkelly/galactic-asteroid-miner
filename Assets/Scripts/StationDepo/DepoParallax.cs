using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class DepoParallax : MonoBehaviour
  {
    private Transform cameraTransform;
    [SerializeField] Transform depoOuterLayerTransform;

    [SerializeField] float parallaxFactor;

    //----------------------------------------------------------------------------------------------

    private void Awake()
    {
      cameraTransform = Camera.main.transform;
    }

    public void Parallax(Vector2 cameraLastPosition)
    {
      Vector2 cameraDelta = (Vector2)cameraTransform.position - cameraLastPosition;
      Vector2 depoPosition = (Vector2)depoOuterLayerTransform.position;

      depoOuterLayerTransform.position = Vector2.Lerp(
        depoPosition,
        depoPosition + cameraDelta,
        parallaxFactor * Time.deltaTime
      );
    }
  }
}
