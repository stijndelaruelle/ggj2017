using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sjabloon;

public class RegisterInput : MonoBehaviour
{
    private InputManager m_InputManger;

    private void Start()
    {
        m_InputManger = InputManager.Instance;

        m_InputManger.BindButton("Keyboard_Submit", KeyCode.Return, InputManager.ButtonState.OnRelease);
        m_InputManger.BindButton("Controller_Submit", 0, ControllerButtonCode.A, InputManager.ButtonState.OnRelease);

        m_InputManger.BindButton("Keyboard_Cancel", KeyCode.Escape, InputManager.ButtonState.OnRelease);
        m_InputManger.BindButton("Controller_Cancel", 0, ControllerButtonCode.A, InputManager.ButtonState.OnRelease);
    }

    private void OnDestroy()
    {
        if (m_InputManger == null)
            return;

        m_InputManger.UnbindButton("Keyboard_Submit");
        m_InputManger.UnbindButton("Controller_Submit");

        m_InputManger.UnbindButton("Keyboard_Cancel");
        m_InputManger.UnbindButton("Controller_Cancel");
    }
}
