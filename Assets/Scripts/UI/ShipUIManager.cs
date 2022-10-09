using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace zephkelly
{
  public class ShipUIManager : MonoBehaviour
  {
    public static ShipUIManager Instance;

    private TextMeshProUGUI fullCargoText;

    private TextMeshProUGUI pickupText1;
    private TextMeshProUGUI pickupText2;
    private TextMeshProUGUI pickupText3;
    private TextMeshProUGUI pickupText4;
    private TextMeshProUGUI pickupText5;
    private TextMeshProUGUI pickupText6;

    private float fadeTime = 5f;
    private float textFadeTimer1;
    private float textFadeTimer2;
    private float textFadeTimer3;
    private float textFadeTimer4;
    private float textFadeTimer5;
    private float textFadeTimer6;

    private bool canDisplayPickup;

    private Dictionary<int, TextMeshProUGUI> pickupTexts = new Dictionary<int, TextMeshProUGUI>();
    private Dictionary<int, float> textFadeTimers = new Dictionary<int, float>();

    public UnityEvent OnCargoFull;
    public UnityEvent OnEmptyCargo;

    private void Awake()
    {
      if (Instance == null)
      {
        Instance = this;
      } else {
        Destroy(gameObject);
      }
    }

    private void Start()
    {
      fullCargoText = GameObject.Find("FullCargoText").GetComponent<TextMeshProUGUI>();
      fullCargoText.color = new Color(fullCargoText.color.r, fullCargoText.color.g, fullCargoText.color.b, 0f);

      pickupText1 = GameObject.Find("PickupText1").GetComponent<TextMeshProUGUI>();
      pickupText2 = GameObject.Find("PickupText2").GetComponent<TextMeshProUGUI>();
      pickupText3 = GameObject.Find("PickupText3").GetComponent<TextMeshProUGUI>();
      pickupText4 = GameObject.Find("PickupText4").GetComponent<TextMeshProUGUI>();
      pickupText5 = GameObject.Find("PickupText5").GetComponent<TextMeshProUGUI>();
      pickupText6 = GameObject.Find("PickupText6").GetComponent<TextMeshProUGUI>();


      pickupTexts.Add(0, pickupText1);
      pickupTexts.Add(1, pickupText2);
      pickupTexts.Add(2, pickupText3);
      pickupTexts.Add(3, pickupText4);
      pickupTexts.Add(4, pickupText5);
      pickupTexts.Add(5, pickupText6);

      textFadeTimers.Add(0, textFadeTimer1);
      textFadeTimers.Add(1, textFadeTimer2);
      textFadeTimers.Add(2, textFadeTimer3);
      textFadeTimers.Add(3, textFadeTimer4);
      textFadeTimers.Add(4, textFadeTimer5);
      textFadeTimers.Add(5, textFadeTimer6);

      OnCargoFull.AddListener(DisplayFullCargo);
      OnEmptyCargo.AddListener(DisplayPickups);
    }

    private void Update()
    {
      if (!canDisplayPickup) return;

      foreach (var pickup in pickupTexts)
      {
        if (textFadeTimers[pickup.Key] > 0)
        {
          textFadeTimers[pickup.Key] -= Time.deltaTime;
        }
        else {
          if (String.Equals(pickupTexts[pickup.Key].text, "")) return;

          pickupTexts[pickup.Key].text = "";
          textFadeTimers[pickup.Key] = 0;
        }
      }
    }

    public void AddNewPickupText(string text)
    {
      if (!canDisplayPickup) {
        return;
      }

      ReorderDictionary();

      textFadeTimers[0] = fadeTime;
      pickupTexts[0].text = text.ToUpper();
    }

    private void ReorderDictionary()
    {
      for (int i = pickupTexts.Count - 1; i > 0; i--)
      {
        pickupTexts[i].text = pickupTexts[i - 1].text;
        textFadeTimers[i] = textFadeTimers[i - 1];

        switch (i)
        {
          case 0:
            pickupTexts[i].color = new Color(1, 1, 1, 1f);
            break;
          case 1:
            pickupTexts[i].color = new Color(1, 1, 1, 0.74f);
            break;
          case 2:
            pickupTexts[i].color = new Color(1, 1, 1, 0.6f);
            break;
          case 3:
            pickupTexts[i].color = new Color(1, 1, 1, 0.4f);
            break;
          case 4:
            pickupTexts[i].color = new Color(1, 1, 1, 0.2f);
            break;
          case 5:
            pickupTexts[i].color = new Color(1, 1, 1, 0.05f);
            break;
        }
      }
    }

    private void DisplayFullCargo()
    {
      canDisplayPickup = false;

      fullCargoText.color = new Color(
        fullCargoText.color.r,
        fullCargoText.color.g,
        fullCargoText.color.b, 1f
      );

      foreach (var pickupText in pickupTexts)
      {
        pickupText.Value.text = "";
      }
    }

    private void DisplayPickups()
    {
      canDisplayPickup = true;

      fullCargoText.color = new Color(
        fullCargoText.color.r,
        fullCargoText.color.g,
        fullCargoText.color.b, 0f
      );
    }
  }
}
