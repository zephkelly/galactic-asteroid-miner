using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class CameraFollow : MonoBehaviour
  {
    private Transform cameraTransform;
    private Transform target;
    private Camera mainCamera;

    [SerializeField] float mouseInterpolateDistance = 2f;
    [SerializeField] float cameraPanSpeed = 0.125f;

      private Vector3 targetVector;
      private Vector3 mousePosition;

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
      mousePosition.y = mousePosition.y * 1.4f; //beacuse the camera is wider than it is tall

      //Follow();
    }

    public void LateUpdate()
    {
      if (target == null) return;
      
      targetVector = target.position + (mousePosition * mouseInterpolateDistance);
      targetVector.z = -10;

      cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetVector, cameraPanSpeed);
    }

    public void ChangeFocus(Transform newTarget)
    {
      target = newTarget;
    }
  }
}