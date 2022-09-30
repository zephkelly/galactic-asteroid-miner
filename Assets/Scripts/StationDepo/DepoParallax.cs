using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class DepoParallax : MonoBehaviour
  {
    private Transform cameraTransform;
    [SerializeField] Transform depoOuterLayerTransform;
    [SerializeField] Transform depoInnerLayerTransform;

    [SerializeField] float parallaxFactorOuter;
    [SerializeField] float parallaxFactorInner;

    //----------------------------------------------------------------------------------------------

    private void Awake()
    {
      cameraTransform = Camera.main.transform;
    }

    public void Parallax(Vector2 cameraLastPosition)
    {
      Vector2 cameraDelta = (Vector2)cameraTransform.position - cameraLastPosition;
      Vector2 depoPositionOuter = (Vector2)depoOuterLayerTransform.position;
      Vector2 depoPositionInner = (Vector2)depoInnerLayerTransform.position;

      depoOuterLayerTransform.position = Vector2.Lerp(
        depoPositionOuter,
        depoPositionOuter + cameraDelta,
        parallaxFactorOuter * Time.deltaTime
      );

      depoInnerLayerTransform.position = Vector2.Lerp(
        depoPositionInner,
        depoPositionInner + cameraDelta,
        parallaxFactorInner * Time.deltaTime
      );
    }
  }
}
