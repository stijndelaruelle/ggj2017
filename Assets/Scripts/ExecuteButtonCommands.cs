using Sjabloon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExecuteButtonCommands : MonoBehaviour
{
    [SerializeField]
    private Button m_Button;

    [SerializeField]
    private ControllerButtonCode m_ButtonCode;

    [SerializeField]
    private KeyCode m_KeyCode;

    private InputManager m_InputManger;

    //Chea fix
    private bool m_SkipFirstFrame = false;

    private void Start()
    {
        m_InputManger = InputManager.Instance;

        m_InputManger.BindButton("Controller_Button_" + m_Button.name, 0, m_ButtonCode, InputManager.ButtonState.OnRelease);
        m_InputManger.BindButton("Keyboard_Button_" + m_Button.name, m_KeyCode, InputManager.ButtonState.OnRelease);
    }

    private void OnDestroy()
    {
        if (m_InputManger == null)
            return;

        m_InputManger.UnbindButton("Controller_Button_" + m_Button.name);
        m_InputManger.UnbindButton("Keyboard_Button_" + m_Button.name);
    }

    private void Update()
    {
        if (m_SkipFirstFrame == false)
        {
            m_SkipFirstFrame = true;
            return;
        }

        bool controllerSubmit = InputManager.Instance.GetButton("Controller_Button_" + m_Button.name);
        bool keyboardSubmit = InputManager.Instance.GetButton("Keyboard_Button_" + m_Button.name);

        if (controllerSubmit || keyboardSubmit)
        {
            ExecuteButton();
        }
    }

    public void ExecuteButton()
    {
        m_Button.onClick.Invoke();
    }
}
