using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class InputManager : MonoBehaviour
  {
    public static InputManager Instance;

    [SerializeField] Vector2 keyboardInputNormalized;
    private Vector2 keyboardInput;
    private float keyboardX;
    private float keyboardY;

    private Vector2 mouseWorldPosition;
    private float mouseX;
    private float mouseY;

    public Vector2 MouseWorldPosition { get { return mouseWorldPosition; } }
    public Vector2 KeyboardInput { get { return keyboardInputNormalized; } }
  
    private void Awake()
    {
      if (Instance == null)
      {
        Instance = this;
      } else {
        Destroy(gameObject);
      }
    }

    private void Update()
    {
      UpdateMouseInput();
      UpdateKeyboardInput();
    }

    private void UpdateMouseInput()
    {
      mouseWorldPosition = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void UpdateKeyboardInput()
    {
      keyboardX = Input.GetAxisRaw("Horizontal");
      keyboardY = Input.GetAxisRaw("Vertical");

      keyboardInput.x = keyboardX;
      keyboardInput.y = keyboardY;

      keyboardInputNormalized = keyboardInput.normalized;
    }

    private void OnApplicationQuit()
    {
      InputManager.Instance = null;
    }
  }
}