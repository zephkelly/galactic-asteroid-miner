using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

namespace zephkelly.UI
{
  public class ShipStarCompass : MonoBehaviour
  {
    private Transform playerTransform;
    private Camera mainCamera;
    private Transform cameraTransform;
    private GameObject pointerStar;
    private GameObject pointerDepo;


    [SerializeField] Canvas parentCanvas;
    private float borderX = 25f;
    private float borderY = 20f;

    private Dictionary<Vector2, Pointer> pointers = new Dictionary<Vector2, Pointer>();

    private Pointer closestBasePointer;
    private Pointer closestStarPointer;

    //------------------------------------------------------------------------------

    private void Awake()
    {
      pointerStar = Resources.Load<GameObject>("Prefabs/UI/StarPointer");
      pointerDepo = Resources.Load<GameObject>("Prefabs/UI/DepoPointer");

      mainCamera = Camera.main;
      cameraTransform = mainCamera.transform;
    }

    private void Start()
    {
      playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
      if (playerTransform == null) return;
      if (pointers.Count == 0) return;

      foreach (var pointer in pointers)
      {
        Vector2 targetPosition = pointer.Key;
        Pointer pointerInfo = pointer.Value;

        pointerInfo.UpdateTargetInfo(Vector2.Distance(targetPosition, playerTransform.position), targetPosition);

        Vector2 screenPosition = mainCamera.WorldToScreenPoint(targetPosition);
        Vector2 cappedScreenPosition = screenPosition;

        bool isOnScreen = false;

        RotatePointer();
        IsPointerOnScreen();
        ClampPointerToScreen();
        MovePointer();
        CheckForClosestPointers();

        void RotatePointer()   //Get direction and rotate pointerStar to angle
        {
          Vector2 direction = targetPosition - (Vector2)cameraTransform.position;
          direction.Normalize();

          float angleToStar = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

          pointerInfo.PointerImageContainer.transform.rotation = Quaternion.Euler(0, 0, angleToStar);
        }

        void IsPointerOnScreen()
        {
          if (screenPosition.x > 0 && screenPosition.x < Screen.width &&
              screenPosition.y > 0 && screenPosition.y < Screen.height)
          {
            if (!pointerInfo.PointerObject.activeSelf) return;
            isOnScreen = true;
            pointerInfo.PointerObject.SetActive(false);
          }
          else
          {
            if (pointerInfo.PointerObject.activeSelf) return;
            isOnScreen = false;
            pointerInfo.PointerObject.SetActive(true);
          }
        }

        void ClampPointerToScreen()
        {
          if (!isOnScreen)
          {
            float cappedPositionX = Mathf.Clamp(screenPosition.x, borderX, Screen.width - borderX);
            float cappedPositionY = Mathf.Clamp(screenPosition.y, borderY, Screen.height - (borderY + 10f));

            cappedScreenPosition = new Vector2(cappedPositionX, cappedPositionY);
            pointerInfo.UpdateScreenPosition(cappedScreenPosition);
            pointerInfo.UpdateMoveDirection();
          }
        }

        void MovePointer()
        {
          if (isOnScreen)
          {
            pointerInfo.PointerObject.transform.position = screenPosition;
          }
          else
          {
            foreach (var otherPointer in pointers)
            {
              if (otherPointer.Value == pointerInfo) continue;
              var distance = Vector2.Distance(otherPointer.Value.ScreenPosition, cappedScreenPosition);

              var lastMoveDirectionOfOtherPointer = otherPointer.Value.MoveDirection;

              if (distance < 50f)
              {
                if (lastMoveDirectionOfOtherPointer == Vector2.zero)
                {
                  pointerInfo.UpdateScreenPosition(cappedScreenPosition + pointerInfo.MoveDirection * 50f);
                  pointerInfo.PointerObject.transform.position = cappedScreenPosition + pointerInfo.MoveDirection * 50f;

                  pointerInfo.ResetMoveDirection();
                  // otherPointer.Value.UpdateMoveDirection();
                }
                else
                {
                  otherPointer.Value.UpdateScreenPosition(otherPointer.Value.ScreenPosition + lastMoveDirectionOfOtherPointer * 50f);
                  otherPointer.Value.PointerObject.transform.position = otherPointer.Value.ScreenPosition + lastMoveDirectionOfOtherPointer * 50f;

                  otherPointer.Value.ResetMoveDirection();
                  // pointerInfo.UpdateMoveDirection();
                }
              }
              else
              {
                pointerInfo.PointerObject.transform.position = cappedScreenPosition;
              }
            }
          }
        }

        // void MovePointer()
        // {
        //   if (isOnScreen)
        //   {
        //     pointerInfo.PointerObject.transform.position = screenPosition;
        //   }
        //   else
        //   {
        //     pointerInfo.PointerObject.transform.position = cappedScreenPosition;
        //   }
        // }

        void CheckForClosestPointers()
        {
          switch (pointer.Value.PointerType)
          {
            case PointerType.Star:
              if (closestStarPointer == null) 
              {
                closestStarPointer = pointer.Value;
                return;
              }
              if (pointer.Value.PointerDistance < closestStarPointer.PointerDistance)
              {
                closestStarPointer = pointer.Value;
              }
              break;
            case PointerType.Station:
              if (closestBasePointer == null) 
              {
                closestBasePointer = pointer.Value;
                return;
              }
              if (pointer.Value.PointerDistance < closestBasePointer.PointerDistance)
              {
                closestBasePointer = pointer.Value;
              }
              break;
          }
        }
      }
    }

