using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sjabloon;

public class BrainwaveDevice : MonoBehaviour
{
    [SerializeField]
    private CharacterManager m_CharacterManager;

    [SerializeField]
    private int m_DeviceID;

    [SerializeField]
    private float m_CirclesToMax = 1;

    [SerializeField]
    private float m_ErrorMargin;

    private float m_Frequency;
    public float Frequency
    {
        get { return m_Frequency; }
    }
    private float m_PreviousFrequencyAngle;

    private float m_Amplitude;
    public float Amplitude
    {
        get { return m_Amplitude; }
    }
    private float m_PreviousAmplitudeAngle;

    private float m_BrainPowerLeft;
    public float BrainPowerLeft
    {
        get { return m_BrainPowerLeft; }
    }

    private float m_BrainPowerRight;
    public float BrainPowerRight
    {
        get { return m_BrainPowerRight; }
    }

    private Character m_Target;

    //Cache
    private InputManager m_InputManager;

    private void Start()
    {
        m_InputManager = InputManager.Instance;

        m_InputManager.BindAxis("Frequency_X_" + m_DeviceID, m_DeviceID, ControllerAxisCode.LeftStickX);
        m_InputManager.BindAxis("Frequency_Y_" + m_DeviceID, m_DeviceID, ControllerAxisCode.LeftStickY);

        m_InputManager.BindAxis("Amplitude_X_" + m_DeviceID, m_DeviceID, ControllerAxisCode.RightStickX);
        m_InputManager.BindAxis("Amplitude_Y_" + m_DeviceID, m_DeviceID, ControllerAxisCode.RightStickY);

        m_InputManager.BindAxis("BrainPowerLeft_" + m_DeviceID, m_DeviceID, ControllerAxisCode.LeftTrigger);
        m_InputManager.BindAxis("BrainPowerRight_" + m_DeviceID, m_DeviceID, ControllerAxisCode.RightTrigger);
    }

    private void Update()
    {
        UpdateFrequency();
        UpdateAmplitude();

        UpdateHacking();
        UpdateCommand();
    }

    private void UpdateFrequency()
    {
        //Gather input
        float newFrequencyX = m_InputManager.GetAxis("Frequency_X_" + m_DeviceID);
        float newFrequencyY = m_InputManager.GetAxis("Frequency_Y_" + m_DeviceID);

        Vector2 newFrequencyVector = new Vector2(newFrequencyX, newFrequencyY);
        float distance = newFrequencyVector.magnitude;

        if (distance < 0.5f)
        {
            m_Frequency = 0.0f;
            return;
        }

        newFrequencyVector.Normalize();

        float angle = Mathf.Atan2(newFrequencyVector.y, newFrequencyVector.x) * Mathf.Rad2Deg;
        if (angle < 0.0f) { angle += 360.0f; }

        float diffAngle = m_PreviousFrequencyAngle - angle;

        //We probably made a turn
        if (diffAngle < -350.0f) { diffAngle += 360.0f; }
        if (diffAngle > 350.0f)  { diffAngle -= 360.0f; }

        m_Frequency += (diffAngle / 360) / m_CirclesToMax;
        m_Frequency = Mathf.Clamp01(m_Frequency);

        m_PreviousFrequencyAngle = angle;
    }

    private void UpdateAmplitude()
    {
        //Gather input
        float newAmplitudeX = m_InputManager.GetAxis("Amplitude_X_" + m_DeviceID);
        float newAmplitudeY = m_InputManager.GetAxis("Amplitude_Y_" + m_DeviceID);

        Vector2 newAmplitudeVector = new Vector2(newAmplitudeX, newAmplitudeY);
        float distance = newAmplitudeVector.magnitude;

        if (distance < 0.5f)
        {
            m_Amplitude = 0.0f;
        }

        newAmplitudeVector.Normalize();

        float angle = Mathf.Atan2(newAmplitudeVector.y, newAmplitudeVector.x) * Mathf.Rad2Deg;
        if (angle < 0.0f) { angle += 360.0f; }

        float diffAngle = m_PreviousAmplitudeAngle - angle;

        //We probably made a turn
        if (diffAngle < -350.0f) { diffAngle += 360.0f; }
        if (diffAngle > 350.0f) { diffAngle -= 360.0f; }

        m_Amplitude += (diffAngle / 360) / m_CirclesToMax;
        m_Amplitude = Mathf.Clamp01(m_Amplitude);

        m_PreviousAmplitudeAngle = angle;
    }

    private void UpdateHacking()
    {
        //Look for a new target
        m_Target = null;

        //Get all the characters
        List<Character> characters = m_CharacterManager.Characters;

        foreach (Character character in characters)
        {
            //Compare frequency
            bool frequencyHacked = false;
            if (character.Frequency > (m_Frequency - m_ErrorMargin) &&
                character.Frequency < (m_Frequency + m_ErrorMargin))
            {
                frequencyHacked = true;
            }

            //Compare amplitude
            bool amplitudeHacked = false;
            if (character.Amplitude > (m_Amplitude - m_ErrorMargin) &&
                character.Amplitude < (m_Amplitude + m_ErrorMargin))
            {
                amplitudeHacked = true;
            }

            //We hacked a character!
            if (frequencyHacked && amplitudeHacked)
            {
                character.FullyHacked();
                m_Target = character;
                return;
            }

            //Almost there, give feedback!
            else if (frequencyHacked || amplitudeHacked)
            {
                character.HalfHacked();
            }
        }
    }

    private void UpdateCommand()
    {
        if (m_Target == null)
            return;

        //Gather input
        m_BrainPowerLeft = m_InputManager.GetAxis("BrainPowerLeft_" + m_DeviceID);
        m_BrainPowerRight = m_InputManager.GetAxis("BrainPowerRight_" + m_DeviceID);

        bool useLeftBrainPower = (m_BrainPowerLeft >= 1.0f);
        bool useRightBrainPower = (m_BrainPowerRight >= 1.0f);

        if (useLeftBrainPower == true || useRightBrainPower == true)
        {
            m_Target.SendBrainCommand(useLeftBrainPower, useRightBrainPower);
        }
    }
}
