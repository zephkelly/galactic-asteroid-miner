using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class CameraController : MonoBehaviour
  {
    public static CameraController Instance;
    private InputManager inputs;
    private Camera mainCamera;
    private Transform cameraTransform;

    private Transform target;
    private Rigidbody2D playerRigidbody;
    private float playerVelocityMagnitude;

    private Vector3 mouseLerpPosition;
    private Vector3 currentOffsetAmount;

    [SerializeField] float mouseInterpolateDistance = 2f;
    [SerializeField] float cameraPanSpeed = 0.125f;

    //Parallaxing-layers----------------------------------------------------------------------------

    [SerializeField] ParallaxStarfield[] starfieldsLayers;
    [SerializeField] DepoParallax depoParallax;

    //----------------------------------------------------------------------------------------------

    private void Awake()
    {
      mainCamera = Camera.main;
      cameraTransform = mainCamera.transform;

      if (Instance == null)
      {
        Instance = this;
      } else {
        Destroy(gameObject);
      }
    }

    private void Start()
    {
      target = GameObject.Find("Player").transform;
      playerRigidbody = target.GetComponent<Rigidbody2D>();
      inputs = InputManager.Instance;
    }

    private void Update()
    {
      if (target == null) return;

      mouseLerpPosition = (mainCamera.ScreenToWorldPoint(Input.mousePosition) - target.position).normalized;
      mouseLerpPosition.y = mouseLerpPosition.y * 1.4f;   //beacuse the camera is wider than it is tall

      playerVelocityMagnitude = playerRigidbody.velocity.magnitude;

      if (playerVelocityMagnitude > (playerVelocityMagnitude * 0.85f)) {
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, 22 + (playerVelocityMagnitude * 0.2f), 0.1f);
      }
    }

    private void FixedUpdate()
    {
      if (target == null) return;

      Vector3 targetVector = target.position + currentOffsetAmount + (mouseLerpPosition * mouseInterpolateDistance * (playerVelocityMagnitude * 0.05f));      

      targetVector.z = cameraTransform.position.z;

      Vector3 cameraLastPosition = cameraTransform.position;
      cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetVector, cameraPanSpeed * (Mathf.Clamp(playerVelocityMagnitude, 25, 50) * 0.03f));
      
      UpdateParllaxing(cameraLastPosition);
    }

    private void UpdateParllaxing(Vector2 cameraLastPosition)
    {
      //Starfields
      for (int i = 0; i < starfieldsLayers.Length; i++)
      {
        starfieldsLayers[i].Parallax(cameraLastPosition);
      }

      //Home depo
      depoParallax.Parallax(cameraLastPosition);
    }

    public void ChangeFocus(Transform newFocus) => target = newFocus;

    public void SetOffset(Vector2 offsetVector) => currentOffsetAmount = offsetVector;

    public void ClearOffset() => currentOffsetAmount = Vector3.zero;
  }
}