    public void UpdateCompass()
    {
      ResetCurrentPointers();
      GetActiveObjects();
      // GetActiveDepos();
    }

    private void ResetCurrentPointers()
    {
      List<Vector2> disposablePointers = new List<Vector2>();

      foreach (var pointerStar in pointers)
      {
        if (pointerStar.Value == closestBasePointer || pointerStar.Value == closestStarPointer) continue;

        disposablePointers.Add(pointerStar.Key);
        Destroy(pointerStar.Value.gameObject);
      }

      foreach (var pointer in disposablePointers)
      {
        pointers.Remove(pointer);
      }
    }

    private void GetActiveObjects()
    {
      //lazyStars
      foreach (var lazyChunk in ChunkManager.Instance.LazyChunks)
      {
        if (lazyChunk.Value.HasStar == false) continue;
        if (lazyChunk.Value.ChunkStar.SpawnPoint != null && pointers.ContainsKey(lazyChunk.Value.ChunkStar.SpawnPoint)) continue;
        if (pointers.ContainsKey(lazyChunk.Value.Position)) continue;

        var starPosition = lazyChunk.Value.ChunkStar.SpawnPoint;
        var starType = lazyChunk.Value.ChunkStar.Type;

        GameObject newStarPointer = Object.Instantiate(pointerStar) as GameObject;
        newStarPointer.transform.SetParent(parentCanvas.transform);

        var starPointer = newStarPointer.GetComponent<Pointer>();
        starPointer.SetupStarPointer(starType);

        pointers.Add(starPosition, starPointer);
      }

      //ActiveStars
      foreach (var activeChunk in ChunkManager.Instance.ActiveChunks)
      {
        if (activeChunk.Value.HasStar == false) continue;
        if (activeChunk.Value.ChunkStar.SpawnPoint != null && pointers.ContainsKey(activeChunk.Value.ChunkStar.SpawnPoint)) continue;
        if (pointers.ContainsKey(activeChunk.Value.Position)) continue;

        var starPosition = activeChunk.Value.ChunkStar.SpawnPoint;
        var starType = activeChunk.Value.ChunkStar.Type;

        GameObject newStarPointer = Object.Instantiate(pointerStar) as GameObject;
        newStarPointer.transform.SetParent(parentCanvas.transform);

        var starPointer = newStarPointer.GetComponent<Pointer>();
        starPointer.SetupStarPointer(starType);

        pointers.Add(starPosition, starPointer);
      }

      //Lazy Depos
      foreach (var lazyChunk in ChunkManager.Instance.LazyChunks)
      {
        if (lazyChunk.Value.HasDepo == false) continue;
        if (lazyChunk.Value.ChunkDepo.SpawnPoint != null && pointers.ContainsKey(lazyChunk.Value.ChunkDepo.SpawnPoint)) continue;
        if (pointers.ContainsKey(lazyChunk.Value.Position)) continue;

        var depoPosition = lazyChunk.Value.ChunkDepo.SpawnPoint;

        GameObject newDepoPointer = Object.Instantiate(pointerDepo) as GameObject;
        newDepoPointer.transform.SetParent(parentCanvas.transform);

        var depoPointer = newDepoPointer.GetComponent<Pointer>();
        depoPointer.SetupDepoPointer();

        pointers.Add(depoPosition, depoPointer);
      }

      //Active Depos
      foreach (var activeChunk in ChunkManager.Instance.ActiveChunks)
      {
        if (activeChunk.Value.HasDepo == false) continue;
        if (activeChunk.Value.ChunkDepo.SpawnPoint != null && pointers.ContainsKey(activeChunk.Value.ChunkDepo.SpawnPoint)) continue;
        if (pointers.ContainsKey(activeChunk.Value.Position)) continue;

        var depoPosition = activeChunk.Value.ChunkDepo.SpawnPoint;

        GameObject newDepoPointer = Object.Instantiate(pointerDepo) as GameObject;
        newDepoPointer.transform.SetParent(parentCanvas.transform);

        var depoPointer = newDepoPointer.GetComponent<Pointer>();
        depoPointer.SetupDepoPointer();

        pointers.Add(depoPosition, depoPointer);
      }
    }

    private Color GetPointerColor(StarType starType)
    {
      switch (starType)
      {
        case StarType.WhiteDwarf:
          return new Color(1, 1f, 1, 0.7f);
        case StarType.RedGiant:
          return new Color(1, 0.2f, 0.2f, 0.7f);
        default:
          return new Color(1, 1f, 1, 0.7f);
      }
    }
  }
}