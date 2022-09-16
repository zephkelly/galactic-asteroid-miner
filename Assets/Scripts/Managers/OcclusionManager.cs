using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class OcclusionManager : MonoBehaviour
  {
    public static OcclusionManager Instance;

    private void Awake()
    {
      if (Instance == null)
      {
        Instance = this;
      } else {
        Destroy(gameObject);
      }
    }

    private void OnApplicationQuit()
    {
      OcclusionManager.Instance = null;
    }
  }
}
  