using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class CameraController : MonoBehaviour
  {
    private InputManager inputs;
    private Camera mainCamera;
    private Transform cameraTransform;

    //----------------------------------------------------------------------------------------------

    [SerializeField] ParallaxStarfield[] parallaxStarfield;
    [SerializeField] float mouseInterpolateDistance = 2f;
    [SerializeField] float cameraPanSpeed = 0.125f;

    private Transform target;
    private Rigidbody2D targetRigid2D;
    private Vector3 mouseLerpPosition;

    //----------------------------------------------------------------------------------------------

    private void Awake()
    {
      mainCamera = Camera.main;
      cameraTransform = mainCamera.transform;
    }

    private void Start()
    {
      target = GameObject.Find("Player").transform;
      targetRigid2D = target.GetComponent<Rigidbody2D>();
      inputs = InputManager.Instance;
    }

    private void Update()
    {
      if (target == null) return;

      mouseLerpPosition = (mainCamera.ScreenToWorldPoint(Input.mousePosition) - target.position).normalized;
      mouseLerpPosition.y = mouseLerpPosition.y * 1.4f;   //beacuse the camera is wider than it is tall

      cameraPanSpeed = 0.125f;
    }

    private void FixedUpdate()
    {
      if (target == null) return;

      Vector3 targetVector = target.position + (mouseLerpPosition * mouseInterpolateDistance);
      targetVector.z = cameraTransform.position.z;

      Vector3 cameraLastPosition = cameraTransform.position;

      cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetVector, cameraPanSpeed);

      //Parallax starfield layers
      for (int i = 0; i < parallaxStarfield.Length; i++)
      {
        parallaxStarfield[i].Parallax(cameraLastPosition);
      }
    }

    public void ChangeFocus(Transform newTarget)
    {
      target = newTarget;
    }
  }
}
