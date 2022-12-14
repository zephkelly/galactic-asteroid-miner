using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace zephkelly
{
  public class Pointer : MonoBehaviour
  {
    private GameObject _pointerObject;
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] Image _pointerImage;
    [SerializeField] Transform _pointerImageContainer;

    public Image PointerImage { get => _pointerImage; }
    public GameObject PointerObject { get => _pointerObject; }
    public Transform PointerImageContainer { get => _pointerImageContainer; }
    public TextMeshProUGUI Distance { get => _text; }

    private void Start()
    {
      _pointerObject = this.gameObject;
    }

    public void UpdateTargetDistance(float distance)
    {
      _text.text = $"{distance.ToString()}Km";
    }

    public void SetPointerColor(StarType starType)
    {
      switch (starType)
      {
        case StarType.WhiteDwarf:
          _pointerImage.color = new Color(1f, 1f, 1f, 0.7f);
          break;
        case StarType.BrownDwarf:
          _pointerImage.color = new Color(0.58f, 0.38f, 0.1f, 0.7f);
          break;
        case StarType.RedDwarf:
          _pointerImage.color = new Color(1f, 0.18f, 0f, 0.7f);
          break;
        case StarType.YellowDwarf:
          _pointerImage.color = new Color(1f, 0.76f, 0f, 0.7f);
          break;
        case StarType.BlueGiant:
          _pointerImage.color = new Color(0f, 0.28f, 1f, 0.7f);
          break;
        case StarType.OrangeGiant:
          _pointerImage.color = new Color(1f, 0.48f, 0f, 0.7f);
          break;
        case StarType.RedGiant:
          _pointerImage.color = new Color(1f, 0.28f, 0.22f, 0.7f);
          break;
        case StarType.BlueSuperGiant:
          _pointerImage.color = new Color(0f, 0.55f, 1f, 0.7f);
          break;
        case StarType.RedSuperGiant:
          _pointerImage.color = new Color(0.92f, 0f, 0f, 0.7f);
          break;
        case StarType.BlueHyperGiant:
          _pointerImage.color = new Color(0f, 0.6f, 0.92f, 0.7f);
          break;
        case StarType.RedHyperGiant:
          _pointerImage.color = new Color(1f, 0.24f, 0.2f, 0.7f);
          break;
        case StarType.NeutronStar:
          _pointerImage.color = new Color(0.3f, 0.88f, 1f, 0.7f);
          break;
        case StarType.BlackHole:
          _pointerImage.color = new Color(0f, 0f, 0f, 0.8f);
          break;
        
        default:
          _pointerImage.color = Color.white;
          break;
      }
    }

    public void SetBaseColor()
    {
      _pointerImage.color = new Color(0.25f, 1f, 1f, 0.7f);
    }
  }
}
