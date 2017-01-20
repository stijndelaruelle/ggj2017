using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sjabloon;

public class InputTester : MonoBehaviour
{
    private void Start()
    {
        InputManager.Instance.BindButton("Test", KeyCode.A, InputManager.ButtonState.OnRelease);
        InputManager.Instance.BindButton("Test_2", 0, ControllerButtonCode.A, InputManager.ButtonState.OnRelease);
    }

    private void Update()
    {
        bool keyboardPressed = InputManager.Instance.GetButton("Test");
        bool controllerPressed = InputManager.Instance.GetButton("Test_2");

        if (keyboardPressed)
        {
            Debug.Log("Keyboard pressed");
        }

        if (controllerPressed)
        {
            Debug.Log("Controller pressed");
        }
    }
}
