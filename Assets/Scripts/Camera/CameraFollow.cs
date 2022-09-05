using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class CameraFollow : MonoBehaviour
  {
    private Camera mainCamera;
    private Transform cameraTransform;
    [SerializeField] ParallaxStarfield[] parallaxStarfield;

    //----------------------------------------------------------------------------------------------

    [SerializeField] float mouseInterpolateDistance = 2f;
    [SerializeField] float cameraPanSpeed = 0.125f;

    private Transform target;
    private Vector3 targetVector;
    private Vector3 mousePosition;
    private Vector3 cameraLastPosition;

    //----------------------------------------------------------------------------------------------

    public void Awake()
    {
      mainCamera = Camera.main;
      cameraTransform = mainCamera.transform;
    }

    public void Start()
    {
      target = GameObject.Find("Player").transform;
    }

    private void Update()
    {
      mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
      mousePosition = mousePosition - target.position;
      mousePosition.Normalize();
      //beacuse the camera is wider than it is tall
      mousePosition.y = mousePosition.y * 1.4f;
    }

    public void LateUpdate()
    {
      if (target == null) return;
      
      targetVector = target.position + (mousePosition * mouseInterpolateDistance);
      targetVector.z = -10;

      Vector3 cameraLastPosition = cameraTransform.position;

      Vector3 newCameraPosition = Vector3.Lerp(cameraTransform.position, targetVector, cameraPanSpeed);
      cameraTransform.position = newCameraPosition;

      //Parallax starfield layers
      foreach (ParallaxStarfield starfield in parallaxStarfield)
      {
        starfield.Parallax(cameraLastPosition);
      }
    }

    public void ChangeFocus(Transform newTarget)
    {
      target = newTarget;
    }
  }
}