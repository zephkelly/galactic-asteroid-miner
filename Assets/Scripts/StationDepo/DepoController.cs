using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace zephkelly
{
  public class DepoController : MonoBehaviour 
  {
    private void OnTriggerEnter2D(Collider2D other)
    {
      if (other.CompareTag("Star"))
      {
        Debug.Log("Depo entered star");
      }

      if (!other.CompareTag("Player")) return;

      DepoUIManager.Instance.OnHoverDepo?.Invoke();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
      if (!other.CompareTag("Player")) return;

      DepoUIManager.Instance.OnLeaveDepoHover?.Invoke();
    }
  }
}