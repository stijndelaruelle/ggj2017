using Sjabloon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDifficulty : MonoBehaviour
{
    [SerializeField]
    private int m_DifficultyMode;

    [SerializeField]
    private ControllerButtonCode m_ButtonCode;

    [SerializeField]
    private KeyCode m_KeyCode;

    private InputManager m_InputManager;

    private void Start()
    {
        m_InputManager = InputManager.Instance;

        m_InputManager.BindButton("Controller_SetDifficulty_" + m_DifficultyMode, 0, m_ButtonCode, InputManager.ButtonState.Pressed);
        m_InputManager.BindButton("Keyboard_SetDifficulty_" + m_DifficultyMode, m_KeyCode, InputManager.ButtonState.Pressed);
    }

    private void OnDestroy()
    {
        if (m_InputManager == null)
            return;

        m_InputManager.UnbindButton("Controller_SetDifficulty_" + m_DifficultyMode);
        m_InputManager.UnbindButton("Keyboard_SetDifficulty_" + m_DifficultyMode);
    }

    private void Update()
    {
        bool controller = m_InputManager.GetButton("Controller_SetDifficulty_" + m_DifficultyMode);
        bool keyboard = m_InputManager.GetButton("Keyboard_SetDifficulty_" + m_DifficultyMode);

        if (controller || keyboard)
        {
            SetDifficultyMode();
        }
    }

    private void OnMouseUp()
    {
        SetDifficultyMode();
    }

    public void SetDifficultyMode()
    {
        PlayerPrefs.SetInt("DifficultyMode", m_DifficultyMode);
    }
}
