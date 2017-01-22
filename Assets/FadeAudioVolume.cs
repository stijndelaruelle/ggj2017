using Sjabloon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAudioVolume : MonoBehaviour
{
    [SerializeField]
    private AudioSource m_Source;

    [SerializeField]
    private float m_TargetVolume;

    [SerializeField]
    private float m_FadeTime = 0.0f;
    private float m_FadeTimer;
    private float m_StartVolume;

    [SerializeField]
    private ControllerButtonCode m_ButtonCode;

    [SerializeField]
    private KeyCode m_KeyCode;

    private InputManager m_InputManager;

    private void Start()
    {
        m_InputManager = InputManager.Instance;

        m_InputManager.BindButton("Controller_FadeAudio", 0, m_ButtonCode, InputManager.ButtonState.Pressed);
        m_InputManager.BindButton("Keyboard_FadeAudio", m_KeyCode, InputManager.ButtonState.Pressed);

        m_StartVolume = m_Source.volume;
    }

    private void OnDestroy()
    {
        if (m_InputManager == null)
            return;

        m_InputManager.UnbindButton("Controller_FadeAudio");
        m_InputManager.UnbindButton("Keyboard_FadeAudio");
    }

    private void Update()
    {
        bool controller = m_InputManager.GetButton("Controller_FadeAudio");
        bool keyboard = m_InputManager.GetButton("Keyboard_FadeAudio");

        if (controller || keyboard)
        {
            StartFadeAudio();
        }

        if (m_FadeTimer > 0.0f)
        {
            m_FadeTimer -= Time.deltaTime;
            m_Source.volume = Mathf.Lerp(m_StartVolume, m_TargetVolume, 1.0f - (m_FadeTimer / m_FadeTime));
        }
    }

    public void StartFadeAudio()
    {
        m_StartVolume = m_Source.volume;
        m_FadeTimer = m_FadeTime;
    }
}
