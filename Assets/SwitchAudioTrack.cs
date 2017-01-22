using Sjabloon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchAudioTrack : MonoBehaviour {

    [SerializeField] AudioSource source;
    [SerializeField] AudioClip clip;

    [SerializeField]
    private ControllerButtonCode m_ButtonCode;

    [SerializeField]
    private KeyCode m_KeyCode;

    private InputManager m_InputManager;

    private void Start()
    {
        m_InputManager = InputManager.Instance;

        m_InputManager.BindButton("Controller_ChangeAudio_" + clip.name, 0, m_ButtonCode, InputManager.ButtonState.Pressed);
        m_InputManager.BindButton("Keyboard_ChangeAudio_" + clip.name, m_KeyCode, InputManager.ButtonState.Pressed);
    }

    private void OnDestroy()
    {
        if (m_InputManager == null)
            return;

        m_InputManager.UnbindButton("Controller_ChangeAudio_" + clip.name);
        m_InputManager.UnbindButton("Keyboard_ChangeAudio_" + clip.name);
    }

    private void Update()
    {
        bool controller = m_InputManager.GetButton("Controller_ChangeAudio_" + clip.name);
        bool keyboard = m_InputManager.GetButton("Keyboard_ChangeAudio_" + clip.name);

        if (controller || keyboard)
        {
            ChangeAudio();
        }
    }

    public void ChangeAudio()
    {
        float time = source.time;
        source.clip = clip;
        source.Play();
        source.time = time;
    }
}
