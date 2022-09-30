using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class ShipStarCompass : MonoBehaviour
  {
    private Camera mainCamera;
    private Transform cameraTransform;
    private GameObject pointer;

    [SerializeField] Canvas parentCanvas;
    [SerializeField] float border = 10f;

    private Dictionary<Vector2, RectTransform> pointers = 
      new Dictionary<Vector2, RectTransform>();

    //------------------------------------------------------------------------------

    private void Awake()
    {
      pointer = Resources.Load<GameObject>("Prefabs/UI/StarPointer");
      mainCamera = Camera.main;
      cameraTransform = mainCamera.transform;
    }

    public void UpdateCompass()
    {
      ResetCurrentPointers();
      GetActiveStars();
    }

    private void ResetCurrentPointers()
    {
      foreach (var pointer in pointers) {
        Destroy(pointer.Value.gameObject);
      }

      pointers.Clear();
    }

    private void GetActiveStars()
    {
      foreach (var lazyChunk in ChunkManager.Instance.LazyChunks)
      {
        if (lazyChunk.Value.HasStar == false) continue;

        GameObject newPointer = Object.Instantiate(pointer);
        newPointer.transform.SetParent(parentCanvas.transform);
        RectTransform rectTransform = newPointer.GetComponent<RectTransform>();

        pointers.Add(lazyChunk.Value.Position, rectTransform);
      }

      foreach (var activeChunk in ChunkManager.Instance.ActiveChunks)
      {
        if (pointers.ContainsKey(activeChunk.Value.Position)) continue;
        if (activeChunk.Value.HasStar == false) continue;

        GameObject newPointer = Object.Instantiate(pointer);
        newPointer.transform.SetParent(parentCanvas.transform);
        RectTransform rectTransform = newPointer.GetComponent<RectTransform>();

        pointers.Add(activeChunk.Value.Position, rectTransform);
      }

      GameObject basePointer = Object.Instantiate(pointer);
      basePointer.transform.SetParent(parentCanvas.transform);
      RectTransform baseRect = basePointer.GetComponent<RectTransform>();
      basePointer.GetComponentInChildren<UnityEngine.UI.Image>().color = new Color(0, 0.7f, 1, 1);
      pointers.Add(Vector2.zero, baseRect);
    }

    private void LateUpdate()
    {
      if (pointers.Count == 0) return;

      foreach (var pointer in pointers)
      {
        Transform pointerTransform = pointer.Value;
        Vector2 targetPosition = pointer.Key;

        Vector2 screenPosition = mainCamera.WorldToScreenPoint(targetPosition);
        Vector2 cappedScreenPosition = screenPosition;

        bool isOnScreen;

        RotatePointer();
        IsPointerOnScreen();
        ClampPointerToScreen();
        MovePointer();

        //----------------------------------------------------------------------------

        void RotatePointer()   //Get direction and rotate pointer to angle
        {
          Vector2 direction = targetPosition - (Vector2)cameraTransform.position;
          direction.Normalize();

          float angleToStar = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

          pointerTransform.localEulerAngles = new Vector3(0, 0, angleToStar);
        }

        void IsPointerOnScreen()
        {
          if (screenPosition.x > 0 && screenPosition.x < Screen.width &&
              screenPosition.y > 0 && screenPosition.y < Screen.height)
          {
            isOnScreen = true;
          }
          else
          {
            isOnScreen = false;
          }
        }

        void ClampPointerToScreen()
        {
          if (!isOnScreen)
          {
            float cappedPositionX = Mathf.Clamp(screenPosition.x, border, Screen.width - border);
            float cappedPositionY = Mathf.Clamp(screenPosition.y, border, Screen.height - border);

            cappedScreenPosition = new Vector2(cappedPositionX, cappedPositionY);
          }
        }

        void MovePointer()
        {
          if (isOnScreen)
          {
            pointerTransform.position = screenPosition;
          }
          else
          {
            pointerTransform.position = cappedScreenPosition;
          }
        }
      }
    }
  }
}