﻿using Sjabloon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraTo : MonoBehaviour
{
	[SerializeField] GameObject target;

    [SerializeField]
    private ControllerButtonCode m_ButtonCode;

    [SerializeField]
    private KeyCode m_KeyCode;

    private InputManager m_InputManager;

    private void Start()
    {
        m_InputManager = InputManager.Instance;

        m_InputManager.BindButton("Controller_MoveCamera_" + target.name, 0, m_ButtonCode, InputManager.ButtonState.Pressed);
        m_InputManager.BindButton("Keyboard_MoveCamera_" + target.name, m_KeyCode, InputManager.ButtonState.Pressed);
    }

    private void OnDestroy()
    {
        if (m_InputManager == null)
            return;

        m_InputManager.UnbindButton("Controller_MoveCamera_" + target.name);
        m_InputManager.UnbindButton("Keyboard_MoveCamera_" + target.name);
    }

    private void Update()
    {
        bool controller = m_InputManager.GetButton("Controller_MoveCamera_" + target.name);
        bool keyboard = m_InputManager.GetButton("Keyboard_MoveCamera_" + target.name);

        if (controller || keyboard)
        {
            MoveCamera();
        }
    }

    void OnMouseUp()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        iTween.MoveTo(Camera.main.gameObject, iTween.Hash("y", target.transform.position.y, "x", target.transform.position.x, "easetype", "easeInOutQuad", "time", 0.5f));
    }
}
