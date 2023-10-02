using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace zephkelly
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

    private Dictionary<Vector2, Pointer> pointers =
      new Dictionary<Vector2, Pointer>();

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

    public void UpdateCompass()
    {
      ResetCurrentPointers();
      GetActiveStars();
      // GetActiveDepos();
    }

    private void ResetCurrentPointers()
    {
      foreach (var pointerStar in pointers)
      {
        Destroy(pointerStar.Value.gameObject);
      }

      pointers.Clear();
    }

    private void GetActiveStars()
    {
      //lazyStars
      foreach (var lazyChunk in ChunkManager.Instance.LazyChunks)
      {
        if (lazyChunk.Value.HasStar == false) continue;

        var starPosition = lazyChunk.Value.ChunkStar.SpawnPoint;
        var starType = lazyChunk.Value.ChunkStar.Type;

        GameObject newStarPointer = Object.Instantiate(pointerStar) as GameObject;
        newStarPointer.transform.SetParent(parentCanvas.transform);

        var starPointer = newStarPointer.GetComponent<Pointer>();
        starPointer.SetPointerColor(starType);

        pointers.Add(starPosition, starPointer);
      }

      //ActiveStars
      foreach (var activeChunk in ChunkManager.Instance.ActiveChunks)
      {
        if (pointers.ContainsKey(activeChunk.Value.Position)) continue;
        if (activeChunk.Value.HasStar == false) continue;

        var starPosition = activeChunk.Value.ChunkStar.SpawnPoint;
        var starType = activeChunk.Value.ChunkStar.Type;

        GameObject newStarPointer = Object.Instantiate(pointerStar) as GameObject;
        newStarPointer.transform.SetParent(parentCanvas.transform);

        var starPointer = newStarPointer.GetComponent<Pointer>();
        starPointer.SetPointerColor(starType);

        pointers.Add(starPosition, starPointer);
      }

      //Lazy Depos
      foreach (var lazyChunk in ChunkManager.Instance.LazyChunks)
      {
        if (pointers.ContainsKey(lazyChunk.Value.Position)) continue;
        if (lazyChunk.Value.HasDepo == false) continue;

        var depoPosition = lazyChunk.Value.Position;

        GameObject newDepoPointer = Object.Instantiate(pointerDepo) as GameObject;
        newDepoPointer.transform.SetParent(parentCanvas.transform);

        var depoPointer = newDepoPointer.GetComponent<Pointer>();
        depoPointer.SetBaseColor();

        pointers.Add(depoPosition, depoPointer);
      }

      //Active Depos
      foreach (var activeChunk in ChunkManager.Instance.ActiveChunks)
      {
        if (pointers.ContainsKey(activeChunk.Value.Position)) continue;
        if (activeChunk.Value.HasDepo == false) continue;

        var depoPosition = activeChunk.Value.ChunkDepo.AttachedObject.transform.position;

        GameObject newDepoPointer = Object.Instantiate(pointerDepo) as GameObject;
        newDepoPointer.transform.SetParent(parentCanvas.transform);

        var depoPointer = newDepoPointer.GetComponent<Pointer>();
        depoPointer.SetBaseColor();

        pointers.Add(depoPosition, depoPointer);
      }

      //Depo pointerStar
      GameObject newBasePointer = Object.Instantiate(pointerDepo) as GameObject;
      newBasePointer.transform.SetParent(parentCanvas.transform);

      var basePointer = newBasePointer.GetComponent<Pointer>();
      basePointer.SetBaseColor();

      pointers.Add(Vector2.zero, basePointer);
    }

    private void LateUpdate()
    {
      if (playerTransform == null) return;
      if (pointers.Count == 0) return;

      foreach (var pointerStar in pointers)
      {
        Vector2 targetPosition = pointerStar.Key;
        var pointerInfo = pointerStar.Value;

        var distance = (int)Vector2.Distance(targetPosition, playerTransform.position) / 5;

        pointerInfo.UpdateTargetDistance(distance);

        Vector2 screenPosition = mainCamera.WorldToScreenPoint(targetPosition);
        Vector2 cappedScreenPosition = screenPosition;

        bool isOnScreen = false;

        RotatePointer();
        IsPointerOnScreen();
        ClampPointerToScreen();
        MovePointer();

        //----------------------------------------------------------------------------

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
            pointerInfo.PointerObject.transform.position = cappedScreenPosition;
          }
        }
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