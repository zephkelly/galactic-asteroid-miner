using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace zephkelly
{
  public class ShipLightFlash : MonoBehaviour
  {
    [SerializeField] float maximumLightIntensity = 3f;
    [SerializeField] float flashSpeed = 2f;

    private Light2D redLight;

    public void Awake()
    {
      redLight = gameObject.GetComponent<Light2D>();
    }

    public void Update()
    {
      redLight.intensity = Mathf.PingPong(Time.time * flashSpeed, maximumLightIntensity);
    }
  }
}